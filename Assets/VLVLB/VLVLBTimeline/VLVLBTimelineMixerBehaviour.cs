using System;
using System.Collections.Generic;
using UnityEngine;
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


            var cue = new List<PTLProps>();
            for (int i = 0; i < inputCount; i++)
            {
                var clip = clips[i].asset as VLVLBTimelineClip;
                float inputWeight = playable.GetInputWeight(i);
                ScriptPlayable<VLVLBTimelineBehaviour> inputPlayable =
                    (ScriptPlayable<VLVLBTimelineBehaviour>) playable.GetInput(i);
                VLVLBTimelineBehaviour input = inputPlayable.GetBehaviour();
                float normalisedTime = (float)(inputPlayable.GetTime() / inputPlayable.GetDuration ());
                var time = playable.GetTime();


                if (inputWeight > 0)
                {

                    var props = new PTLProps();
                    var referenceProp = input.props;
                    props.BPM = referenceProp.BPM;
                    props.offsetChildTime = referenceProp.offsetChildTime;
                    props.offsetUniverseTime = referenceProp.offsetUniverseTime;
                    // props.normalisedTime = normalisedTime;
                    props.ignoreOffsetPan = referenceProp.ignoreOffsetPan;
                    props.timeScalePan = referenceProp.timeScalePan;
                    props.ignoreOffsetTilt = referenceProp.ignoreOffsetTilt;
                    props.timeScaleTilt = referenceProp.timeScaleTilt;
                    props.ignoreOffsetAngle = referenceProp.ignoreOffsetAngle;
                    props.timeScaleAngle = referenceProp.timeScaleAngle;
                    props.ignoreOffsetIntensity = referenceProp.ignoreOffsetIntensity;
                    props.timeScaleIntensity = referenceProp.timeScaleIntensity;
                    props.ignoreOffsetColor = referenceProp.ignoreOffsetColor;
                    props.timeScaleColor = referenceProp.timeScaleColor;
                    props.pan = referenceProp.pan;
                    props.tilt = referenceProp.tilt;
                    props.intensity = referenceProp.intensity;
                    props.lightColor = referenceProp.lightColor;
                    props.spotAngle = referenceProp.spotAngle;
                    props.rangeLimit = referenceProp.rangeLimit;
                    props.truncatedRadius = referenceProp.truncatedRadius;
                    props.decalSize = referenceProp.decalSize;
                    props.decalDepth = referenceProp.decalDepth;
                    props.decalOpacity = referenceProp.decalOpacity;
                    props.fixedTime = normalisedTime;
                    props.loopType = referenceProp.loopType;
                    props.inputPlayableTime = Math.Max(time + clip.offsetClipTime, 0);
                    props.weight = inputWeight;
                    props.useManualTransform = referenceProp.useManualTransform;

                    var elementCount = 0;
                    foreach (var ptl in trackBinding.ptls)
                    {
                        ptl.elementIndex = elementCount;
                        elementCount++;
                    }

                    foreach (var universe in trackBinding.ptlUniverses)
                    {
                        foreach (var ptl in universe.ptls)
                        {
                            ptl.elementIndex = elementCount;
                            elementCount++;
                        }
                    }

                    var ptlCount = elementCount;
                    
                    Debug.Log($"{referenceProp.manualTransforms.Count}, {ptlCount}");
                    if (referenceProp.manualTransforms == null)
                    {
                        referenceProp.manualTransforms = new List<PanTilt>( ptlCount);
                    }
                    else if( referenceProp.manualTransforms.Count != ptlCount)
                    {
                        if (referenceProp.manualTransforms.Count > ptlCount)
                        {
                           
                            referenceProp.manualTransforms.RemoveRange( ptlCount-1, referenceProp.manualTransforms.Count - ptlCount);
                            
                        }

                        if (referenceProp.manualTransforms.Count < ptlCount)
                        {
                            var diff = ptlCount - referenceProp.manualTransforms.Count;
                            for(int j = 0; j < diff; j++)
                            {
                                referenceProp.manualTransforms.Add(new PanTilt(0,0));
                            }
                        }
                    }
                    props.manualTransforms = referenceProp.manualTransforms;
                    
                    cue.Add(props);
                }

            }

            trackBinding.UpdatePTL(cue);
        }
    }
}