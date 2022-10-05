using System;
using UnityEngine;

namespace StageLightSupervisor
{
    
    [Serializable]
    public class RollTransform
    {
        [DisplayNameAttribute("Roll")]public Vector2 rollRange = new Vector2(0, 0);
        public Vector2 rollMinMax = new Vector2(-180, 180);
        [DisplayNameAttribute("Easing")]public EaseType easeType = EaseType.Linear;
        [DisplayNameAttribute("Curve")]public AnimationCurve animationCurve = new AnimationCurve(new Keyframe[]
        {
            new Keyframe(0,0),
            new Keyframe(1,1)
        });
    }
    [Serializable]
    public class RollProperty:StageLightAdditionalProperty
    {
        [DisplayNameAttribute("Animation Mode")]public StageLightValue<AnimationMode> lightTransformControlType;
        [DisplayNameAttribute("Roll Transform")]public StageLightValue<RollTransform> rollTransform;
        // [DisplayNameAttribute("End")]public StageLightValue<float> endRoll;
        // [DisplayNameAttribute("Easing")]public StageLightValue<EaseType> easeType;
        // [DisplayNameAttribute("Curve")]public StageLightValue<AnimationCurve> animationCurve;

        public RollProperty(RollProperty rollProperty)
        {
            bpmOverrideData = rollProperty.bpmOverrideData;
            this.lightTransformControlType = rollProperty.lightTransformControlType;
            this.rollTransform = rollProperty.rollTransform;
            
            // this.animationCurve = rollProperty.animationCurve;
        }

        public RollProperty()
        {
            bpmOverrideData = new StageLightValue<BpmOverrideData>(){value = new BpmOverrideData()};
            lightTransformControlType = new StageLightValue<AnimationMode>(){value =  AnimationMode.Ease};
            rollTransform = new StageLightValue<RollTransform>() {value = new RollTransform()};
           
            // animationCurve = new StageLightValue<AnimationCurve>() {value = new AnimationCurve()};
        }

    }
    
    [Serializable]
    public class PanProperty:RollProperty
    {
        public PanProperty(PanProperty panProperty):base(panProperty)
        {
            propertyName = "Pan";
        }

        public PanProperty():base()
        {
            propertyName = "Pan";
        }
    }
    [Serializable]
    public class TiltProperty:RollProperty
    {
        public TiltProperty(TiltProperty tiltProperty):base(tiltProperty)
        {
            
            propertyName = "Tilt4";
        }

        public TiltProperty():base()
        {
            
            propertyName = "Tilt";
        }
    }

}