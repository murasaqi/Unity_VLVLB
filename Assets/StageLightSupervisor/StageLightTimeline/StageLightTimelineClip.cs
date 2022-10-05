using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using StageLightSupervisor;
using UnityEditor;


[Serializable]
public class StageLightTimelineClip : PlayableAsset, ITimelineClipAsset
{
    
    public StageLightProfile referenceStageLightProfile;
    [HideInInspector] public StageLightProfile stageLightProfile;
    [HideInInspector]public StageLightTimelineBehaviour template = new StageLightTimelineBehaviour ();
    public ClipCaps clipCaps
    {
        get { return ClipCaps.Blending; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<StageLightTimelineBehaviour>.Create (graph, template);
        StageLightTimelineBehaviour clone = playable.GetBehaviour ();
        if (stageLightProfile == null)
        {
            stageLightProfile = ScriptableObject.CreateInstance<StageLightProfile>();
            stageLightProfile.Init();
        }
        // ApplySetting();
        return playable;
    }
    
    
    [ContextMenu("Apply")]
    public void LoadProfile()
    {
        if(referenceStageLightProfile == null) return;
        stageLightProfile = referenceStageLightProfile.Clone();
    }

    public void SaveProfile()
    {
#if UNITY_EDITOR
        Undo.RegisterCompleteObjectUndo(referenceStageLightProfile, referenceStageLightProfile.name);
        referenceStageLightProfile = stageLightProfile.Clone(); 
        // Set dirty flag
        EditorUtility.SetDirty(referenceStageLightProfile);
        AssetDatabase.SaveAssets();
#endif
    }
}
