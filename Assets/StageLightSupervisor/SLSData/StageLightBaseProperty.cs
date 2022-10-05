using System;

namespace StageLightSupervisor
{
    [Serializable]
    public class StageLightBaseProperty: StageLightProperty
    {
        [DisplayNameAttribute("Loop Type")] public StageLightValue<LoopType> loopType = new StageLightValue<LoopType>() { value = LoopType.Loop };
        [DisplayNameAttribute("BPM")]public StageLightValue<float> bpm = new StageLightValue<float>() { value = 120 };
        [DisplayNameAttribute("BPM Scale")]public StageLightValue<float> bpmScale = new StageLightValue<float>(){value = 1f};
        [DisplayNameAttribute("BPM Offset")]public StageLightValue<float> bpmOffset = new StageLightValue<float>() { value = 0f };
        
        public StageLightBaseProperty()
        {
            propertyName = "Time";
            propertyOverride = false;
            loopType = new StageLightValue<LoopType>(){value = LoopType.Loop};
            bpm = new StageLightValue<float>() { value = 120 };
            bpmScale = new StageLightValue<float>() { value = 1f };
            bpmOffset = new StageLightValue<float>() { value = 0f };
        }
            
        public StageLightBaseProperty(StageLightBaseProperty other)
        {
            propertyOverride = other.propertyOverride;
            propertyName = other.propertyName;
            bpm = other.bpm;
            bpmScale = other.bpmScale;
            bpmOffset = other.bpmOffset;
            loopType = other.loopType;
            
        }
    }
}