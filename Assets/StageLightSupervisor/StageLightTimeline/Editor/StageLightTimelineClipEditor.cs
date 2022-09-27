using System.Collections.Generic;
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
            var stageLightSetting = new SerializedObject(serializedObject.FindProperty("stageLightProfile").objectReferenceValue);
            // var stageLightData =
            //     new SerializedObject(stageLightSetting.FindProperty("stageLightData").objectReferenceValue);
            var iterator = stageLightSetting.GetIterator();
            // var iterator = serializedObject.GetIterator();
            bool enterChildren = true;
            while (iterator.NextVisible(enterChildren))
            {
                enterChildren = false;
                EditorGUILayout.PropertyField(iterator, true);
            }
            // DrawPropertyInInspector(stageLightSetting.FindProperty("stageLightBaseProperty"));
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