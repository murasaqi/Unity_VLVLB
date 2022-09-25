using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace StageLightSupervisor
{
    [ExecuteAlways]
    public class LightFixture : StageLightExtension
    {
        // public LightProperty lightProperty = new LightProperty();
        public Light light;
        public Color lightColor;
        public float lightIntensity;
        public float spotAngle;
        public float innerSpotAngle;
        public UniversalAdditionalLightData universalAdditionalLightData;
        
        public override void UpdateFixture(float currentTime)
        {
            if(light == null) return;
            base.UpdateFixture(currentTime);

            lightColor = new Color(0,0,0,1);
            lightIntensity = 0f;
            spotAngle = 0f;
            innerSpotAngle = 0f;
            
            while (stageLightDataQueue.Count>0)
            {
                var data = stageLightDataQueue.Dequeue();
                var lightProperty = data.stageLightSetting.lightProperty;
                var weight = data.weight;
                if(lightProperty == null) continue;
                var t = GetNormalizedTime(currentTime,data.stageLightSetting.stageLightBaseProperty, lightProperty.LoopType);
                lightColor += lightProperty.lightColor.value.Evaluate(t) * weight;
                lightIntensity += lightProperty.lightIntensity.value.Evaluate(t) * weight;
                spotAngle += lightProperty.spotAngle.value * weight;
                innerSpotAngle += lightProperty.innerSpotAngle.value * weight;
            }
        }

        private void Update()
        {
            light.color = lightColor;
            light.intensity = lightIntensity;
            light.spotAngle = spotAngle;
            light.range = innerSpotAngle;
        }
    }
}
