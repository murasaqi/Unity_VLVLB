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
                var qTiltProperty = queueData.stageLightProfile.TryGet<TiltProperty>() as TiltProperty;
                var stageLightBaseProperty = queueData.stageLightProfile.TryGet<StageLightBaseProperty>() as StageLightBaseProperty;
                var weight = queueData.weight;
                if (qTiltProperty == null || stageLightBaseProperty == null) continue;
                var bpm = stageLightBaseProperty.bpm.value;
                var bpmOffset = qTiltProperty.bpmOverrideData.value.bpmOverride ? qTiltProperty.bpmOverrideData.value.bpmOffset : stageLightBaseProperty.bpmOffset.value;
                var bpmScale = qTiltProperty.bpmOverrideData.value.bpmOverride ? qTiltProperty.bpmOverrideData.value.bpmScale : stageLightBaseProperty.bpmScale.value;
                var loopType = qTiltProperty.bpmOverrideData.value.bpmOverride ? qTiltProperty.bpmOverrideData.value.loopType : stageLightBaseProperty.loopType.value;
                var time = GetNormalizedTime(currentTime,bpm,bpmOffset,bpmScale,loopType);
               
                // var end = qTiltProperty.endRoll.value;
                
                if (qTiltProperty.rollTransform.value.mode == AnimationMode.Ease)
                {
                    _angle += EaseUtil.GetEaseValue(qTiltProperty.rollTransform.value.easeType, time, 1f, qTiltProperty.rollTransform.value.rollRange.x, qTiltProperty.rollTransform.value.rollRange.y) * weight;
                    // if(weight >= 1f)Debug.Log($"{queueData.stageLightSetting.name}: {_angle},{time},{weight}");

                }
                else
                {
                    _angle += qTiltProperty.rollTransform.value.animationCurve.Evaluate(time) *weight;
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