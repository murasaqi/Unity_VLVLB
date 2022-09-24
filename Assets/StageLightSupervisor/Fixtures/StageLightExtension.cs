using System;
using UnityEngine;

namespace StageLightSupervisor
{
    [Serializable]
    public abstract class StageLightExtension: MonoBehaviour
    {
        public float weight;
        public StageLightProperty<float> bpm = new StageLightProperty<float>() { value = 120 };
        public StageLightProperty<float> bpmScale = new StageLightProperty<float>(){value = 1f};
        public StageLightProperty<float> bpmOffset = new StageLightProperty<float>() { value = 0f };
        public StageLightProperty<int> index = new StageLightProperty<int>() { value = 0 };
        public StageLightProperty<LoopType> loopType = new StageLightProperty<LoopType>() { value = LoopType.Loop };
        public ClipProperty clipProperty = new ClipProperty();
        public virtual void UpdateFixture(float currentTime)
        {

        }

        public virtual void Init()
        {
            
        }

        public float GetNormalizedTime(float time)
        {
            var scaledBpm = bpm.value * bpmScale.value;
            var offsetBpm = scaledBpm + (bpmOffset.value * index.value);
            // var offsetChildTime = 60f / offsetBpm;
            var duration = 60 / offsetBpm;
            var t = (float)time % duration;
            // 
            var normalisedTime = t / duration;

            var result = normalisedTime;
            if (loopType.value == LoopType.PingPong)
            {
                var inv = Mathf.CeilToInt(time / duration) % 2 != 0;
                if (inv)
                {
                    result = 1 - normalisedTime;
                }
            }
            else if(loopType.value == LoopType.Fixed)
            {
                result = Mathf.InverseLerp(clipProperty.clipStartTime, clipProperty.clipEndTime, normalisedTime);
            }
           
            return result;
        }
    }
}
