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

        [HideInInspector][SerializeField]public bool useProfile = false;
        [SerializeField]public float offsetClipTime;
        [SerializeField]public ExposedReference<VLVLBClipProfile> ptlPropObject;
        [SerializeField]public VLVLBTimelineBehaviour behaviour = new VLVLBTimelineBehaviour();
        [HideInInspector]public VLVLBClipProfile resolvedVlvlbClipProfile = null;
        [HideInInspector] public VLVLBTimelineMixerBehaviour mixer;
        [HideInInspector] public VLVLBTimelineTrack track;
        private PlayableGraph playableGraph;
        [HideInInspector] public bool forceTimelineClipUpdate = false;
        public ClipCaps clipCaps
        {
            get { return ClipCaps.Blending; }
        }

        public void SaveProps()
        {
            if (behaviour != null)
            {
               
                
               
                behaviour.SaveToProfile();
                Debug.Log($"Save to {behaviour.vlvlbClipProfile.name}");
                
            }
            
            
        }
        
        public void LoadProps()
        {

            // Debug.Log(clone);
            if (behaviour != null )
            {
                // resolvedVlvlbClipProfile = ptlPropObject.Resolve(playableGraph.GetResolver());
                behaviour.vlvlbClipProfile = resolvedVlvlbClipProfile;
                behaviour.LoadFromProfile();
                forceTimelineClipUpdate = true;
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
            if (behaviour == null) return;
            Undo.RegisterCompleteObjectUndo(behaviour.vlvlbClipProfile, behaviour.vlvlbClipProfile.name);
            EditorUtility.SetDirty(behaviour.vlvlbClipProfile);
            var exportPath = ptlPropObject.defaultValue != null ? AssetDatabase.GetAssetPath(ptlPropObject.defaultValue) : "Asset";
            var exportName = ptlPropObject.defaultValue != null ? ptlPropObject.defaultValue.name+"(Clone)" : "vlvlbSettings";
            var path = EditorUtility.SaveFilePanel("Save VLVLB Asset", exportPath,exportName, "asset");
            string fileName = Path.GetFileName(path);
            if(path == "") return;
            path = path.Replace("\\", "/").Replace(Application.dataPath, "Assets");
            string dir = Path.GetDirectoryName(path);
            Debug.Log($"dir: {dir}, file: {fileName}");
            var newProfile = ScriptableObject.CreateInstance<VLVLBClipProfile>();
            newProfile.ptlProps = new PTLProps(behaviour.props);
            AssetDatabase.CreateAsset(newProfile, path);
            useProfile = true;
            ptlPropObject = new ExposedReference<VLVLBClipProfile>();
            ptlPropObject.defaultValue = AssetDatabase.LoadAssetAtPath<VLVLBClipProfile>(path);
            Debug.Log($"Load {path}");
            behaviour.props = new PTLProps(newProfile.ptlProps);
           // LoadProps();
#endif
        }

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            playableGraph = graph;
            var playable = ScriptPlayable<VLVLBTimelineBehaviour>.Create(graph, behaviour);
            behaviour = playable.GetBehaviour();
            if (ptlPropObject.defaultValue == null)
            {
                behaviour.vlvlbClipProfile = null;
            }
            resolvedVlvlbClipProfile = ptlPropObject.Resolve(graph.GetResolver());
            behaviour.vlvlbClipProfile = resolvedVlvlbClipProfile;
            
            
            if (useProfile && resolvedVlvlbClipProfile != null)
            {
                LoadProps();
            }
            return playable;
        }


       
    }
}