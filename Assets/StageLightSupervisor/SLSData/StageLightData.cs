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
        
        public StageLightValue(StageLightValue<T> stageLightValue)
        {
            propertyOverride = stageLightValue.propertyOverride;
            this.value = stageLightValue.value;
        }

        public StageLightValue()
        {
            propertyOverride = false;
            value = default;
        }
    }


    [Serializable]
    public class StageLightProperty:StageLightData
    {
        public string propertyName;


    }
    
    
    [Serializable]
    public class BpmOverrideData:StageLightData
    {
        [DisplayName("Loop Type")] public LoopType loopType = LoopType.Loop;
        [DisplayNameAttribute("Override Time")] public bool bpmOverride = false;
        [DisplayNameAttribute("BPM Scale")] public float bpmScale = 1;
        [DisplayNameAttribute("BPM Offset")] public float bpmOffset = 0;
       
        public BpmOverrideData()
        {
            propertyOverride = false;
            bpmScale = 1;
            bpmOffset = 0;
            loopType = LoopType.Loop;
        }
    }
    
    
    [Serializable]
    public class StageLightAdditionalProperty:StageLightProperty
    {
        [DisplayNameAttribute("BPM Override")]public StageLightValue<BpmOverrideData> bpmOverrideData = new StageLightValue<BpmOverrideData>(); 
        
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