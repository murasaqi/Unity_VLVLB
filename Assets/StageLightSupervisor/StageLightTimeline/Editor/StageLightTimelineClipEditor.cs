using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using UnityEngine.Timeline;
using UnityEditor.Timeline;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;
using VLB;
using Object = UnityEngine.Object;

namespace StageLightSupervisor.StageLightTimeline.Editor
{
    [CustomEditor(typeof(StageLightTimelineClip))]
    public class StageLightTimelineClipEditor : UnityEditor.Editor
    {
        
        
        private List<StageLightPropertyEditor> _stageLightPropertyEditors = new List<StageLightPropertyEditor>();

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
            
            _stageLightPropertyEditors.Clear();
            foreach (var property in stageLightProperties)
            {
                
                DrawStageLightPropertyGUI(property, stageLightProfile.targetObject);
                
                // var stageLightPropertyEditor = new StageLightPropertyEditor(property);
                // _stageLightPropertyEditors.Add(stageLightPropertyEditor);
                // stageLightPropertyEditor.DrawGUI(stageLightProfile.targetObject);
            }
            
           
            // DrawPropertyInInspector(stageLightProfile.FindProperty("stageLightProperties"));
        }

        private void DrawRollProperty(FieldInfo[] fields)
        {
            foreach (var fieldInfo in fields)
            {
            }
        }
        

        private void DrawStageLightPropertyGUI(StageLightProperty property, Object undoTarget)
        {
         
            EditorGUILayout.Space(2);
            var fields = property.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic).ToList();
            // var baseFields = typeof(StageLightProperty).GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic); 
            // fields = fields.Except(baseFields).ToArray();
            var propertyOverrideFieldInfo = property.GetType().BaseType.GetField("propertyOverride");

            var orderedFields = new List<FieldInfo>();
            // var bpmOverrideFields = new List<FieldInfo>();
            if (property.GetType().BaseType == typeof(StageLightAdditionalProperty) || property.GetType().BaseType == typeof(RollProperty))
            {
                orderedFields.Add(fields.Find(x=>x.Name == "bpmOverrideData"));
             
                foreach (var f in fields)
                {
                    if(f.Name != "bpmOverrideData")
                        orderedFields.Add(f);
                }
            }
            else
            {
                orderedFields = fields;
            }
           
            
            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginChangeCheck();
            var enable = EditorGUILayout.Toggle((bool)propertyOverrideFieldInfo.GetValue(property), GUILayout.Width(30));
            if (EditorGUI.EndChangeCheck())
            {
                propertyOverrideFieldInfo.SetValue(property,enable);
            }
            
