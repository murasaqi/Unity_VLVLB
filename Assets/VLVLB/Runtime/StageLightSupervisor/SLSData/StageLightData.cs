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
    public class DecalSizeProperty : StageLightData
    {
        public StageLightProperty<Vector2> decalSize;
        public StageLightProperty<float> decalScalarSize;
        
        public DecalSizeProperty()
        {
            decalSize = new StageLightProperty<Vector2>(){value = new Vector2(1,1)};
            decalScalarSize = new StageLightProperty<float> { value = 1f };
        }
        public DecalSizeProperty( Vector2 size, float scalarSize)
        {
            decalSize = new StageLightProperty<Vector2>(){value = size};
            decalScalarSize = new StageLightProperty<float> { value = scalarSize };
        }
        
        public DecalSizeProperty(DecalSizeProperty other)
        {
            decalSize = other.decalSize;
            decalScalarSize = other.decalScalarSize;
        }
        
    }
    [Serializable]
    public class RollProperty:StageLightData
    {
        public StageLightProperty<bool> manualMode;
        public StageLightProperty<float> startRoll;
        public StageLightProperty<float> endRoll;
        public StageLightProperty<EaseType> easeType;
        public StageLightProperty<AnimationCurve> animationCurve;

        public RollProperty(RollProperty rollProperty)
        {
            this.manualMode = rollProperty.manualMode;
            this.startRoll = rollProperty.startRoll;
            this.endRoll = rollProperty.endRoll;
            this.easeType = rollProperty.easeType;
            this.animationCurve = rollProperty.animationCurve;
        }

        public RollProperty()
        {
            manualMode = new StageLightProperty<bool>(){value =  false};
            startRoll = new StageLightProperty<float>() {value = 0f};
            endRoll = new StageLightProperty<float>() {value = 0f};
            easeType = new StageLightProperty<EaseType>() {value = EaseType.Linear};
            animationCurve = new StageLightProperty<AnimationCurve>() {value = new AnimationCurve()};
        }
    }

 
    
    
   
    
    
}