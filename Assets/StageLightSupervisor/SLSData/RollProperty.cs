using System;
using UnityEngine;

namespace StageLightSupervisor
{
    [Serializable]
    public class RollProperty:StageLightProperty
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
    public class PanProperty:RollProperty
    {
        public PanProperty(PanProperty panProperty):base(panProperty)
        {
            
        }

        public PanProperty():base()
        {
            
        }
    }
    [Serializable]
    public class TiltProperty:RollProperty
    {
        public TiltProperty(TiltProperty tiltProperty):base(tiltProperty)
        {
            
        }

        public TiltProperty():base()
        {
            
        }
    }

}