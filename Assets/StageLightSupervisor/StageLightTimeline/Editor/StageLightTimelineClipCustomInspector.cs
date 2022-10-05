using System;
using System.Collections.Generic;
using System.IO;
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
using VLVLB;
using Object = UnityEngine.Object;

namespace StageLightSupervisor.StageLightTimeline.Editor
{
    [CustomEditor(typeof(StageLightTimelineClip))]
    public class StageLightTimelineClipCustomInspector : UnityEditor.Editor
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

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                GUI.backgroundColor= Color.red;
                GUI.contentColor = Color.white;
                if (GUILayout.Button("Save as",GUILayout.MaxWidth(100)))
                {
                    ExportProfile(stageLightTimelineClip);
                }
                
                
                GUI.backgroundColor= Color.green;
                GUI.contentColor = Color.white;
                if (GUILayout.Button("Load Profile",GUILayout.MaxWidth(100)))
                {
                    stageLightTimelineClip.LoadProfile();
                }
                
                
                GUI.backgroundColor= Color.white;
                GUI.contentColor = Color.white;
                if (GUILayout.Button("Save Profile",GUILayout.MaxWidth(100)))
                {
                    stageLightTimelineClip.SaveProfile();
                    
                    serializedObject.ApplyModifiedProperties();
                }

                
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

                            if (stageLightValueFieldInfo.FieldType == typeof(MinMaxEasingValue))
                            {

                                DrawMinMaxEaseUI(labelValue,stageLightValueFieldInfo, fieldValue, undoTarget);
                            }

                            if (stageLightValueFieldInfo.FieldType == typeof(BpmOverrideData))
                            {
                                var bpmOverrideData = stageLightValueFieldInfo.GetValue(fieldValue) as BpmOverrideData;
                                // Debug.Log(bpmOverrideData);
                                
                                using (new EditorGUILayout.VerticalScope())
                                {
                                   
                                    using (new EditorGUILayout.HorizontalScope())
                                    {   
                                        
                                        using (new EditorCommon.LabelWidth(120))
                                        {
                                            EditorGUI.BeginChangeCheck();
                                            var bpmOverride = EditorGUILayout.Toggle("Override Time",
                                                bpmOverrideData.bpmOverride);
                                            if (EditorGUI.EndChangeCheck())
                                            {
                                                bpmOverrideData.GetType().GetField("bpmOverride")
                                                    .SetValue(bpmOverrideData, bpmOverride);
                                            }
                                        }

                                        
                                    }

                                    using (new EditorGUI.IndentLevelScope())
                                    {
                                        using (new EditorGUILayout.HorizontalScope())
                                        {
                                            // using (new EditorCommon.LabelWidth(100))
                                            // {
                                                EditorGUI.BeginChangeCheck();
                                                var resultLoopType =
                                                    EditorGUILayout.EnumPopup("Loop Type", bpmOverrideData.loopType);
                                                if (EditorGUI.EndChangeCheck())
                                                {
                                                    bpmOverrideData.GetType().GetField("loopType")
                                                        .SetValue(bpmOverrideData, resultLoopType);
                                                }
                                            // }
                                        }
                                        
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
                                if (resultValue != null)
                                {
                                    stageLightValueFieldInfo.SetValue(fieldValue, resultValue);
                                    // var stageLightTimelineClip = serializedObject.targetObject as StageLightTimelineClip;
                                    // stageLightTimelineClip.stageLightProfile.Serialize();
                                }
                                
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

        protected void DrawMinMaxEaseUI(string labelName ,FieldInfo fieldInfo,object target, Object undoTarget=null)
        {
            
            var minMaxEasingValue = fieldInfo.GetValue(target) as MinMaxEasingValue;
            using (new EditorGUILayout.VerticalScope())
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField(labelName);
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUI.BeginChangeCheck();
                    // using (new EditorCommon.LabelWidth(80))
                    // {
                        var animationMode = EditorGUILayout.EnumPopup("Mode", minMaxEasingValue.mode);
                        if (EditorGUI.EndChangeCheck())
                        {
                            minMaxEasingValue.GetType().GetField("mode")
                                .SetValue(minMaxEasingValue, animationMode);
                        }
                    // }   
                }

                if (minMaxEasingValue.mode == AnimationMode.Ease)
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        var easeType = minMaxEasingValue.easeType;
                        EditorGUI.BeginChangeCheck();
                        var resultEaseType = EditorGUILayout.EnumPopup("Ease Type", easeType);
                        if(EditorGUI.EndChangeCheck())
                        {
                            minMaxEasingValue.GetType().GetField("easeType").SetValue(minMaxEasingValue,resultEaseType);
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
                                    minMaxEasingValue.rollMinMax.x);
                                if (EditorGUI.EndChangeCheck())
                                {
                                    if(undoTarget != null)Undo.RecordObject(undoTarget, "Changed Area Of Effect");
                                    minMaxEasingValue.GetType().GetField("rollMinMax")
                                        .SetValue(minMaxEasingValue,
                                            new Vector2(min, minMaxEasingValue.rollMinMax.y) as object);
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
                                    minMaxEasingValue.rollMinMax.y);
                                if (EditorGUI.EndChangeCheck())
                                {
                                    Undo.RecordObject(undoTarget, "Changed Area Of Effect");
                                    minMaxEasingValue.GetType().GetField("rollMinMax")
                                        .SetValue(minMaxEasingValue,
                                            new Vector2(minMaxEasingValue.rollMinMax.x, max) as object);
                                }
                            }
                        }

                        EditorGUILayout.EndHorizontal();
                    }

