using System;
using UnityEngine;

namespace StageLightManeuver
{
    
    [Serializable]
    public class MinMaxEasingValue
    {
        [DisplayName("Mode")] public AnimationMode mode = AnimationMode.Ease;
        [DisplayName("Range")]public Vector2 rollRange = new Vector2(0, 0);
        public Vector2 rollMinMax = new Vector2(-180, 180);
        [DisplayName("Easing")]public EaseType easeType = EaseType.Linear;
        [DisplayName("Curve")]public AnimationCurve animationCurve = new AnimationCurve(new Keyframe[]
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
        [DisplayName("Roll Transform")]public StageLightToggleValue<MinMaxEasingValue> rollTransform;
       
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
            bpmOverrideData = new StageLightToggleValue<BpmOverrideData>(){value = new BpmOverrideData()};
            rollTransform = new StageLightToggleValue<MinMaxEasingValue>() {value = new MinMaxEasingValue()};
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