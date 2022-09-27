using System;
using System.Collections.Generic;
using UnityEngine;

namespace StageLightSupervisor
{
    
    [Serializable]
    public class StageLightDataQueue
    {
        public StageLightProfile stageLightProfile;
        public float weight = 1;
    }
    
    [Serializable]
    public abstract class StageLightExtension: MonoBehaviour,IStageLight
    {
        // public StageLightBaseProperty stageLightBaseProperty = new StageLightBaseProperty();
        public ClipProperty clipProperty = new ClipProperty();
        public Queue<StageLightDataQueue> stageLightDataQueue = new Queue<StageLightDataQueue>();

        public int Index { get; set; }
        public List<StageLight> StageLightChild { get; set; }

        public virtual void UpdateFixture(float currentTime)
        {

        }

        public virtual void Init()
        {
            
        }
        
        
        public float GetNormalizedTime(float time,StageLightBaseProperty stageLightBaseProperty,LoopType loopType)
        {
            
            var scaledBpm = stageLightBaseProperty.bpm.value * stageLightBaseProperty.bpmScale.value;
            var duration = 60 / scaledBpm;
            var offset = duration* stageLightBaseProperty.bpmOffset.value * Index;
            var offsetTime = time + offset;
            var result = 0f;
            var t = (float)offsetTime % duration;
            var normalisedTime = t / duration;
            
            if (loopType == LoopType.Loop)
            {
                result = normalisedTime;     
            }else if (loopType == LoopType.PingPong)
            {
                result = Mathf.PingPong(offsetTime / duration, 1f);
            }
            else if(loopType == LoopType.Fixed)
            {
                result = Mathf.InverseLerp(clipProperty.clipStartTime, clipProperty.clipEndTime, normalisedTime);
            }
           
            return result;
        }
    }
}
