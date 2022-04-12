using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace VLVLB
{

    [Serializable]
    public class VLVLBTimelineClip : PlayableAsset, ITimelineClipAsset
    {

        public ExposedReference<VLVLBClipProfile> ptlPropObject;
        public float offsetClipTime;
        public bool useProfile = true;
        public VLVLBTimelineBehaviour template = new VLVLBTimelineBehaviour();
        public VLVLBClipProfile resolvedVlvlbClipProfile = null;
       
        public ClipCaps clipCaps
        {
            get { return ClipCaps.Blending; }
        }

        public void SaveProps()
        {
            if (template != null)
            {
                template.SaveToProfile();
                Debug.Log($"Save to {template.vlvlbClipProfile.name}");
            }
            
            
        }
        
        public void LoadProps()
        {

            // Debug.Log(clone);
            if (template != null)
            {
                template.vlvlbClipProfile = resolvedVlvlbClipProfile;
                template.LoadFromProfile();
                // if(resolvedPtlPropsObject != null)Debug.Log($"{name}: Load {template.ptlPropsObject.name}");
            }
        }

        // private void OnValidate()
        // {
        //     if (useProfile && ptlPropObject.defaultValue != null)
        //     {
        //         LoadProps();
        //     }
        // }

        public void ExportProfile()
        {
            #if UNITY_EDITOR
            if (template == null) return;
            
                var path = EditorUtility.SaveFilePanelInProject("Save VLVLB Asset", "vlvlbSettings", "asset", "Please enter a file name to save the texture to");
            // if (string.IsNullOrEmpty(path))
            //     return;
            string fileName = Path.GetFileName(path);
            string dir = Path.GetDirectoryName(path);
            Debug.Log($"dir: {dir}, file: {fileName}");
            // var setting = 
           AssetDatabase.CreateAsset(template.ExportToProfile(), path);
           ptlPropObject = new ExposedReference<VLVLBClipProfile>();
           ptlPropObject.defaultValue = AssetDatabase.LoadAssetAtPath<VLVLBClipProfile>(path);
           Debug.Log($"Load {path}");
           LoadProps();
           // ptlPropObject = path
#endif
        }

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<VLVLBTimelineBehaviour>.Create(graph, template);
            template = playable.GetBehaviour();
            if (ptlPropObject.defaultValue == null)
            {
                template.vlvlbClipProfile = null;
            }
            resolvedVlvlbClipProfile = ptlPropObject.Resolve(graph.GetResolver());
            template.vlvlbClipProfile = resolvedVlvlbClipProfile;
            
            
            if (useProfile && resolvedVlvlbClipProfile != null)
            {
                LoadProps();
            }
            // clone.ptlPropsObject = ptlPropObject;
            // if (clone.useProfile && ptlPropObject != null) name = ptlPropObject.name + "(Synch)";
            return playable;
        }


       
    }
}