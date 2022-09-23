using System;
using UnityEngine;

namespace StageLightSupervisor
{
    [Serializable]
    public abstract class StageLightExtension: MonoBehaviour
    {
        public float weight;
        public StageLightProperty<float> bpm = new StageLightProperty<float>() { value = 120 };
        public StageLightProperty<float> bpmScale = new StageLightProperty<float>() { value = 1f };
        public StageLightProperty<float> bpmOffset = new StageLightProperty<float>() { value = 0f };
        public StageLightProperty<int> index = new StageLightProperty<int>() { value = 0 };
        public virtual void Update(float currentTime)
        {

        }


        public float GetNormalizedTime(float time)
        {
            var scaledBpm = bpm.value * bpmScale.value;
            // var offsetBpm = scaledBpm + (bpmOffset.value * index.value);
            // var offsetChildTime = 60f / offsetBpm;
            var duration = 60 / scaledBpm;
            var t = (float)time % duration;
            // var inv = Mathf.CeilToInt((float) offsetTime / duration) % 2 != 0;
            var normalisedTime = t / duration;
            // if (loopType == LoopType.Loop) return normalisedTime;
            // return inv ? 1f - normalisedTime : normalisedTime;
            return t;
        }
    }
}
