using System;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace VLVLB
{

    public class VLVLBTimelineMixerBehaviour : PlayableBehaviour
    {
        private List<TimelineClip> m_Clips;

        internal List<TimelineClip> clips
        {
            get { return m_Clips; }
            set { m_Clips = value; }
        }

        // NOTE: This function is called at runtime and edit time.  Keep that in mind when setting the values of properties.
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            PanTiltLightGroup trackBinding = playerData as PanTiltLightGroup;

            if (!trackBinding)
                return;

            int inputCount = playable.GetInputCount();



            // float pan = 0f;
            // float tilt = 0f;
            // float intensity = 0f;
            // Color color =Color.black;
            // float spotAngle = 0f;
            // float rangeLimit = 0f;
            // float startDistance = 0f;
            var cue = new List<PTLProps>();
            for (int i = 0; i < inputCount; i++)
            {
                var clip = clips[i].asset as VLVLBTimelineClip;
                float inputWeight = playable.GetInputWeight(i);
                ScriptPlayable<VLVLBTimelineBehaviour> inputPlayable =
                    (ScriptPlayable<VLVLBTimelineBehaviour>) playable.GetInput(i);
                VLVLBTimelineBehaviour input = inputPlayable.GetBehaviour();
                float normalisedTime = (float)(inputPlayable.GetTime() / inputPlayable.GetDuration ());

                // var duration = 60 / input.BPM;
                // var t = (float)inputPlayable.GetTime()%duration;
                // var inv = Mathf.CeilToInt((float) inputPlayable.GetTime() / duration) %2 != 0;
                // var normalisedTime = t / duration;
                // normalisedTime = inv ? 1f - normalisedTime : normalisedTime;

                var time = inputPlayable.GetTime();


                if (inputWeight > 0)
                {

                    var props = new PTLProps();
                    props.offsetChildTime = input.offsetChildTime;
                    props.BPM = input.BPM;
                    props.offsetChildTime = input.offsetChildTime;
                    props.offsetUniverseTime = input.offsetUniverseTime;
                    // props.normalisedTime = normalisedTime;
                    props.ignoreOffsetPan = input.ignoreOffsetPan;
                    props.timeScalePan = input.timeScalePan;
                    props.ignoreOffsetTilt = input.ignoreOffsetTilt;
                    props.timeScaleTilt = input.timeScaleTilt;
                    props.ignoreOffsetAngle = input.ignoreOffsetAngle;
                    props.timeScaleAngle = input.timeScaleAngle;
                    props.ignoreOffsetIntensity = input.ignoreOffsetIntensity;
                    props.timeScaleIntensity = input.timeScaleIntensity;
                    props.ignoreOffsetColor = input.ignoreOffsetColor;
                    props.timeScaleColor = input.timeScaleColor;
                    props.pan = input.pan;
                    props.tilt = input.tilt;
                    props.intensity = input.intensity;
                    props.color = input.color;
                    props.spotAngle = input.spotAngle;
                    props.rangeLimit = input.rangeLimit;
                    props.truncatedRadius = input.truncatedRadius;
                    props.decalSize = input.decalSize;
                    props.decalDepth = input.decalDepth;
                    props.decalOpacity = input.decalOpacity;
                    props.fixedTime = normalisedTime;
                    props.loopType = input.loopType;
                    props.rootEmissionPower = input.rootEmissionPower;


                    
                    // Debug.Log(input.ptlPropsObject);
                    // if (input.ptlPropsObject != null && input.saveProperties)
                    // {
                    //
                    //     clips[i].displayName = input.ptlPropsObject.name + "(Auto Save)";
                    //     // input.saveProperties = false;
                    //     input.ptlPropsObject.ptlProps = props;
                    //    
                    // }
                    //
                    // if (input.ptlPropsObject != null && input.useProfile)
                    // {
                    //     clips[i].displayName = input.ptlPropsObject.name + "(Synch)";
                    //     props = ScriptableObject.Instantiate(input.ptlPropsObject).ptlProps;
                    // }

                    // var clip = clips[i].asset as PTLTimelineClip;
                    props.inputPlayableTime = Math.Max(time + clip.offsetClipTime, 0);
                    props.weight = inputWeight;
                    cue.Add(props);
                }
                // if(inputWeight > 0)Debug.Log(t);

            }

            // Debug.Log($"angle{spotAngle}, limit:{rangeLimit}, distance:{startDistance}");
            trackBinding.UpdatePTL(cue);
        }
    }
}