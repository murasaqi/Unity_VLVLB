using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace StageLightSupervisor
{
    [Serializable]
    public abstract class StageLightData
    {
        [SerializeField] public bool propertyOverride = false;
    }

    [Serializable]
    public class StageLightProperty<T>:StageLightData
    {
        [SerializeField]public T value;
        
    }


    [Serializable]
    public class DecalProperty : StageLightData
    {
        public StageLightProperty<float> decalSizeScaler;
        public StageLightProperty<float> floorHeight;
        public StageLightProperty<float> decalDepthScaler;
        public StageLightProperty<float> fadeFactor;
        public StageLightProperty<float> opacity;
        public DecalProperty()
        {
            decalSizeScaler = new StageLightProperty<float>(){value = 0.8f};
            floorHeight = new StageLightProperty<float> { value = 0f };
            decalDepthScaler = new StageLightProperty<float> { value = 1f };
            fadeFactor = new StageLightProperty<float> { value = 1f };
            opacity = new StageLightProperty<float> { value = 1f };
        }

        public DecalProperty(DecalProperty other)
        {
            decalSizeScaler = other.decalSizeScaler;
            floorHeight = other.floorHeight;
            decalDepthScaler = other.decalDepthScaler;
            fadeFactor = other.fadeFactor;
            opacity = other.opacity;
        }
        
    }
    [Serializable]
    public class RollProperty:StageLightData
    {
        public StageLightProperty<LightTransformControlType> lightTransformControlType;
        public StageLightProperty<float> startRoll;
        public StageLightProperty<float> endRoll;
        public StageLightProperty<EaseType> easeType;
        public StageLightProperty<AnimationCurve> animationCurve;

        public RollProperty(RollProperty rollProperty)
        {
            this.lightTransformControlType = rollProperty.lightTransformControlType;
            this.startRoll = rollProperty.startRoll;
            this.endRoll = rollProperty.endRoll;
            this.easeType = rollProperty.easeType;
            this.animationCurve = rollProperty.animationCurve;
        }

        public RollProperty()
        {
            lightTransformControlType = new StageLightProperty<LightTransformControlType>(){value =  LightTransformControlType.Ease};
            startRoll = new StageLightProperty<float>() {value = 0f};
            endRoll = new StageLightProperty<float>() {value = 0f};
            easeType = new StageLightProperty<EaseType>() {value = EaseType.Linear};
            animationCurve = new StageLightProperty<AnimationCurve>() {value = new AnimationCurve()};
        }
    }

    [Serializable]
    public class LightProperty: StageLightData
    {
        public StageLightProperty<Gradient> lightColor;// = new StageLightProperty<Gradient>(){value = new Gradient()};
        public StageLightProperty<AnimationCurve> lightIntensity;// = new StageLightProperty<float>(){value = 1f};
        public StageLightProperty<float> spotAngle;// = new StageLightProperty<float>(){value = 15f};
        public StageLightProperty<float> innerSpotAngle;// = new StageLightProperty<float>(){value = 10f};
        
        public 
            LightProperty()
        {
            lightColor = new StageLightProperty<Gradient>(){value = new Gradient()};
            lightIntensity = new StageLightProperty<AnimationCurve>(){value = new AnimationCurve()};
            spotAngle = new StageLightProperty<float>(){value = 15f};
            innerSpotAngle = new StageLightProperty<float>(){value = 10f};
        }
        
        public LightProperty(LightProperty other)
        {
            lightColor = other.lightColor;
            lightIntensity = other.lightIntensity;
            spotAngle = other.spotAngle;
            innerSpotAngle = other.innerSpotAngle;
        }
    }

    [Serializable]
    public class StageLightExtensionProperty: StageLightData
    {
        public float weight;
        public StageLightProperty<float> bpm = new StageLightProperty<float>() { value = 120 };
        public StageLightProperty<float> bpmScale = new StageLightProperty<float>(){value = 1f};
        public StageLightProperty<float> bpmOffset = new StageLightProperty<float>() { value = 0f };
        public StageLightProperty<int> index = new StageLightProperty<int>() { value = 0 };
        public StageLightProperty<LoopType> loopType = new StageLightProperty<LoopType>() { value = LoopType.Loop };
       
        public StageLightExtensionProperty()
        {
            bpm = new StageLightProperty<float>() { value = 120 };
            bpmScale = new StageLightProperty<float>() { value = 1f };
            bpmOffset = new StageLightProperty<float>() { value = 0f };
            index = new StageLightProperty<int>() { value = 0 };
            loopType = new StageLightProperty<LoopType>() { value = LoopType.Loop };
        }
            
        public StageLightExtensionProperty(StageLightExtensionProperty other)
        {
            bpm = other.bpm;
            bpmScale = other.bpmScale;
            bpmOffset = other.bpmOffset;
            index = other.index;
            loopType = other.loopType;
        }
    }
    //
    // [Serializable]
    // public class BpmProperty: StageLightData
    // {
    //     public StageLightProperty<float> bpmScale = new StageLightProperty<float>(){value = 1f};
    //     public StageLightProperty<float> bpmOffset = new StageLightProperty<float>() { value = 0f };
    //         
    //     public BpmProperty()
    //     {
    //         bpmScale = new StageLightProperty<float>(){value = 1f};
    //         bpmOffset = new StageLightProperty<float>() { value = 0f };
    //     }
    //     
    //     public BpmProperty(BpmProperty other)
    //     {
    //         bpmScale = other.bpmScale;
    //         bpmOffset = other.bpmOffset;
    //     }
    // }

    [Serializable]

    public class ClipProperty
    {
        public float clipStartTime;
        public float clipEndTime;
        
        public ClipProperty()
        {
            clipStartTime = 0f;
            clipEndTime = 0f;
        }
    }







}