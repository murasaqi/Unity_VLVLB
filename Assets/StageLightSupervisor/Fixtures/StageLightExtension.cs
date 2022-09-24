using System;
using UnityEngine;

namespace StageLightSupervisor
{
    [Serializable]
    public abstract class StageLightExtension: MonoBehaviour
    {
        public StageLightExtensionProperty stageLightExtensionProperty = new StageLightExtensionProperty();
        public ClipProperty clipProperty = new ClipProperty();
        public virtual void UpdateFixture(float currentTime)
        {

        }

        public virtual void Init()
        {
            
        }

        public float GetNormalizedTime(float time)
        {
            var scaledBpm = stageLightExtensionProperty.bpm.value * stageLightExtensionProperty.bpmScale.value;
            var offsetBpm = scaledBpm + (stageLightExtensionProperty.bpmOffset.value * stageLightExtensionProperty.index.value);
            // var offsetChildTime = 60f / offsetBpm;
            var duration = 60 / offsetBpm;
            var t = (float)time % duration;
            // 
            var normalisedTime = t / duration;

            var result = normalisedTime;
            if (stageLightExtensionProperty.loopType.value == LoopType.PingPong)
            {
                var inv = Mathf.CeilToInt(time / duration) % 2 != 0;
                if (inv)
                {
                    result = 1 - normalisedTime;
                }
            }
            else if(stageLightExtensionProperty.loopType.value == LoopType.Fixed)
            {
                result = Mathf.InverseLerp(clipProperty.clipStartTime, clipProperty.clipEndTime, normalisedTime);
            }
           
            return result;
        }
    }
}
