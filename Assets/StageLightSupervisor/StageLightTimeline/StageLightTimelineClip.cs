using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using StageLightSupervisor;

[Serializable]
public class StageLightTimelineClip : PlayableAsset, ITimelineClipAsset
{
    public StageLightTimelineBehaviour template = new StageLightTimelineBehaviour ();

    public ClipCaps clipCaps
    {
        get { return ClipCaps.Blending; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<StageLightTimelineBehaviour>.Create (graph, template);
        StageLightTimelineBehaviour clone = playable.GetBehaviour ();
        return playable;
    }
}
