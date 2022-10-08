using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace StageLightManeuver
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
    public class StageLightToggleValue<T>:StageLightData
    {
        public T value;
        
        public StageLightToggleValue(StageLightToggleValue<T> stageLightToggleValue)
        {
            propertyOverride = stageLightToggleValue.propertyOverride;
            this.value = stageLightToggleValue.value;
        }

        public StageLightToggleValue()
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
        [DisplayName("Override Time")] public bool bpmOverride = false;
        [DisplayName("BPM Scale")] public float bpmScale = 1;
        [DisplayName("BPM Offset")] public float bpmOffset = 0;
       
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
        [DisplayName("BPM Override")]public StageLightToggleValue<BpmOverrideData> bpmOverrideData = new StageLightToggleValue<BpmOverrideData>(); 
        
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

        public ClipProperty(ClipProperty other)
        {
            clipStartTime = other.clipStartTime;
            clipEndTime = other.clipEndTime;
        }
    }







}