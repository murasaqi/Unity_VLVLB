using System;
using System.Collections.Generic;
using UnityEngine;

namespace StageLightSupervisor
{
    
    [Serializable]
    public class StageLightDataQueue
    {
        public StageLightSetting stageLightSetting;
        public float weight = 1;
    }
    
    [Serializable]
    public abstract class StageLightExtension: MonoBehaviour,IStageLight
    {
        // public StageLightBaseProperty stageLightBaseProperty = new StageLightBaseProperty();
        public ClipProperty clipProperty = new ClipProperty();
        public Queue<StageLightDataQueue> stageLightDataQueue = new Queue<StageLightDataQueue>();

        public int Index { get; set; }
        public List<StageLight> StageLightGroup { get; set; }

        public virtual void UpdateFixture(float currentTime)
        {

        }

        public virtual void Init()
        {
            
        }
        
        
        public float GetNormalizedTime(float time,StageLightBaseProperty stageLightBaseProperty,LoopType loopType)
        {
            
            var scaledBpm = stageLightBaseProperty.bpm.value * stageLightBaseProperty.bpmScale.value;
            // var offsetBpm = scaledBpm + (stageLightBaseProperty.bpmOffset.value * Index);
            // var offsetChildTime = 60f / offsetBpm;
            var duration = 60 / scaledBpm;
            var offset = duration* stageLightBaseProperty.bpmOffset.value * Index;
            Debug.Log(offset);
            var offsetTime = time + offset;
            var t = (float)offsetTime % duration;
            // 
            var normalisedTime = t / duration;

            var result = normalisedTime;
            if (loopType == LoopType.PingPong)
            {
                var inv = Mathf.CeilToInt(offsetTime / duration) % 2 != 0;
                if (inv)
                {
                    result = 1 - normalisedTime;
                }
            }
            else if(loopType == LoopType.Fixed)
            {
                result = Mathf.InverseLerp(clipProperty.clipStartTime, clipProperty.clipEndTime, normalisedTime);
            }
           
            return result;
        }
    }
}
