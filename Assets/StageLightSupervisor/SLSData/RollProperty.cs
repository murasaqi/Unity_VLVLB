using System;
using UnityEngine;

namespace StageLightSupervisor
{
    
    [Serializable]
    public class MinMaxEasingValue
    {
        [DisplayNameAttribute("Mode")] public AnimationMode mode = AnimationMode.Ease;
        [DisplayNameAttribute("Range")]public Vector2 rollRange = new Vector2(0, 0);
        public Vector2 rollMinMax = new Vector2(-180, 180);
        [DisplayNameAttribute("Easing")]public EaseType easeType = EaseType.Linear;
        [DisplayNameAttribute("Curve")]public AnimationCurve animationCurve = new AnimationCurve(new Keyframe[]
        {
            new Keyframe(0,0),
            new Keyframe(1,1)
        });


        public MinMaxEasingValue()
        {
            mode = AnimationMode.Ease;
            rollRange = new Vector2(0, 0);
            rollMinMax = new Vector2(-180, 180);
            easeType = EaseType.Linear;
            animationCurve = new AnimationCurve(new Keyframe[]
            {
                new Keyframe(0,0),
                new Keyframe(1,1)
            });
        }
    }
    [Serializable]
    public class RollProperty:StageLightAdditionalProperty
    {
        [DisplayNameAttribute("Roll Transform")]public StageLightValue<MinMaxEasingValue> rollTransform;
       
        public RollProperty(RollProperty rollProperty)
        {
            bpmOverrideData = rollProperty.bpmOverrideData;
            this.rollTransform = rollProperty.rollTransform;
            propertyOverride = rollProperty.propertyOverride;

            // this.animationCurve = rollProperty.animationCurve;
        }

        public RollProperty()
        {
            propertyOverride = false;
            bpmOverrideData = new StageLightValue<BpmOverrideData>(){value = new BpmOverrideData()};
            rollTransform = new StageLightValue<MinMaxEasingValue>() {value = new MinMaxEasingValue()};
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
            
            propertyName = "Tilt";
        }

        public TiltProperty():base()
        {
            
            propertyName = "Tilt";
        }
    }

}