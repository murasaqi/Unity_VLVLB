using UnityEngine;

namespace VLVLB
{

    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PTLPropsObject", order = 1)]
    public class VLVLBClipProfile : ScriptableObject
    {
        public PTLProps ptlProps = new PTLProps()
        {
            ignoreOffsetPan = false,
            timeScalePan = 1,
            pan = new AnimationCurve(new Keyframe[] {new Keyframe(0, 180), new Keyframe(1, -180)}),
            ignoreOffsetTilt = false,
            timeScaleTilt = 1,
            tilt = new AnimationCurve(new Keyframe[] {new Keyframe(0, 180), new Keyframe(1, -180)}),
            ignoreOffsetIntensity = false,
            timeScaleIntensity = 1,
            intensity = new AnimationCurve(new Keyframe[] {new Keyframe(0, 0), new Keyframe(1, 1)}),
            ignoreOffsetAngle = false,
            timeScaleAngle = 1,
            spotAngle = 14f,
            rangeLimit = 1.5f,
            truncatedRadius = 0.03f,
            ignoreOffsetColor = false,
            timeScaleColor = 1,
            fixedTime  = 0f,
            loopType = LoopType.PingPong

        };
    }

}