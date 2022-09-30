using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace StageLightSupervisor
{

    [System.AttributeUsage(System.AttributeTargets.Field)]
    public class DisplayNameAttribute : System.Attribute
    {
        public string name;
        public DisplayNameAttribute(string name)
        {
            this.name = name;
        }
    }
    
    
    [Serializable]
    public class StageLightData
    {
        [SerializeField] public bool propertyOverride = false;
    }

    [Serializable]
    public class StageLightValue<T>:StageLightData
    {
        [SerializeField]public T value;
    }


    [Serializable]
    public class StageLightProperty:StageLightData
    {
        public LoopType LoopType;
    }

    [Serializable]
    public class StageLightBaseProperty: StageLightProperty
    {
        [DisplayNameAttribute("BPM")]public StageLightValue<float> bpm = new StageLightValue<float>() { value = 120 };
        [DisplayNameAttribute("BPM Scale")]public StageLightValue<float> bpmScale = new StageLightValue<float>(){value = 1f};
        [DisplayNameAttribute("BPM Offset")]public StageLightValue<float> bpmOffset = new StageLightValue<float>() { value = 0f };
        
        public StageLightBaseProperty()
        {
            bpm = new StageLightValue<float>() { value = 120 };
            bpmScale = new StageLightValue<float>() { value = 1f };
            bpmOffset = new StageLightValue<float>() { value = 0f };
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