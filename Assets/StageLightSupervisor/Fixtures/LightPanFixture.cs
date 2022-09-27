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
    
    public enum LightTransformControlType
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


                var qLightBaseProperty = queueData.stageLightProfile.stageLightBaseProperty;
               
                
                var qPanProperty = queueData.stageLightProfile.panProperty;
                var weight = queueData.weight;
                if (qPanProperty == null) continue;
                var time = GetNormalizedTime(currentTime,qLightBaseProperty,qPanProperty.LoopType);
                // Debug.Log($"{queueData.stageLightSetting.name},{time}");
                if (qPanProperty.lightTransformControlType.value == LightTransformControlType.Ease)
                {
                    
                    // Debug.Log($"{qPanProperty.startRoll.value},{qPanProperty.endRoll.value},{weight}");
                    _angle += EaseUtil.GetEaseValue(qPanProperty.easeType.value, time, 1f, qPanProperty.startRoll.value,
                        qPanProperty.endRoll.value) * weight;
                }
                else
                {
                    _angle += qPanProperty.animationCurve.value.Evaluate(currentTime) * weight;
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