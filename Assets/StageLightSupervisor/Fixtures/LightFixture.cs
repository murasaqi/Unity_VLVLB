using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace StageLightSupervisor
{
    public class LightFixture : StageLightExtension
    {
        public LightProperty lightProperty = new LightProperty();
        public Light light;
        public UniversalAdditionalLightData universalAdditionalLightData;
        
        public override void Update(float currentTime)
        {
            if(light == null) return;
            base.Update(currentTime);
            var t = GetNormalizedTime(currentTime);
            light.color = lightProperty.lightColor.value.Evaluate(t);
            light.intensity = lightProperty.lightIntensity.value.Evaluate(t);
            light.spotAngle = lightProperty.spotAngle.value;
            light.range = lightProperty.innerSpotAngle.value;
            
        }
    }
}
