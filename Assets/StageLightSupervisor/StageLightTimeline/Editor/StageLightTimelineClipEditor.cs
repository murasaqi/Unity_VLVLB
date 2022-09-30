using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Timeline;
using UnityEditor.Timeline;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;

namespace StageLightSupervisor.StageLightTimeline.Editor
{
    [CustomEditor(typeof(StageLightTimelineClip))]
    public class StageLightTimelineClipEditor : UnityEditor.Editor
    {
        
        
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            var template = EditorGUIUtility.Load("StageLightTimelineClipEditor.uxml") as VisualTreeAsset;
            template.CloneTree(root);
            return root;
        }

        private List<string> mExcluded = new List<string>();

        public override void OnInspectorGUI()
        {
            BeginInspector();
            mExcluded.Clear();
            mExcluded.Add("stageLightSetting");
            // DrawRemainingPropertiesInInspector();
        }
        
        private void BeginInspector()
        {
            serializedObject.Update();
            DrawRemainingPropertiesInInspector();
            var stageLightTimelineClip = serializedObject.targetObject as StageLightTimelineClip;
            stageLightTimelineClip.stageLightProfile.Init();

            if (GUILayout.Button("Load Profile"))
            {
                stageLightTimelineClip.ApplySetting();
            }
            var stageLightProperties = stageLightTimelineClip.stageLightProfile.stageLightProperties;
            var stageLightProfile = new SerializedObject(serializedObject.FindProperty("stageLightProfile").objectReferenceValue);
            
            foreach (var property in stageLightProperties)
            {
                var fields = property.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);

                var propertyOverrideFieldInfo = property.GetType().BaseType.GetField("propertyOverride");
                EditorGUILayout.BeginHorizontal();
                EditorGUI.BeginChangeCheck();
                var enable = EditorGUILayout.Toggle((bool)propertyOverrideFieldInfo.GetValue(property), GUILayout.Width(30));
                if (EditorGUI.EndChangeCheck())
                {
                    propertyOverrideFieldInfo.SetValue(property,enable);
                }
                EditorGUILayout.PrefixLabel(property.GetType().ToString());
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space(2);
                
                EditorGUI.BeginDisabledGroup(!enable);
                using (new EditorGUI.IndentLevelScope())
                {
                    // いろいろ書く。この中のやつがインテントされる。
                      
                    foreach (var fieldInfo in fields)
                    {
                        var fieldType = fieldInfo.FieldType;
                        if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(StageLightValue<>))
                        {
                            var fieldValue = fieldInfo.GetValue(property);
                            var stageLightValueFieldInfo = fieldValue.GetType().GetField("value");
                            var propertyOverride = fieldValue.GetType().BaseType.GetField("propertyOverride");
                            
                            Debug.Log(propertyOverride.GetValue(fieldValue));
                            EditorGUILayout.BeginHorizontal();
                            
                            EditorGUI.BeginChangeCheck();
                            
                            var toggle = EditorGUILayout.Toggle((bool)propertyOverride.GetValue(fieldValue),
                                GUILayout.Width(40));
                            if (EditorGUI.EndChangeCheck())
                            {
                                propertyOverride.SetValue(fieldValue, toggle);
                            }
                            

                            EditorGUI.BeginDisabledGroup(false);
                            var displayName = fieldInfo.GetCustomAttribute<DisplayNameAttribute>();
                            var labelValue = displayName != null ? displayName.name : fieldInfo.Name;

                            object resultValue = null;
                            EditorGUI.BeginChangeCheck();
                            if (stageLightValueFieldInfo.FieldType == typeof(System.Single))
                            {
                                resultValue = EditorGUILayout.FloatField(labelValue,
                                    (float)stageLightValueFieldInfo.GetValue(fieldValue));
                               
                            }

                          if (stageLightValueFieldInfo.FieldType == typeof(System.Int32))
                            {
                                resultValue = EditorGUILayout.IntField(labelValue,
                                    (int)stageLightValueFieldInfo.GetValue(fieldValue));
                            }

                            if (stageLightValueFieldInfo.FieldType == typeof(System.Boolean))
                            {
                                resultValue = EditorGUILayout.Toggle(labelValue,
                                    (bool)stageLightValueFieldInfo.GetValue(fieldValue));
                            }

                            if (stageLightValueFieldInfo.FieldType == typeof(System.String))
                            {
                                resultValue = EditorGUILayout.TextField(labelValue,
                                    (string)stageLightValueFieldInfo.GetValue(fieldValue));
                            }

                            if (stageLightValueFieldInfo.FieldType == typeof(UnityEngine.Color))
                            {
                                resultValue = EditorGUILayout.ColorField(labelValue,
                                    (Color)stageLightValueFieldInfo.GetValue(fieldValue));
                            }

                            if (stageLightValueFieldInfo.FieldType == typeof(UnityEngine.Vector2))
                            {
                                resultValue = EditorGUILayout.Vector2Field(
                                    labelValue,(Vector2)stageLightValueFieldInfo.GetValue(fieldValue));
                            }

                            if (stageLightValueFieldInfo.FieldType == typeof(UnityEngine.Vector3))
                            {
                                resultValue = EditorGUILayout.Vector3Field(
                                    labelValue,(Vector3)stageLightValueFieldInfo.GetValue(fieldValue));
                            }

                            if (stageLightValueFieldInfo.FieldType == typeof(UnityEngine.Vector4))
                            {
                                resultValue = EditorGUILayout.Vector4Field(
                                    labelValue,(Vector4)stageLightValueFieldInfo.GetValue(fieldValue));
                            }

                            if (stageLightValueFieldInfo.FieldType == typeof(UnityEngine.Quaternion))
                            {
                                resultValue = EditorGUILayout.Vector4Field(
                                    labelValue,(Vector4)stageLightValueFieldInfo.GetValue(fieldValue));
                            }

                            if (stageLightValueFieldInfo.FieldType == typeof(UnityEngine.AnimationCurve))
                            {
                                resultValue = EditorGUILayout.CurveField(labelValue,
                                    (AnimationCurve)stageLightValueFieldInfo.GetValue(fieldValue));
                            }
                            
                            if (stageLightValueFieldInfo.FieldType.BaseType != null && stageLightValueFieldInfo.FieldType.BaseType == typeof(System.Enum))
                            {
                                var easeType = stageLightValueFieldInfo.GetValue(fieldValue)as Enum;
                                resultValue = EditorGUILayout.EnumPopup(labelValue,easeType);
                            }

                            if (stageLightValueFieldInfo.FieldType == typeof(UnityEngine.Gradient))
                            {
                                resultValue = EditorGUILayout.GradientField(labelValue,
                                    (Gradient)stageLightValueFieldInfo.GetValue(fieldValue));
                            }
                            
                            if (EditorGUI.EndChangeCheck())
                            {
                                Undo.RecordObject(stageLightProfile.targetObject, "Changed Area Of Effect");
                               if(resultValue !=null) stageLightValueFieldInfo.SetValue(fieldValue, resultValue);
                            }
                            
                            
                            EditorGUILayout.EndHorizontal();
                            EditorGUILayout.Space(2);
                            EditorGUI.EndDisabledGroup();
                        }
                    }
                }
                EditorGUI.EndDisabledGroup();
            }
            
           
            // DrawPropertyInInspector(stageLightProfile.FindProperty("stageLightProperties"));
        }
        
        
        protected void DrawRemainingPropertiesInInspector()
        {
            EditorGUI.BeginChangeCheck();
            DrawPropertiesExcluding(serializedObject, mExcluded.ToArray());
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();
        }
        
        protected void DrawPropertyInInspector(SerializedProperty p)
        {
            if (!IsPropertyExcluded(p.name))
            {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(p);
                if (EditorGUI.EndChangeCheck())
                    serializedObject.ApplyModifiedProperties();
                ExcludeProperty(p.name);
            }
        }
        
        private bool IsPropertyExcluded(string propertyName)
        {
            return mExcluded.Contains(propertyName);
        }

        private void ExcludeProperty(string propertyName)
        {
            mExcluded.Add(propertyName);
        }
        private void OnDisable()
        {
           
        }

        private void OnDestroy()
        {
        }


      

    }
}