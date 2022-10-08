using System;
using UnityEngine;

namespace StageLightManeuver
{
    [Serializable]
    public class LightProperty: StageLightAdditionalProperty
    {
        public StageLightToggleValue<Gradient> lightToggleColor;// = new StageLightProperty<Gradient>(){value = new Gradient()};
        public StageLightToggleValue<AnimationCurve> lightToggleIntensity;// = new StageLightProperty<float>(){value = 1f};
        public StageLightToggleValue<float> spotAngle;// = new StageLightProperty<float>(){value = 15f};
        public StageLightToggleValue<float> innerSpotAngle;// = new StageLightProperty<float>(){value = 10f};
        
        public LightProperty()
        {
            propertyName = "Light";
            propertyOverride = false;
            bpmOverrideData = new StageLightToggleValue<BpmOverrideData>(){value = new BpmOverrideData()};
            lightToggleColor = new StageLightToggleValue<Gradient>(){value = new Gradient()};
            lightToggleIntensity = new StageLightToggleValue<AnimationCurve>(){value = new AnimationCurve(new []{new Keyframe(0,0),new Keyframe(1,1)})};
            spotAngle = new StageLightToggleValue<float>(){value = 15f};
            innerSpotAngle = new StageLightToggleValue<float>(){value = 10f};
        }
        
        public LightProperty(LightProperty other)
        {
            propertyName = other.propertyName;
            propertyOverride = other.propertyOverride;
            bpmOverrideData = new StageLightToggleValue<BpmOverrideData>(other.bpmOverrideData);
            lightToggleColor = new StageLightToggleValue<Gradient>(other.lightToggleColor);
            lightToggleIntensity = new StageLightToggleValue<AnimationCurve>(other.lightToggleIntensity);
            spotAngle = new StageLightToggleValue<float>(other.spotAngle);
            innerSpotAngle = new StageLightToggleValue<float>(other.innerSpotAngle);
        }
    }
}