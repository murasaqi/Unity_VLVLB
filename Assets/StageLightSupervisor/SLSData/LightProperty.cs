using System;
using UnityEngine;

namespace StageLightSupervisor
{
    [Serializable]
    public class LightProperty: StageLightProperty
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
}