            EditorGUILayout.LabelField(property.propertyName, EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();
            
            EditorGUI.BeginDisabledGroup(!enable);
           

            using (new EditorGUI.IndentLevelScope())
            {

                using (new EditorGUILayout.VerticalScope("GroupBox"))
                {

                    foreach (var fieldInfo in orderedFields)
                    {
                        var fieldType = fieldInfo.FieldType;

                        if (fieldType.IsGenericType &&
                            fieldType.GetGenericTypeDefinition() == typeof(StageLightValue<>))
                        {
                            var fieldValue = fieldInfo.GetValue(property);
                            var stageLightValueFieldInfo = fieldValue.GetType().GetField("value");
                            var propertyOverride = fieldValue.GetType().BaseType.GetField("propertyOverride");
                            var displayName = fieldInfo.GetCustomAttribute<DisplayNameAttribute>();
                            var labelValue = displayName != null ? displayName.name : fieldInfo.Name;
                            EditorGUILayout.BeginHorizontal();

                            EditorGUI.BeginChangeCheck();

                            var propertyOverrideToggle = false;

                            propertyOverrideToggle = EditorGUILayout.Toggle((bool)propertyOverride.GetValue(fieldValue),
                                GUILayout.Width(40));

                            if (EditorGUI.EndChangeCheck())
                            {
                                propertyOverride.SetValue(fieldValue, propertyOverrideToggle);
                            }

                            EditorGUI.BeginDisabledGroup(!propertyOverrideToggle);

                            object resultValue = null;
                            EditorGUI.BeginChangeCheck();
                            if (stageLightValueFieldInfo.FieldType == typeof(System.Single))
                            {
                                if (property.GetType().BaseType == typeof(StageLightAdditionalProperty) &&
                                    labelValue == "BPM Scale" ||
                                    property.GetType().BaseType == typeof(StageLightAdditionalProperty) &&
                                    labelValue == "BPM Offset")
                                {
                                    var stageLightAdditionalProperty = property as StageLightAdditionalProperty;
                                    EditorGUI.BeginDisabledGroup(!stageLightAdditionalProperty.bpmOverrideData.value.bpmOverride);
                                    using (new EditorGUI.IndentLevelScope())
                                    {
                                        resultValue = EditorGUILayout.FloatField(labelValue,
                                            (float)stageLightValueFieldInfo.GetValue(fieldValue));
                                    }
                                    EditorGUI.EndDisabledGroup();
                                }
                                else
                                {
                                    resultValue = EditorGUILayout.FloatField(labelValue,
                                        (float)stageLightValueFieldInfo.GetValue(fieldValue));
                                }
                            }

                            if (stageLightValueFieldInfo.FieldType == typeof(RollTransform))
                            {
                                var rollProperty = property as RollProperty;
                                var rollTransform = stageLightValueFieldInfo.GetValue(fieldValue) as RollTransform;
                                if (rollProperty.lightTransformControlType.value == AnimationMode.Ease)
                                {

                                    using (new EditorGUILayout.VerticalScope())
                                    {
                                        using (new EditorGUILayout.HorizontalScope())
                                        {
                                            var easeType = rollTransform.easeType;
                                            EditorGUI.BeginChangeCheck();
                                            var resultEaseType = EditorGUILayout.EnumPopup(labelValue, easeType);
                                            if(EditorGUI.EndChangeCheck())
                                            {
                                                rollTransform.GetType().GetField("easeType").SetValue(rollTransform,resultEaseType);
                                            }
                                        }

                                        using (new EditorGUILayout.HorizontalScope())
                                        {
                                            EditorGUILayout.BeginHorizontal();
                                            // GUILayout.FlexibleSpace();
                                            using (new EditorGUILayout.HorizontalScope())
                                            {
                                                using (new EditorCommon.LabelWidth(60))
                                                {
                                                    EditorGUI.BeginChangeCheck();
                                                    var min = EditorGUILayout.FloatField("Min",
                                                        rollTransform.rollMinMax.x);
                                                    if (EditorGUI.EndChangeCheck())
                                                    {
                                                        Undo.RecordObject(undoTarget, "Changed Area Of Effect");
                                                        rollTransform.GetType().GetField("rollMinMax")
                                                            .SetValue(rollTransform,
                                                                new Vector2(min, rollTransform.rollMinMax.y) as object);
                                                    }
                                                }
                                            }

                                            GUILayout.FlexibleSpace();
                                            using (new EditorGUILayout.HorizontalScope())
                                            {
                                                using (new EditorCommon.LabelWidth(60))
                                                {
                                                    EditorGUI.BeginChangeCheck();
                                                    var max = EditorGUILayout.FloatField("Max",
                                                        rollTransform.rollMinMax.y);
                                                    if (EditorGUI.EndChangeCheck())
                                                    {
                                                        Undo.RecordObject(undoTarget, "Changed Area Of Effect");
                                                        rollTransform.GetType().GetField("rollMinMax")
                                                            .SetValue(rollTransform,
                                                                new Vector2(rollTransform.rollMinMax.x, max) as object);
                                                    }
                                                }
                                            }

                                            EditorGUILayout.EndHorizontal();
                                        }

                                        using (new EditorGUILayout.HorizontalScope())
                                        {

                                            EditorGUILayout.FloatField(rollTransform.rollRange.x, GUILayout.Width(80));
                                            EditorGUILayout.MinMaxSlider(ref rollTransform.rollRange.x,
                                                ref rollTransform.rollRange.y,
                                                rollTransform.rollMinMax.x, rollTransform.rollMinMax.y);
                                            EditorGUILayout.FloatField(rollTransform.rollRange.y, GUILayout.Width(80));

                                        }
                                    }
                                }
                                else
                                {

                                    resultValue = EditorGUILayout.CurveField("Curve",
                                        (AnimationCurve)rollTransform.animationCurve);

                                }
                            }

                            if (stageLightValueFieldInfo.FieldType == typeof(BpmOverrideData))
                            {
                                var bpmOverrideData = stageLightValueFieldInfo.GetValue(fieldValue) as BpmOverrideData;
                                using (new EditorGUILayout.VerticalScope())
                                {
                                    using (new EditorGUILayout.HorizontalScope())
                                    {
                                        using (new EditorCommon.LabelWidth(120))
                                        {
                                            EditorGUI.BeginChangeCheck();
                                            var bpmOverrideValue = EditorGUILayout.Toggle("BPM Override",
                                                bpmOverrideData.bpmOverride,
                                                GUILayout.Width(80));
                                            if (EditorGUI.EndChangeCheck())
                                            {
                                                bpmOverrideData.GetType().GetField("bpmOverride")
                                                    .SetValue(bpmOverrideData, bpmOverrideValue);
                                            }
                                        }
                                    }

                                    using (new EditorGUI.IndentLevelScope())
                                    {
                                        using (new EditorGUILayout.HorizontalScope())
                                        {
                                            using (new EditorCommon.LabelWidth(120))
                                            {
                                                EditorGUI.BeginChangeCheck();
                                                var bpmScaleValue = EditorGUILayout.FloatField("BPM Scale",
                                                    bpmOverrideData.bpmScale,
                                                    GUILayout.Width(180));
                                                if (EditorGUI.EndChangeCheck())
                                                {
                                                    bpmOverrideData.GetType().GetField("bpmScale")
                                                        .SetValue(bpmOverrideData, bpmScaleValue);
                                                }

                                                EditorGUI.BeginChangeCheck();
                                                var bpmOffsetValue = EditorGUILayout.FloatField("BPM Offset",
                                                    bpmOverrideData.bpmOffset,
                                                    GUILayout.Width(180));
                                                if (EditorGUI.EndChangeCheck())
                                                {
                                                    bpmOverrideData.GetType().GetField("bpmOffset")
                                                        .SetValue(bpmOverrideData, bpmOffsetValue);
                                                }
                                            }
                                        }
                                    }
                                }
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
                                    labelValue, (Vector2)stageLightValueFieldInfo.GetValue(fieldValue));
                            }

                            if (stageLightValueFieldInfo.FieldType == typeof(UnityEngine.Vector3))
                            {
                                resultValue = EditorGUILayout.Vector3Field(
                                    labelValue, (Vector3)stageLightValueFieldInfo.GetValue(fieldValue));
                            }

                            if (stageLightValueFieldInfo.FieldType == typeof(UnityEngine.Vector4))
                            {
                                resultValue = EditorGUILayout.Vector4Field(
                                    labelValue, (Vector4)stageLightValueFieldInfo.GetValue(fieldValue));
                            }

                            if (stageLightValueFieldInfo.FieldType == typeof(UnityEngine.Quaternion))
                            {
                                resultValue = EditorGUILayout.Vector4Field(
                                    labelValue, (Vector4)stageLightValueFieldInfo.GetValue(fieldValue));
                            }

                            if (stageLightValueFieldInfo.FieldType == typeof(UnityEngine.AnimationCurve))
                            {
                                resultValue = EditorGUILayout.CurveField(labelValue,
                                    (AnimationCurve)stageLightValueFieldInfo.GetValue(fieldValue));

                            }

                            if (stageLightValueFieldInfo.FieldType == typeof(Texture2D))
                            {
                                resultValue = EditorGUILayout.ObjectField(labelValue,
                                    (Texture2D)stageLightValueFieldInfo.GetValue(fieldValue), typeof(Texture2D), false);
                            }

                            if (stageLightValueFieldInfo.FieldType.BaseType != null &&
                                stageLightValueFieldInfo.FieldType.BaseType == typeof(System.Enum))
                            {
                                var easeType = stageLightValueFieldInfo.GetValue(fieldValue) as Enum;
                                resultValue = EditorGUILayout.EnumPopup(labelValue, easeType);
                            }

                            if (stageLightValueFieldInfo.FieldType == typeof(UnityEngine.Gradient))
                            {
                                resultValue = EditorGUILayout.GradientField(labelValue,
                                    (Gradient)stageLightValueFieldInfo.GetValue(fieldValue));
                            }

                            if (EditorGUI.EndChangeCheck())
                            {
                                Undo.RecordObject(undoTarget, "Changed Area Of Effect");
                                if (resultValue != null) stageLightValueFieldInfo.SetValue(fieldValue, resultValue);
                            }
                            
                            EditorGUILayout.EndHorizontal();
                            EditorGUI.EndDisabledGroup();
                        }
                    }
                }
            }

            // EditorGUILayout.Space(2);
            EditorGUI.EndDisabledGroup();
            
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