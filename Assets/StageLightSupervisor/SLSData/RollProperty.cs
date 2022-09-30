using System;
using UnityEngine;

namespace StageLightSupervisor
{
    [Serializable]
    public class RollProperty:StageLightProperty
    {
        [DisplayNameAttribute("Animation Mode")]public StageLightValue<AnimationMode> lightTransformControlType;
        [DisplayNameAttribute("Start")]public StageLightValue<float> startRoll;
        [DisplayNameAttribute("End")]public StageLightValue<float> endRoll;
        [DisplayNameAttribute("Easing")]public StageLightValue<EaseType> easeType;
        [DisplayNameAttribute("Curve")]public StageLightValue<AnimationCurve> animationCurve;

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
            lightTransformControlType = new StageLightValue<AnimationMode>(){value =  AnimationMode.Ease};
            startRoll = new StageLightValue<float>() {value = 0f};
            endRoll = new StageLightValue<float>() {value = 0f};
            easeType = new StageLightValue<EaseType>() {value = EaseType.Linear};
            animationCurve = new StageLightValue<AnimationCurve>() {value = new AnimationCurve()};
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