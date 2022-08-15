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
        [Header("Clip UI Options", order = 0)] 
        [SerializeField][Range(0,1f)]public float colorLineHeight = 0.1f;
        [SerializeField]public bool drawBeat =false;
        [SerializeField] public Color beatLineColor = new Color(0, 1, 0.7126422f, 0.3f);
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            var playableBehaviour = ScriptPlayable<VLVLBTimelineMixerBehaviour>.Create(graph, inputCount);
            var behaviour = playableBehaviour.GetBehaviour();
            behaviour.clips = GetClips().ToList();

            foreach (var c in behaviour.clips)
            {
                var vlvlbTimelineClip =  c.asset as VLVLBTimelineClip;
                vlvlbTimelineClip.mixer = behaviour;
                vlvlbTimelineClip.track = this;
            }
            
            return playableBehaviour;
        }
    }
}