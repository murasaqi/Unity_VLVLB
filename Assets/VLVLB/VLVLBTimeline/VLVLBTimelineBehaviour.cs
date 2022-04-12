using System;
using UnityEngine;
using UnityEngine.Playables;


namespace VLVLB
{
    [Serializable]

    public enum LoopType
    {
        Loop,
        PingPong,
        Fixed
    }

    [Serializable]
    public class VLVLBTimelineBehaviour : PlayableBehaviour
    {
        [SerializeField]public float BPM = 120;
        public float offsetChildTime = 0f;
         public float offsetUniverseTime = 0;
        public bool  ignoreOffsetPan = false;
        public float timeScalePan = 1;
        
        [NormalizedAnimationCurveAttribute(false,true)]public AnimationCurve pan = new AnimationCurve(new Keyframe[] {new Keyframe(0, -30), new Keyframe(1, 30)});
        public bool ignoreOffsetTilt = false;
        public float timeScaleTilt = 1;
        [NormalizedAnimationCurveAttribute(false,true)]public AnimationCurve tilt = new AnimationCurve(new Keyframe[] {new Keyframe(0, 40), new Keyframe(1, -40)});
        public bool ignoreOffsetIntensity = false;
        public float timeScaleIntensity = 1;
        [NormalizedAnimationCurveAttribute(false,true)]public AnimationCurve intensity = new AnimationCurve(new Keyframe[] {new Keyframe(0, 0), new Keyframe(1, 1)});
        public float rootEmissionPower = 1.2f;
        public bool ignoreOffsetAngle = false;
        public float timeScaleAngle = 1;

        public float spotAngle = 14f;
        public float rangeLimit = 1.5f;
        public float truncatedRadius = 0.03f;
        public Vector2 decalSize = new Vector2(1, 1);
        public float decalDepth = 20;
        public float decalOpacity = 1f;
        
        public bool ignoreOffsetColor = false;
        public float timeScaleColor = 1;

        [GradientUsage(true)] public Gradient color = new Gradient();
        // public Color colorEnd;

        public LoopType loopType = LoopType.PingPong;

        
        // public bool saveProperties;
        // public bool useProfile = false;
        [HideInInspector]public VLVLBClipProfile vlvlbClipProfile = null;
        // public PTLProps ptlProps;
        public override void OnPlayableCreate(Playable playable)
        {

        }
        
        public void LoadFromProfile()
        {
            // Debug.Log($"{BPM},{ptlPropsObject.ptlProps.BPM}");
            if (vlvlbClipProfile == null) return;
            offsetChildTime =  vlvlbClipProfile.ptlProps.offsetChildTime;
            BPM =  vlvlbClipProfile.ptlProps.BPM;
            // Debug.Log($"{BPM},{ptlPropsObject.ptlProps.BPM}");
            offsetChildTime =  vlvlbClipProfile.ptlProps.offsetChildTime;
            offsetUniverseTime =  vlvlbClipProfile.ptlProps.offsetUniverseTime;
            ignoreOffsetPan =  vlvlbClipProfile.ptlProps.ignoreOffsetPan;
            timeScalePan =  vlvlbClipProfile.ptlProps.timeScalePan;
            ignoreOffsetTilt =  vlvlbClipProfile.ptlProps.ignoreOffsetTilt;
            timeScaleTilt =  vlvlbClipProfile.ptlProps.timeScaleTilt;
            ignoreOffsetAngle =  vlvlbClipProfile.ptlProps.ignoreOffsetAngle;
            timeScaleAngle =  vlvlbClipProfile.ptlProps.timeScaleAngle;
            ignoreOffsetIntensity =  vlvlbClipProfile.ptlProps.ignoreOffsetIntensity;
            timeScaleIntensity =  vlvlbClipProfile.ptlProps.timeScaleIntensity;
            rootEmissionPower = vlvlbClipProfile.ptlProps.rootEmissionPower;
            ignoreOffsetColor =  vlvlbClipProfile.ptlProps.ignoreOffsetColor;
            timeScaleColor =  vlvlbClipProfile.ptlProps.timeScaleColor;
            pan=  CloneAnimationCurve(vlvlbClipProfile.ptlProps.pan);
            tilt =  CloneAnimationCurve(vlvlbClipProfile.ptlProps.tilt);
            intensity =  CloneAnimationCurve(vlvlbClipProfile.ptlProps.intensity);

            color = CloneGradient(vlvlbClipProfile.ptlProps.color);
            spotAngle =  vlvlbClipProfile.ptlProps.spotAngle;
            rangeLimit =  vlvlbClipProfile.ptlProps.rangeLimit;
            truncatedRadius =  vlvlbClipProfile.ptlProps.truncatedRadius;
            decalSize = vlvlbClipProfile.ptlProps.decalSize;
            decalDepth = vlvlbClipProfile.ptlProps.decalDepth;
            decalOpacity = vlvlbClipProfile.ptlProps.decalOpacity;

        }

