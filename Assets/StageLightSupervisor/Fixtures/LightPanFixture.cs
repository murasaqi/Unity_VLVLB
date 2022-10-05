using System;
using UnityEngine;

namespace StageLightSupervisor
{
    [Serializable]
    public enum LightTransformType
    {
        Pan,
        Tilt
    }
    
    public enum AnimationMode
    {
        Ease,
        AnimationCurve
    }
    
    [ExecuteAlways]
    public class LightPanFixture: StageLightExtension
    {
        private LightTransformType _lightTransformType = LightTransformType.Pan;
        private float _angle;
        public Transform rotateTransform;
        
        public override void UpdateFixture(float currentTime)
        {   
            base.UpdateFixture(currentTime);
            if(rotateTransform == null) return;
           
            _angle = 0f;
            while (stageLightDataQueue.Count>0)
            {
                var queueData = stageLightDataQueue.Dequeue();


                var qLightBaseProperty = queueData.stageLightProfile.TryGet<StageLightBaseProperty>() as StageLightBaseProperty;
                var qPanProperty = queueData.stageLightProfile.TryGet<PanProperty>() as PanProperty;
                var weight = queueData.weight;
                if (qPanProperty == null || qLightBaseProperty == null) continue;
                var bpm = qLightBaseProperty.bpm.value;
                var bpmOffset = qPanProperty.bpmOverrideData.value.bpmOverride
                    ? qPanProperty.bpmOverrideData.value.bpmOffset
                    : qLightBaseProperty.bpmOffset.value;
                var bpmScale = qPanProperty.bpmOverrideData.value.bpmOverride
                    ? qPanProperty.bpmOverrideData.value.bpmScale
                    : qLightBaseProperty.bpmScale.value;
                var loopType = qPanProperty.bpmOverrideData.value.bpmOverride
                    ? qPanProperty.bpmOverrideData.value.loopType
                    : qLightBaseProperty.loopType.value;
                var time = GetNormalizedTime(currentTime,bpm,bpmOffset,bpmScale,loopType);
                // Debug.Log($"{queueData.stageLightSetting.name},{time}");
                if (qPanProperty.rollTransform.value.mode == AnimationMode.Ease)
                {
                    _angle += EaseUtil.GetEaseValue(qPanProperty.rollTransform.value.easeType, time, 1f, qPanProperty.rollTransform.value.rollRange.x,
                        qPanProperty.rollTransform.value.rollRange.y) * weight;
                }
                else
                {
                    _angle += qPanProperty.rollTransform.value.animationCurve.Evaluate(time) * weight;
                }
                
            }
            
        }

        void Update()
        {
            var vec = _lightTransformType == LightTransformType.Pan ? Vector3.up : Vector3.left;
            rotateTransform.localEulerAngles =  vec * _angle;
        }
    }
}