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
                var qTiltProperty = queueData.stageLightSetting.tiltProperty;
                var stageLightBaseProperty = queueData.stageLightSetting.stageLightBaseProperty;
                var weight = queueData.weight;
                if (qTiltProperty == null) continue;
                var time = GetNormalizedTime(currentTime,stageLightBaseProperty,qTiltProperty.LoopType);
                var start = qTiltProperty.startRoll.value;
                var end = qTiltProperty.endRoll.value;
                
                if (qTiltProperty.lightTransformControlType.value == LightTransformControlType.Ease)
                {
                    _angle += EaseUtil.GetEaseValue(qTiltProperty.easeType.value, time, 1f, start, end) * weight;
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