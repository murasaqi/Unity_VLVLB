using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace VLVLB
{

    [TrackColor(1f, 0.6798111f, 0f)]
    [TrackClipType(typeof(VLVLBTimelineClip))]
    [TrackBindingType(typeof(PanTiltLightGroup))]
    public class VLVLBTimelineTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            var playableBehaviour = ScriptPlayable<VLVLBTimelineMixerBehaviour>.Create(graph, inputCount);
            playableBehaviour.GetBehaviour().clips = GetClips().ToList();
            return playableBehaviour;
        }
    }
}