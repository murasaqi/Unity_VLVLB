using System;

namespace StageLightSupervisor
{
    [Serializable]
    public class StageLightBaseProperty: StageLightProperty
    {
        [DisplayNameAttribute("BPM")]public StageLightValue<float> bpm = new StageLightValue<float>() { value = 120 };
        [DisplayNameAttribute("BPM Scale")]public StageLightValue<float> bpmScale = new StageLightValue<float>(){value = 1f};
        [DisplayNameAttribute("BPM Offset")]public StageLightValue<float> bpmOffset = new StageLightValue<float>() { value = 0f };
        
        public StageLightBaseProperty()
        {
            propertyName = "Time";
            bpm = new StageLightValue<float>() { value = 120 };
            bpmScale = new StageLightValue<float>() { value = 1f };
            bpmOffset = new StageLightValue<float>() { value = 0f };
            // index = new StageLightProperty<int>() { value = 0 };
            // loopType = new StageLightProperty<LoopType>() { value = LoopType.Loop };
        }
            
        public StageLightBaseProperty(StageLightBaseProperty other)
        {
            propertyName = other.propertyName;
            bpm = other.bpm;
            bpmScale = other.bpmScale;
            bpmOffset = other.bpmOffset;
            // index = other.index;
            // loopType = other.loopType;
        }
    }
}