        private AnimationCurve CloneAnimationCurve(AnimationCurve curve)
        {
            var clone= new AnimationCurve();
            clone.keys = curve.keys;
            return clone;
        }

        private Gradient CloneGradient(Gradient gradient)
        {
            var clone = new Gradient();
            clone.alphaKeys = gradient.alphaKeys;
            clone.colorKeys = gradient.colorKeys;
            clone.mode = gradient.mode;
            return clone;
        }
        public void SaveToProfile()
        {
           
            if (vlvlbClipProfile == null) return;
            vlvlbClipProfile.ptlProps.offsetChildTime = offsetChildTime;
            vlvlbClipProfile.ptlProps.BPM = BPM;
            vlvlbClipProfile.ptlProps.offsetUniverseTime = offsetUniverseTime;
            vlvlbClipProfile.ptlProps.ignoreOffsetPan = ignoreOffsetPan;
            vlvlbClipProfile.ptlProps.timeScalePan = timeScalePan;
            vlvlbClipProfile.ptlProps.ignoreOffsetTilt = ignoreOffsetTilt;
            vlvlbClipProfile.ptlProps.timeScaleTilt = timeScaleTilt;
            vlvlbClipProfile.ptlProps.ignoreOffsetAngle = ignoreOffsetAngle;
            vlvlbClipProfile.ptlProps.timeScaleAngle = timeScaleAngle; 
            vlvlbClipProfile.ptlProps.ignoreOffsetIntensity = ignoreOffsetIntensity;
            vlvlbClipProfile.ptlProps.timeScaleIntensity = timeScaleIntensity;
            vlvlbClipProfile.ptlProps.rootEmissionPower = rootEmissionPower;
            vlvlbClipProfile.ptlProps.ignoreOffsetColor = ignoreOffsetColor;
            vlvlbClipProfile.ptlProps.timeScaleColor = timeScaleColor;
            vlvlbClipProfile.ptlProps.pan = CloneAnimationCurve(pan);
            vlvlbClipProfile.ptlProps.tilt = CloneAnimationCurve(tilt);
            vlvlbClipProfile.ptlProps.intensity = CloneAnimationCurve(intensity);
            vlvlbClipProfile.ptlProps.color = CloneGradient(color);
            vlvlbClipProfile.ptlProps.spotAngle = spotAngle;
            vlvlbClipProfile.ptlProps.rangeLimit = rangeLimit;
            vlvlbClipProfile.ptlProps.truncatedRadius = truncatedRadius;
            vlvlbClipProfile.ptlProps.decalSize = decalSize;
            vlvlbClipProfile.ptlProps.decalDepth = decalDepth;
            vlvlbClipProfile.ptlProps.decalOpacity = decalOpacity;

        }
        
