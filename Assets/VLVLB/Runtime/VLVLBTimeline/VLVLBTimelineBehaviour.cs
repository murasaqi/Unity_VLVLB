using System;
using System.Collections.Generic;
using UnityEditor;
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

        [SerializeField]public PTLProps props = new PTLProps();




        // public bool saveProperties;
        // public bool useProfile = false;
        [HideInInspector]public VLVLBClipProfile vlvlbClipProfile = null;
        // public PTLProps ptlProps;
        public override void OnPlayableCreate(Playable playable)
        {

        }
        
        public void LoadFromProfile()
        {
            if (vlvlbClipProfile == null) return;
            props =new PTLProps(vlvlbClipProfile.ptlProps);

        }
        
        public void SaveToProfile()
        {
#if UNITY_EDITOR
            Undo.RegisterCompleteObjectUndo(vlvlbClipProfile, vlvlbClipProfile.name);
            // EditorUtility.SetDirty(vlvlbClipProfile);
            if (vlvlbClipProfile == null) return;
            this.vlvlbClipProfile.ptlProps = new PTLProps(this.props);
            EditorUtility.SetDirty(vlvlbClipProfile);
            AssetDatabase.SaveAssets();
            
#endif
        }
        
        // public VLVLBClipProfile ExportToProfile(VLVLBClipProfile vlvlbClipProfile)
        // {
        //     var exportPtlProps = new VLVLBClipProfile(vlvlbClipProfile.ptlProps);
        //     return exportPtlProps;
        //
        // }
    }

    
    [Serializable]
    public abstract class PtlPropsElement
    {
       
        [HideInInspector]public double inputPlayableTime;
        [HideInInspector]public float weight;
        [Header("--- BPM ---")]        
        [SerializeField] public float BPM;
        [SerializeField][HideInInspector]public float fixedTime;
        [Header("--- Loop Type ---")]        
        [SerializeField]public LoopType loopType;
        [Header("--- Offset time ---")]        
        [SerializeField]public float offsetUniverseTime;
        [SerializeField]public float offsetChildTime;
        [Header("--- Pan ---")]        
        [SerializeField]public float timeScalePan;
        [SerializeField]public bool ignoreOffsetPan;
        [SerializeField, NormalizedAnimationCurve(false,true)]public AnimationCurve pan;
        [Header("--- Tilt ---")]
        [SerializeField]public float timeScaleTilt;
        [SerializeField]public bool ignoreOffsetTilt;
        [SerializeField, NormalizedAnimationCurve(false,true)]public AnimationCurve tilt;
        [Header("--- Color ---")]
        [SerializeField]public bool ignoreOffsetColor;
        [SerializeField]public float timeScaleColor;
        [GradientUsage(true)]public Gradient lightColor;
        [Header("--- Intensity ---")]
        [SerializeField]public bool ignoreOffsetIntensity;
        [SerializeField]public float timeScaleIntensity;
        [SerializeField, NormalizedAnimationCurve(false,true)]public AnimationCurve intensity;
        [Header("--- Angle ---")]
        [SerializeField]public bool ignoreOffsetAngle;
        [SerializeField]public float timeScaleAngle;
        [SerializeField]public float innerSpotAngle;
        [SerializeField]public float spotAngle;
        [SerializeField]public float rangeLimit;
        [SerializeField]public float truncatedRadius;
        [SerializeField, HideInInspector] public float sideThickness;
        [Header("--- Decal ---")]
        [SerializeField]public Vector2 decalSize;
        [SerializeField]public float decalDepth;
        [SerializeField]public float decalOpacity;
        [Header("--- Manual Transform ---")]
        [SerializeField]public bool useManualTransform;
        [SerializeField]public List<PanTilt> manualTransforms;
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
                sideThickness = props.sideThickness;
                innerSpotAngle  = props.innerSpotAngle;
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
                intensity = new AnimationCurve(){keys = new Keyframe[] {new Keyframe(0, 1), new Keyframe(1, 1)}};
                spotAngle = 38;
                sideThickness = 0.2f;
                rangeLimit = 10;
                truncatedRadius = 0;
                decalSize = new Vector2(0.5f,0.5f);
                decalDepth = 10;
                decalOpacity = 1;
                fixedTime = 0;
                lightColor = new Gradient(){alphaKeys = new GradientAlphaKey[1]{new GradientAlphaKey(1,0)},colorKeys = new GradientColorKey[1]{new GradientColorKey(Color.white,0)}};
                loopType = LoopType.Loop;
                useManualTransform = false;
                manualTransforms = new List<PanTilt>();
                innerSpotAngle =21.8f;
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
                if (loopType == LoopType.Loop) return normalisedTime;
                return inv ? 1f - normalisedTime : normalisedTime;
            }
           
        }
    }
}