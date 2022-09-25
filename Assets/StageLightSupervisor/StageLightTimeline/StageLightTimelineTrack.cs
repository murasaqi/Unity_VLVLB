using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using StageLightSupervisor;

[TrackColor(0.8239978f, 0.9150943f, 0.3338079f)]
[TrackClipType(typeof(StageLightTimelineClip))]
[TrackBindingType(typeof(StageLight))]
public class StageLightTimelineTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        var mixer = ScriptPlayable<StageLightTimelineMixerBehaviour>.Create (graph, inputCount);
        var stageLightTimelineMixer = mixer.GetBehaviour();
        stageLightTimelineMixer.clips = GetClips().ToList();
        return mixer;
    }
}
