using System;
using UnityEngine;

namespace StageLightSupervisor
{
    
    [Serializable]
    public class RollTransform
    {
        [DisplayNameAttribute("Roll")]public Vector2 rollRange = new Vector2(0, 0);
        [DisplayNameAttribute("Easing")]public EaseType easeType = EaseType.Linear;
    }
    [Serializable]
    public class RollProperty:StageLightProperty
    {
        [DisplayNameAttribute("Animation Mode")]public StageLightValue<AnimationMode> lightTransformControlType;
        [DisplayNameAttribute("RollValue")]public StageLightValue<RollTransform> rollTransform;
        // [DisplayNameAttribute("End")]public StageLightValue<float> endRoll;
        // [DisplayNameAttribute("Easing")]public StageLightValue<EaseType> easeType;
        [DisplayNameAttribute("Curve")]public StageLightValue<AnimationCurve> animationCurve;

        public RollProperty(RollProperty rollProperty)
        {
            this.lightTransformControlType = rollProperty.lightTransformControlType;
            this.rollTransform = rollProperty.rollTransform;
            this.animationCurve = rollProperty.animationCurve;
        }

        public RollProperty()
        {
            lightTransformControlType = new StageLightValue<AnimationMode>(){value =  AnimationMode.Ease};
            rollTransform = new StageLightValue<RollTransform>() {value = new RollTransform()};
           
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