#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;
using VLB;

namespace VLVLB
{
    [CustomEditor(typeof(VLVLBTimelineClip))]
    public class VLVLBClipEditor:Editor
    {

        private VLVLBTimelineClip vlvlbTimelineClip;
        private void OnEnable()
        {
       
            vlvlbTimelineClip = target as VLVLBTimelineClip;
            
        }
        List<string> mExcluded = new List<string>();
        public override void OnInspectorGUI()
        {
            BeginInspector();
            
            int buttonWidth = 64;
            float indentOffset = EditorGUI.indentLevel * 15f;
            var lineRect = GUILayoutUtility.GetRect(1, EditorGUIUtility.singleLineHeight);
            var labelRect = new Rect(lineRect.x, lineRect.y, EditorGUIUtility.labelWidth - indentOffset, lineRect.height);
            var fieldRect = new Rect(labelRect.xMax, lineRect.y, lineRect.width - labelRect.width - buttonWidth, lineRect.height);
            var buttonNewRect = new Rect(fieldRect.xMax, lineRect.y, buttonWidth, lineRect.height);
            var buttonMargine = 4f;
            EditorGUI.BeginDisabledGroup(vlvlbTimelineClip.useProfile);
            if(GUI.Button(buttonNewRect,"Save"))
            {
                vlvlbTimelineClip.SaveProps();
            }
            buttonNewRect = new Rect(fieldRect.xMax-buttonWidth-buttonMargine, lineRect.y, buttonWidth, lineRect.height);
            if(GUI.Button(buttonNewRect,"Load"))
            {
                vlvlbTimelineClip.LoadProps();
                vlvlbTimelineClip.forceTimelineClipUpdate = true;
            }
            
            buttonNewRect = new Rect(fieldRect.xMax-buttonWidth*2f-buttonMargine*2f, lineRect.y, buttonWidth, lineRect.height);
            if(GUI.Button(buttonNewRect,"Save as"))
            {
                using (new EditorCommon.LabelWidth(120))
                {
                    
                }
                vlvlbTimelineClip.forceTimelineClipUpdate = true;
            }

            EditorGUI.EndDisabledGroup();
            
            buttonNewRect = new Rect(fieldRect.xMax-buttonWidth*3f-30, lineRect.y, buttonWidth+10, lineRect.height);
            EditorGUI.BeginChangeCheck();
            var useProfile = GUI.Toggle(buttonNewRect, vlvlbTimelineClip.useProfile,"Sync profile");
            if (EditorGUI.EndChangeCheck())
            {
                vlvlbTimelineClip.useProfile = useProfile;
                serializedObject.ApplyModifiedProperties();
                vlvlbTimelineClip.LoadProps();
                vlvlbTimelineClip.forceTimelineClipUpdate = true;
            }
            
            EditorGUI.BeginChangeCheck();
            EditorGUI.BeginDisabledGroup(vlvlbTimelineClip.useProfile);
            DrawProperties(serializedObject.FindProperty("behaviour").serializedObject, mExcluded.ToArray());
            EditorGUI.EndDisabledGroup();
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                vlvlbTimelineClip.forceTimelineClipUpdate = true;
                GUI.changed = false;
                TimelineEditor.Refresh(RefreshReason.ContentsModified);
            }


        } 
        
        void DrawProperties(SerializedObject obj, params string[] propertyToExclude)
        {
            SerializedProperty iterator = obj.GetIterator();
            bool enterChildren = true;
            while (iterator.NextVisible(enterChildren))
            {
                enterChildren = false;
                if(iterator.name == "m_Script")
                {
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.PropertyField(iterator,true);
                    EditorGUI.EndDisabledGroup();
                }
                else
                if(iterator.name == "ptlPropObject")
                {
                    EditorGUI.BeginChangeCheck();
                    EditorGUILayout.PropertyField(iterator,true);
                    if (EditorGUI.EndChangeCheck())
                    {
                        vlvlbTimelineClip.useProfile = true;
                        vlvlbTimelineClip.LoadProps();
                        vlvlbTimelineClip.forceTimelineClipUpdate = true;

                    }
                }
                else
                {
                    EditorGUILayout.PropertyField(iterator, true);     
                }
                
               
            }
        }
        
        protected void BeginInspector()
        {
            serializedObject.Update();
            mExcluded.Clear();
            GetExcludedPropertiesInInspector(mExcluded);
        }
        
        protected void GetExcludedPropertiesInInspector(List<string> excluded)
        {
            excluded.Add("m_Script");
        }

        private void OnDestroy()
        {
            DestroyComponentEditors();
        }

        void DestroyComponentEditors()
        {
            
        }
        
    }
}

#endif