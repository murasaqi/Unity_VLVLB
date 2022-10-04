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
                var lightProperty = data.stageLightProfile.lightProperty;
                var weight = data.weight;
                if(lightProperty == null) continue;
                var stageLightBaseProperty = data.stageLightProfile.stageLightBaseProperty;
                var bpm = stageLightBaseProperty.bpm.value;
                var bpmOffset = lightProperty.bpmOverrideData.value.bpmOverride
                    ? lightProperty.bpmOverrideData.value.bpmOffset
                    : stageLightBaseProperty.bpmOffset.value;
                var bpmScale = lightProperty.bpmOverrideData.value.bpmOverride
                    ? lightProperty.bpmOverrideData.value.bpmScale
                    : stageLightBaseProperty.bpmScale.value;
                var t = GetNormalizedTime(currentTime,bpm,bpmOffset,bpmScale, lightProperty.LoopType);
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
