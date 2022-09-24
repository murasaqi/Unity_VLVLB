using UnityEngine;
namespace StageLightSupervisor
{
    public class TimeFixture: StageLightExtension
    {
        public StageLightProperty<float> bpm = new StageLightProperty<float>() { value = 120 };
        public StageLightProperty<float> bpmOffset = new StageLightProperty<float>() { value = 0f };

    }
}