                    using (new EditorGUILayout.HorizontalScope())
                    {

                        EditorGUILayout.FloatField(minMaxEasingValue.rollRange.x, GUILayout.Width(80));
                        EditorGUILayout.MinMaxSlider(ref minMaxEasingValue.rollRange.x,
                            ref minMaxEasingValue.rollRange.y,
                            minMaxEasingValue.rollMinMax.x, minMaxEasingValue.rollMinMax.y);
                        EditorGUILayout.FloatField(minMaxEasingValue.rollRange.y, GUILayout.Width(80));

                    }
                }else
                {
                    EditorGUI.BeginChangeCheck();
                    var curveResult = EditorGUILayout.CurveField("Curve",
                        (AnimationCurve)minMaxEasingValue.animationCurve);
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(undoTarget, "Changed Area Of Effect");
                        minMaxEasingValue.GetType().GetField("animationCurve")
                            .SetValue(minMaxEasingValue,
                                curveResult);
                    }
                }
            }
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


        private void ExportProfile(StageLightTimelineClip stageLightTimelineClip)
        {
           
            Undo.RegisterCompleteObjectUndo(stageLightTimelineClip.stageLightProfile, stageLightTimelineClip.stageLightProfile.name);
            EditorUtility.SetDirty(stageLightTimelineClip.stageLightProfile);
            
            var exportPath = stageLightTimelineClip.referenceStageLightProfile != null ? AssetDatabase.GetAssetPath(stageLightTimelineClip.referenceStageLightProfile) : "Asset";
            var exportName = stageLightTimelineClip.referenceStageLightProfile != null ? stageLightTimelineClip.referenceStageLightProfile.name+"(Clone)" : "new stageLightProfile";
            var path = EditorUtility.SaveFilePanel("Save StageLightProfile Asset", exportPath,exportName, "asset");
            string fileName = Path.GetFileName(path);
            if(path == "") return;
            path = path.Replace("\\", "/").Replace(Application.dataPath, "Assets");
            string dir = Path.GetDirectoryName(path);
            // Debug.Log($"dir: {dir}, file: {fileName}");
            var newProfile = stageLightTimelineClip.stageLightProfile.Clone();
            AssetDatabase.CreateAsset(newProfile, path);
            // useProfile = true;
            // ptlPropObject = new ExposedReference<VLVLBClipProfile>();
            stageLightTimelineClip.referenceStageLightProfile = AssetDatabase.LoadAssetAtPath<StageLightProfile>(path);
            // Debug.Log($"Load {path}");
        }
        private void OnDisable()
        {
           
        }

        private void OnDestroy()
        {
        }


      

    }
}