        public VLVLBClipProfile ExportToProfile()
        {
            var exportPtlProps = new VLVLBClipProfile();
            // if (ptlPropsObject == null) return;
            exportPtlProps.ptlProps.offsetChildTime = offsetChildTime;
            exportPtlProps.ptlProps.BPM = BPM;
            exportPtlProps.ptlProps.offsetUniverseTime = offsetUniverseTime;
            exportPtlProps.ptlProps.ignoreOffsetPan = ignoreOffsetPan;
            exportPtlProps.ptlProps.timeScalePan = timeScalePan;
            exportPtlProps.ptlProps.ignoreOffsetTilt = ignoreOffsetTilt;
            exportPtlProps.ptlProps.timeScaleTilt = timeScaleTilt;
            exportPtlProps.ptlProps.ignoreOffsetAngle = ignoreOffsetAngle;
            exportPtlProps.ptlProps.timeScaleAngle = timeScaleAngle; 
            exportPtlProps.ptlProps.ignoreOffsetIntensity = ignoreOffsetIntensity;
            exportPtlProps.ptlProps.timeScaleIntensity = timeScaleIntensity;
            exportPtlProps.ptlProps.rootEmissionPower = rootEmissionPower;
            exportPtlProps.ptlProps.ignoreOffsetColor = ignoreOffsetColor;
            exportPtlProps.ptlProps.timeScaleColor = timeScaleColor;
            exportPtlProps.ptlProps.pan = CloneAnimationCurve(pan);
            exportPtlProps.ptlProps.tilt = CloneAnimationCurve(tilt);
            exportPtlProps.ptlProps.intensity = CloneAnimationCurve(intensity);
            exportPtlProps.ptlProps.color = CloneGradient(color);
            exportPtlProps.ptlProps.spotAngle = spotAngle;
            exportPtlProps.ptlProps.rangeLimit = rangeLimit;
            exportPtlProps.ptlProps.truncatedRadius = truncatedRadius;
            exportPtlProps.ptlProps.decalSize = decalSize;
            exportPtlProps.ptlProps.decalDepth = decalDepth;
            exportPtlProps.ptlProps.decalOpacity = decalOpacity;

            return exportPtlProps;

        }
    }

    public abstract class PtlPropsElement
    {
       
        public double inputPlayableTime;
        public float weight;
        public float BPM;
        public float fixedTime;
        public float offsetUniverseTime;
        public float offsetChildTime;
        public float timeScalePan;
        public bool ignoreOffsetPan;
        public AnimationCurve pan;
        public float timeScaleTilt;
        public bool ignoreOffsetTilt;
        public AnimationCurve tilt;
        public bool ignoreOffsetColor;
        public float timeScaleColor;
        public Gradient color;
        public bool ignoreOffsetIntensity;
        public float timeScaleIntensity;
        public AnimationCurve intensity;
        public float rootEmissionPower;
        public bool ignoreOffsetAngle;
        public float timeScaleAngle;
        public float spotAngle;
        public float rangeLimit;
        public float truncatedRadius;
        public Vector2 decalSize;
        public float decalDepth;
        public float decalOpacity;
        public LoopType loopType;
    }

    [Serializable]
    public class PTLProps : PtlPropsElement
    {


        public PTLProps()
        {

            timeScaleAngle = 0f;
            timeScaleColor = 0f;
            timeScaleIntensity = 0f;
            timeScalePan = 0f;
            timeScaleTilt = 0f;
            ignoreOffsetTilt = false;
            ignoreOffsetAngle = false;
            ignoreOffsetColor = false;
            ignoreOffsetIntensity = false;
            ignoreOffsetPan = false;
            inputPlayableTime = 0;
            weight = 0f;
            BPM = 0;
            offsetChildTime = 0;
            offsetUniverseTime = 0;
            pan = new AnimationCurve();
            tilt = new AnimationCurve();
            intensity = new AnimationCurve();
            rootEmissionPower = 0f;
            spotAngle = 0f;
            rangeLimit = 0f;
            truncatedRadius = 0f;
            decalSize = new Vector2(1, 1);
            decalDepth = 0;
            decalOpacity = 0f;
            fixedTime = 0f;
            color = new Gradient();
            loopType = LoopType.PingPong;
            
        }

        

        public float GetNormalizedTime(int universe, int index, float timeScale)
        {
            if (loopType == LoopType.Fixed)
            {
                return fixedTime;
            }
            else
            {
                var bpm = BPM * timeScale;
                var offsetTime = inputPlayableTime + (offsetChildTime * index)+(universe* offsetUniverseTime);
                var duration = 60 / bpm;
                var t = (float) offsetTime % duration;
                var inv = Mathf.CeilToInt((float) offsetTime / duration) % 2 != 0;
                var normalisedTime = t / duration;
                return inv ? 1f - normalisedTime : normalisedTime;
            }
           
        }
    }
}