using System;
using UnityEngine;

namespace StageLightSupervisor
{
    [Serializable]
    public class GoboProperty:StageLightAdditionalProperty
    {
        [DisplayName("Gobo Texture")]public StageLightValue<Texture2D> goboTexture;
        [DisplayName("Gobo Property Name")]public StageLightValue<string> goboPropertyName;
        [DisplayName("Rotation Speed")]public StageLightValue<MinMaxEasingValue> goroRotationSpeed;

        public GoboProperty()
        {
            propertyName = "Gobo";
            propertyOverride = false;
            bpmOverrideData = new StageLightValue<BpmOverrideData>(){value = new BpmOverrideData()};
            goboTexture = new StageLightValue<Texture2D>(){value = null};
            goboPropertyName = new StageLightValue<string>(){value = "_GoboTexture"};
            goroRotationSpeed = new StageLightValue<MinMaxEasingValue>(){value = new MinMaxEasingValue()};
        }
        
        public GoboProperty(GoboProperty other)
        {
            propertyName = other.propertyName;
            propertyOverride = other.propertyOverride;
            bpmOverrideData = other.bpmOverrideData;
            goboTexture = other.goboTexture;
            goboPropertyName = other.goboPropertyName;
            goroRotationSpeed = other.goroRotationSpeed;
        }
    }
}