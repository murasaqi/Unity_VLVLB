using System;
using UnityEngine;

namespace StageLightManeuver
{
    [Serializable]
    public class GoboProperty:StageLightAdditionalProperty
    {
        [DisplayName("Gobo Texture")]public StageLightToggleValue<Texture2D> goboTexture;
        [DisplayName("Gobo Property Name")]public StageLightToggleValue<string> goboPropertyName;
        [DisplayName("Rotation Speed")]public StageLightToggleValue<MinMaxEasingValue> goroRotationSpeed;

        public GoboProperty()
        {
            propertyName = "Gobo";
            propertyOverride = false;
            bpmOverrideData = new StageLightToggleValue<BpmOverrideData>(){value = new BpmOverrideData()};
            goboTexture = new StageLightToggleValue<Texture2D>(){value = null};
            goboPropertyName = new StageLightToggleValue<string>(){value = "_GoboTexture"};
            goroRotationSpeed = new StageLightToggleValue<MinMaxEasingValue>(){value = new MinMaxEasingValue()};
        }
        
        public GoboProperty(GoboProperty other)
        {
            propertyName = other.propertyName;
            propertyOverride = other.propertyOverride;
            bpmOverrideData = new StageLightToggleValue<BpmOverrideData>(other.bpmOverrideData);
            goboTexture = new StageLightToggleValue<Texture2D>(other.goboTexture);
            goboPropertyName = new StageLightToggleValue<string>(other.goboPropertyName);
            goroRotationSpeed = new StageLightToggleValue<MinMaxEasingValue>(other.goroRotationSpeed);
        }
    }
}