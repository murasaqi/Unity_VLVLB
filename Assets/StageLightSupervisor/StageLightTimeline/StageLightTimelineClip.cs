using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using StageLightSupervisor;


[Serializable]
public class StageLightTimelineClip : PlayableAsset, ITimelineClipAsset
{
    
    public StageLightProfile referenceStageLightProfile;
    [HideInInspector]public StageLightProfile stageLightProfile = null;
    [HideInInspector]public StageLightTimelineBehaviour template = new StageLightTimelineBehaviour ();
    public ClipCaps clipCaps
    {
        get { return ClipCaps.Blending; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<StageLightTimelineBehaviour>.Create (graph, template);
        StageLightTimelineBehaviour clone = playable.GetBehaviour ();
        if(stageLightProfile == null)stageLightProfile = ScriptableObject.CreateInstance<StageLightProfile>();
        // ApplySetting();
        return playable;
    }
    
    
    [ContextMenu("Apply")]
    public void ApplySetting()
    {
        if(referenceStageLightProfile == null) return;
        stageLightProfile = Instantiate(referenceStageLightProfile);
    }
}
