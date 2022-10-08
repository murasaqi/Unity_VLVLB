using System;

namespace StageLightManeuver
{
    [Serializable]
    public class TimeProperty: StageLightProperty
    {
        [DisplayName("Clip Duration")] public ClipProperty clipProperty;
        [DisplayName("Loop Type")] public StageLightToggleValue<LoopType> loopType = new StageLightToggleValue<LoopType>() { value = LoopType.Loop };
        [DisplayName("BPM")]public StageLightToggleValue<float> bpm = new StageLightToggleValue<float>() { value = 120 };
        [DisplayName("BPM Scale")]public StageLightToggleValue<float> bpmScale = new StageLightToggleValue<float>(){value = 1f};
        [DisplayName("BPM Offset")]public StageLightToggleValue<float> bpmOffset = new StageLightToggleValue<float>() { value = 0f };
        
        public TimeProperty()
        {
            propertyName = "Time";
            propertyOverride = false;
            loopType = new StageLightToggleValue<LoopType>(){value = LoopType.Loop};
            clipProperty = new ClipProperty(){clipStartTime = 0, clipEndTime = 0};
            bpm = new StageLightToggleValue<float>() { value = 120 };
            bpmScale = new StageLightToggleValue<float>() { value = 1f };
            bpmOffset = new StageLightToggleValue<float>() { value = 0f };
        }
            
        public TimeProperty(TimeProperty other)
        {
            propertyOverride = other.propertyOverride;
            propertyName = other.propertyName;
            bpm = new StageLightToggleValue<float>(other.bpm);
            bpmScale = new StageLightToggleValue<float>(other.bpmScale);
            bpmOffset = new StageLightToggleValue<float>(other.bpmOffset);
            loopType = new StageLightToggleValue<LoopType>(other.loopType);
            clipProperty = new ClipProperty(other.clipProperty);
            
        }
    }
}