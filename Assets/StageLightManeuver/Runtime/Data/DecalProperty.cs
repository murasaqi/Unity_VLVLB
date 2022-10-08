using System;
using UnityEngine;

namespace StageLightManeuver
{
    [Serializable]
    public class DecalProperty : StageLightAdditionalProperty
    {
        public StageLightToggleValue<Texture2D> decalTexture;
        public StageLightToggleValue<float> decalSizeScaler;
        public StageLightToggleValue<float> floorHeight;
        public StageLightToggleValue<float> decalDepthScaler;
        public StageLightToggleValue<float> fadeFactor;
        public StageLightToggleValue<float> opacity;
        public DecalProperty()
        {
            propertyOverride = false;
            bpmOverrideData = new StageLightToggleValue<BpmOverrideData>(){value = new BpmOverrideData()};
            propertyName = "Decal";
            decalTexture = new StageLightToggleValue<Texture2D>();
            decalSizeScaler = new StageLightToggleValue<float>(){value = 0.8f};
            floorHeight = new StageLightToggleValue<float> { value = 0f };
            decalDepthScaler = new StageLightToggleValue<float> { value = 1f };
            fadeFactor = new StageLightToggleValue<float> { value = 1f };
            opacity = new StageLightToggleValue<float> { value = 1f };
        }
        public DecalProperty(DecalProperty other)
        {
            propertyOverride = other.propertyOverride;
            bpmOverrideData = other.bpmOverrideData;
            decalTexture = new StageLightToggleValue<Texture2D>(other.decalTexture);
            propertyName = other.propertyName;
            decalSizeScaler = new StageLightToggleValue<float>(other.decalSizeScaler);
            floorHeight = new StageLightToggleValue<float>(other.floorHeight);
            decalDepthScaler = new StageLightToggleValue<float>(other.decalDepthScaler);
            fadeFactor = new StageLightToggleValue<float>(other.fadeFactor);
            opacity = new StageLightToggleValue<float>(other.opacity);
        }

    }
}