using System;
using UnityEngine;

namespace StageLightSupervisor
{
    [Serializable]
    public abstract class StageLightFixtureData:StageLightData
    {
        public float weight;
        public StageLightProperty<float> bpmScale = new StageLightProperty<float>(){value = 1f};
        public StageLightProperty<float> bpmOffset = new StageLightProperty<float>(){value = 0f};
    }


    [Serializable]
    public class TimeFixture : StageLightFixtureData
    {
        public StageLightProperty<float> bpm = new StageLightProperty<float>(){value = 120f};
        
    }

    [Serializable]
    public class LightFixture : StageLightFixtureData
    {
        public StageLightProperty<Gradient> lightColor = new StageLightProperty<Gradient>(){value = new Gradient()};
        public StageLightProperty<float> lightIntensity = new StageLightProperty<float>(){value = 1f};
        public StageLightProperty<float> spotAngle = new StageLightProperty<float>(){value = 15f};
        public StageLightProperty<float> innerSpotAngle = new StageLightProperty<float>(){value = 10f};
    }
}