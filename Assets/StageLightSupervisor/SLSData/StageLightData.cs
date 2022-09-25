using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace StageLightSupervisor
{
    [Serializable]
    public class StageLightData
    {
        [SerializeField] public bool propertyOverride = false;
    }

    [Serializable]
    public class StageLightProperty<T>:StageLightData
    {
        
        [SerializeField]public T value;
        
    }


    public class StageLightProperty:StageLightData
    {
        public LoopType LoopType;
    }

    [Serializable]
    public class StageLightBaseProperty: StageLightProperty
    {
        public StageLightProperty<float> bpm = new StageLightProperty<float>() { value = 120 };
        public StageLightProperty<float> bpmScale = new StageLightProperty<float>(){value = 1f};
        public StageLightProperty<float> bpmOffset = new StageLightProperty<float>() { value = 0f };
        
        public StageLightBaseProperty()
        {
            bpm = new StageLightProperty<float>() { value = 120 };
            bpmScale = new StageLightProperty<float>() { value = 1f };
            bpmOffset = new StageLightProperty<float>() { value = 0f };
            // index = new StageLightProperty<int>() { value = 0 };
            // loopType = new StageLightProperty<LoopType>() { value = LoopType.Loop };
        }
            
        public StageLightBaseProperty(StageLightBaseProperty other)
        {
            bpm = other.bpm;
            bpmScale = other.bpmScale;
            bpmOffset = other.bpmOffset;
            // index = other.index;
            // loopType = other.loopType;
        }
    }
    //
    // [Serializable]
    // public class BpmProperty: StageLightData
    // {
    //     public StageLightProperty<float> bpmScale = new StageLightProperty<float>(){value = 1f};
    //     public StageLightProperty<float> bpmOffset = new StageLightProperty<float>() { value = 0f };
    //         
    //     public BpmProperty()
    //     {
    //         bpmScale = new StageLightProperty<float>(){value = 1f};
    //         bpmOffset = new StageLightProperty<float>() { value = 0f };
    //     }
    //     
    //     public BpmProperty(BpmProperty other)
    //     {
    //         bpmScale = other.bpmScale;
    //         bpmOffset = other.bpmOffset;
    //     }
    // }

    [Serializable]

    public class ClipProperty
    {
        public float clipStartTime;
        public float clipEndTime;
        
        public ClipProperty()
        {
            clipStartTime = 0f;
            clipEndTime = 0f;
        }
    }







}