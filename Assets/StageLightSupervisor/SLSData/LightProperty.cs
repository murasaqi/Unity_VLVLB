using System;
using UnityEngine;

namespace StageLightSupervisor
{
    [Serializable]
    public class LightProperty: StageLightProperty
    {
        public StageLightValue<Gradient> lightColor;// = new StageLightProperty<Gradient>(){value = new Gradient()};
        public StageLightValue<AnimationCurve> lightIntensity;// = new StageLightProperty<float>(){value = 1f};
        public StageLightValue<float> spotAngle;// = new StageLightProperty<float>(){value = 15f};
        public StageLightValue<float> innerSpotAngle;// = new StageLightProperty<float>(){value = 10f};
        
        public 
            LightProperty()
        {
            lightColor = new StageLightValue<Gradient>(){value = new Gradient()};
            lightIntensity = new StageLightValue<AnimationCurve>(){value = new AnimationCurve()};
            spotAngle = new StageLightValue<float>(){value = 15f};
            innerSpotAngle = new StageLightValue<float>(){value = 10f};
        }
        
        public LightProperty(LightProperty other)
        {
            lightColor = other.lightColor;
            lightIntensity = other.lightIntensity;
            spotAngle = other.spotAngle;
            innerSpotAngle = other.innerSpotAngle;
        }
    }
}