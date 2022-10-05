using System;
using UnityEngine;

namespace StageLightSupervisor
{
    [Serializable]
    public class DecalProperty : StageLightAdditionalProperty
    {
        public StageLightValue<Texture2D> decalTexture;
        public StageLightValue<float> decalSizeScaler;
        public StageLightValue<float> floorHeight;
        public StageLightValue<float> decalDepthScaler;
        public StageLightValue<float> fadeFactor;
        public StageLightValue<float> opacity;
        public DecalProperty()
        {
            propertyOverride = false;
            bpmOverrideData = new StageLightValue<BpmOverrideData>(){value = new BpmOverrideData()};
            propertyName = "Decal";
            decalTexture = new StageLightValue<Texture2D>();
            decalSizeScaler = new StageLightValue<float>(){value = 0.8f};
            floorHeight = new StageLightValue<float> { value = 0f };
            decalDepthScaler = new StageLightValue<float> { value = 1f };
            fadeFactor = new StageLightValue<float> { value = 1f };
            opacity = new StageLightValue<float> { value = 1f };
        }
        public DecalProperty(DecalProperty other)
        {
            propertyOverride = other.propertyOverride;
            bpmOverrideData = other.bpmOverrideData;
            decalTexture = other.decalTexture;
            propertyName = other.propertyName;
            decalSizeScaler = other.decalSizeScaler;
            floorHeight = other.floorHeight;
            decalDepthScaler = other.decalDepthScaler;
            fadeFactor = other.fadeFactor;
            opacity = other.opacity;
        }

    }
}