using System;
using UnityEngine;

namespace StageLightSupervisor
{
    [Serializable]
    public class DecalProperty : StageLightProperty
    {
        public StageLightProperty<float> decalSizeScaler;
        public StageLightProperty<float> floorHeight;
        public StageLightProperty<float> decalDepthScaler;
        public StageLightProperty<float> fadeFactor;
        public StageLightProperty<float> opacity;
        public DecalProperty()
        {
            decalSizeScaler = new StageLightProperty<float>(){value = 0.8f};
            floorHeight = new StageLightProperty<float> { value = 0f };
            decalDepthScaler = new StageLightProperty<float> { value = 1f };
            fadeFactor = new StageLightProperty<float> { value = 1f };
            opacity = new StageLightProperty<float> { value = 1f };
        }

        public DecalProperty(DecalProperty other)
        {
            decalSizeScaler = other.decalSizeScaler;
            floorHeight = other.floorHeight;
            decalDepthScaler = other.decalDepthScaler;
            fadeFactor = other.fadeFactor;
            opacity = other.opacity;
        }

        // public DecalProperty Mix(DecalProperty a, DecalProperty b, float weight)
        // {
        //     DecalProperty result = new DecalProperty();
        //     result.decalSizeScaler.value = Mathf.Lerp(a.decalSizeScaler.value, b.decalSizeScaler.value, weight);
        //     result.floorHeight = StageLightProperty<float>.Mix(a.floorHeight, b.floorHeight, weight);
        //     result.decalDepthScaler = StageLightProperty<float>.Mix(a.decalDepthScaler, b.decalDepthScaler, weight);
        //     result.fadeFactor = StageLightProperty<float>.Mix(a.fadeFactor, b.fadeFactor, weight);
        //     result.opacity = StageLightProperty<float>.Mix(a.opacity, b.opacity, weight);
        //     return result;
        // }
        
        // public static DecalProperty operator +(DecalProperty a, DecalProperty b)
        // {
        //     DecalProperty result = new DecalProperty();
        //     result.decalSizeScaler.value = a.decalSizeScaler.value + b.decalSizeScaler.value;
        //     result.floorHeight.value = a.floorHeight.value + b.floorHeight.value;
        //     result.decalDepthScaler.value = a.decalDepthScaler.value + b.decalDepthScaler.value;
        //     result.fadeFactor.value = a.fadeFactor.value + b.fadeFactor.value;
        //     result.opacity.value = a.opacity.value + b.opacity.value;
        //     return result;
        // }
        //         
        // public static DecalProperty operator *(DecalProperty a, float b)
        // {
        //     DecalProperty result = new DecalProperty();
        //     result.decalSizeScaler.value = a.decalSizeScaler.value * b;
        //     result.floorHeight.value = a.floorHeight.value * b;
        //     result.decalDepthScaler.value = a.decalDepthScaler.value * b;
        //     result.fadeFactor.value = a.fadeFactor.value * b;
        //     result.opacity.value = a.opacity.value * b;
        //     return result;
        // }
        //
        // public static DecalProperty operator *(float b, DecalProperty a)
        // {
        //     DecalProperty result = new DecalProperty();
        //     result.decalSizeScaler.value = a.decalSizeScaler.value * b;
        //     result.floorHeight.value = a.floorHeight.value * b;
        //     result.decalDepthScaler.value = a.decalDepthScaler.value * b;
        //     result.fadeFactor.value = a.fadeFactor.value * b;
        //     result.opacity.value = a.opacity.value * b;
        //     return result;
        // }
        //
        // public static DecalProperty operator /(DecalProperty a, float b)
        // {
        //     DecalProperty result = new DecalProperty();
        //     result.decalSizeScaler.value = a.decalSizeScaler.value / b;
        //     result.floorHeight.value = a.floorHeight.value / b;
        //     result.decalDepthScaler.value = a.decalDepthScaler.value / b;
        //     result.fadeFactor.value = a.fadeFactor.value / b;
        //     result.opacity.value = a.opacity.value / b;
        //     return result;
        // }
        //
        // public static DecalProperty operator -(DecalProperty a, DecalProperty b)
        // {
        //     DecalProperty result = new DecalProperty();
        //     result.decalSizeScaler.value = a.decalSizeScaler.value - b.decalSizeScaler.value;
        //     result.floorHeight.value = a.floorHeight.value - b.floorHeight.value;
        //     result.decalDepthScaler.value = a.decalDepthScaler.value - b.decalDepthScaler.value;
        //     result.fadeFactor.value = a.fadeFactor.value - b.fadeFactor.value;
        //     result.opacity.value = a.opacity.value - b.opacity.value;
        //     return result;
        // }
        //
        // public static DecalProperty operator -(DecalProperty a)
        // {
        //     DecalProperty result = new DecalProperty();
        //     result.decalSizeScaler.value = -a.decalSizeScaler.value;
        //     result.floorHeight.value = -a.floorHeight.value;
        //     result.decalDepthScaler.value = -a.decalDepthScaler.value;
        //     result.fadeFactor.value = -a.fadeFactor.value;
        //     result.opacity.value = -a.opacity.value;
        //     return result;
        // }
        //
        // public static DecalProperty operator +(DecalProperty a)
        // {
        //     DecalProperty result = new DecalProperty();
        //     result.decalSizeScaler.value = +a.decalSizeScaler.value;
        //     result.floorHeight.value = +a.floorHeight.value;
        //     result.decalDepthScaler.value = +a.decalDepthScaler.value;
        //     result.fadeFactor.value = +a.fadeFactor.value;
        //     result.opacity.value = +a.opacity.value;
        //     return result;
        // }
        //
        // public static DecalProperty operator %(DecalProperty a, DecalProperty b)
        // {
        //     DecalProperty result = new DecalProperty();
        //     result.decalSizeScaler.value = a.decalSizeScaler.value % b.decalSizeScaler.value;
        //     result.floorHeight.value = a.floorHeight.value % b.floorHeight.value;
        //     result.decalDepthScaler.value = a.decalDepthScaler.value % b.decalDepthScaler.value;
        //     result.fadeFactor.value = a.fadeFactor.value % b.fadeFactor.value;
        //     result.opacity.value = a.opacity.value % b.opacity.value;
        //     return result;
        // }
        //
        // public static DecalProperty operator %(DecalProperty a, float b)
        // {
        //     DecalProperty result = new DecalProperty();
        //     result.decalSizeScaler.value = a.decalSizeScaler.value % b;
        //     result.floorHeight.value = a.floorHeight.value % b;
        //     result.decalDepthScaler.value = a.decalDepthScaler.value % b;
        //     result.fadeFactor.value = a.fadeFactor.value % b;
        //     result.opacity.value = a.opacity.value % b;
        //     return result;
        // }
        //
        // public static DecalProperty operator %(float a, DecalProperty b)
        // {
        //     DecalProperty result = new DecalProperty();
        //     result.decalSizeScaler.value = a % b.decalSizeScaler.value;
        //     result.floorHeight.value = a % b.floorHeight.value;
        //     result.decalDepthScaler.value = a % b.decalDepthScaler.value;
        //     result.fadeFactor.value = a % b.fadeFactor.value;
        //     result.opacity.value = a % b.opacity.value;
        //     return result;
        // }
        
        
    }
}