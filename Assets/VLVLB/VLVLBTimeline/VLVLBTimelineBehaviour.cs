using System;
using System.Collections.Generic;
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
    public class PanTilt
    {
        public float pan = 0;
        public float tilt = 0;
        
        public PanTilt(float pan, float tilt)
        {
            this.pan = pan;
            this.tilt = tilt;
        }
    }
    [Serializable]
    public class VLVLBTimelineBehaviour : PlayableBehaviour
    {
        // public LoopType loopType = LoopType.PingPong;
        // [SerializeField]public float BPM = 120;
        // public float offsetChildTime = 0f;
        // public float offsetUniverseTime = 0;
        //
        // public bool ignoreOffsetColor = false;
        // public float timeScaleColor = 1;
        // [GradientUsage(true)] public Gradient lightColor = new Gradient();
        // public bool  ignoreOffsetPan = false;
        // public float timeScalePan = 1;
        //
        // [NormalizedAnimationCurveAttribute(false,true)]public AnimationCurve pan = new AnimationCurve(new Keyframe[] {new Keyframe(0, -30), new Keyframe(1, 30)});
        // public bool ignoreOffsetTilt = false;
        // public float timeScaleTilt = 1;
        // [NormalizedAnimationCurveAttribute(false,true)]public AnimationCurve tilt = new AnimationCurve(new Keyframe[] {new Keyframe(0, 40), new Keyframe(1, -40)});
        // public bool ignoreOffsetIntensity = false;
        // public float timeScaleIntensity = 1;
        // [NormalizedAnimationCurveAttribute(false,true)]public AnimationCurve intensity = new AnimationCurve(new Keyframe[] {new Keyframe(0, 0), new Keyframe(1, 1)});
        // // public float rootEmissionPower = 1.2f;
        // public bool ignoreOffsetAngle = false;
        // public float timeScaleAngle = 1;
        //
        // public float spotAngle = 14f;
        // public float rangeLimit = 1.5f;
        // public float truncatedRadius = 0.03f;
        // public Vector2 decalSize = new Vector2(1, 1);
        // public float decalDepth = 20;
        // public float decalOpacity = 1f;
        //
        // public bool useManualTransform = false;
        // public List<PanTilt> manualTransforms = new List<PanTilt>();


        public PTLProps props = new PTLProps();




        // public bool saveProperties;
        // public bool useProfile = false;
        [HideInInspector]public VLVLBClipProfile vlvlbClipProfile = null;
        // public PTLProps ptlProps;
        public override void OnPlayableCreate(Playable playable)
        {

        }
        
        public void LoadFromProfile()
        {
            props =new PTLProps(vlvlbClipProfile.ptlProps);

        }
        
        public void SaveToProfile()
        {
            if (vlvlbClipProfile == null) return;
            this.vlvlbClipProfile.ptlProps = new PTLProps(this.props);
        }
        
       
        public VLVLBClipProfile ExportToProfile()
        {
            var exportPtlProps = new VLVLBClipProfile(this.props);
            return exportPtlProps;

        }
    }

    public abstract class PtlPropsElement
    {
       
        [HideInInspector]public double inputPlayableTime;
        [HideInInspector]public float weight;
        public float BPM;
        [HideInInspector]public float fixedTime;
        public LoopType loopType;
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
        [GradientUsage(true)]public Gradient lightColor;
        public bool ignoreOffsetIntensity;
        public float timeScaleIntensity;
        public AnimationCurve intensity;
        public bool ignoreOffsetAngle;
        public float timeScaleAngle;
        public float spotAngle;
        public float rangeLimit;
        public float truncatedRadius;
        public Vector2 decalSize;
        public float decalDepth;
        public float decalOpacity;
        public bool useManualTransform;
        public List<PanTilt> manualTransforms;
    }

    [Serializable]
    public class PTLProps : PtlPropsElement
    {
        public List<PanTilt> CloneManualTransforms (List<PanTilt> transforms)
        {
            var clone = new List<PanTilt>();
            foreach (var transform in transforms)
            {
                clone.Add(new PanTilt(transform.pan, transform.tilt));
            }
            return clone;
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
        public PTLProps(PTLProps props = null)
        {
            if (props != null)
            {
               
                timeScaleAngle = props.timeScaleAngle;
                timeScaleColor = props.timeScaleColor;
                timeScaleIntensity = props.timeScaleIntensity;
                ignoreOffsetPan = props.ignoreOffsetPan;
                timeScalePan = props.timeScalePan;
                timeScaleTilt =    props.timeScaleTilt;
                pan = CloneAnimationCurve( props.pan);
                tilt = CloneAnimationCurve(props.tilt);
                ignoreOffsetTilt = props.ignoreOffsetTilt;
                ignoreOffsetAngle = props.ignoreOffsetAngle;
                ignoreOffsetColor = props.ignoreOffsetColor;
                ignoreOffsetIntensity = props.ignoreOffsetIntensity;
                inputPlayableTime = props.inputPlayableTime;
                weight = props.weight;
                BPM = props.BPM;
                offsetChildTime = props.offsetChildTime;
                offsetUniverseTime = props.offsetUniverseTime;
                intensity = CloneAnimationCurve(props.intensity);
                spotAngle = props.spotAngle;
                rangeLimit = props.rangeLimit;
                truncatedRadius = props.truncatedRadius;
                decalSize = props.decalSize;
                decalDepth = props.decalDepth;
                decalOpacity = props.decalOpacity;
                fixedTime = props.fixedTime;
                lightColor = CloneGradient(props.lightColor);
                loopType =  props.loopType;
                useManualTransform = props.useManualTransform;
                manualTransforms = CloneManualTransforms(props.manualTransforms);
            }else
            {
                timeScaleAngle = 1;
                timeScaleColor = 1;
                timeScaleIntensity = 1;
                timeScalePan = 1;
                timeScaleTilt = 1;
                ignoreOffsetTilt = false;
                ignoreOffsetAngle = false;
                ignoreOffsetColor = false;
                ignoreOffsetIntensity = false;
                ignoreOffsetPan = false;
                inputPlayableTime = 0;
                weight = 1;
                BPM = 120;
                offsetChildTime = 0;
                offsetUniverseTime = 0;
                pan = new AnimationCurve();
                tilt = new AnimationCurve();
                intensity = new AnimationCurve();
                spotAngle = 0;
                rangeLimit = 0;
                truncatedRadius = 0;
                decalSize = new Vector2(0,0);
                decalDepth = 0;
                decalOpacity = 1;
                fixedTime = 0;
                lightColor = new Gradient();
                loopType = LoopType.Loop;
                useManualTransform = false;
                manualTransforms = new List<PanTilt>();
            }
            
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