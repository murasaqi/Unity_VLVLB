using UnityEngine;

namespace StageLightSupervisor
{
    [ExecuteAlways]
    public class LightTiltFixture: StageLightExtension
    {
        private LightTransformType _lightTransformType = LightTransformType.Tilt;
        private float _angle =0f;
        public Transform rotateTransform;
        public override void UpdateFixture(float currentTime)
        {
            base.UpdateFixture(currentTime);
            if(rotateTransform == null) return;
            _angle = 0f;
            while (stageLightDataQueue.Count>0)
            {
                var queueData = stageLightDataQueue.Dequeue();
                var qTiltProperty = queueData.stageLightProfile.tiltProperty;
                var stageLightBaseProperty = queueData.stageLightProfile.stageLightBaseProperty;
                var weight = queueData.weight;
                if (qTiltProperty == null) continue;
                var time = GetNormalizedTime(currentTime,stageLightBaseProperty,qTiltProperty.LoopType);
                var start = qTiltProperty.rollTransform.value;
                // var end = qTiltProperty.endRoll.value;
                
                if (qTiltProperty.lightTransformControlType.value == AnimationMode.Ease)
                {
                    _angle += EaseUtil.GetEaseValue(qTiltProperty.rollTransform.value.easeType, time, 1f, qTiltProperty.rollTransform.value.rollRange.x, qTiltProperty.rollTransform.value.rollRange.y) * weight;
                    // if(weight >= 1f)Debug.Log($"{queueData.stageLightSetting.name}: {_angle},{time},{weight}");

                }
                else
                {
                    _angle += qTiltProperty.animationCurve.value.Evaluate(currentTime) *weight;
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