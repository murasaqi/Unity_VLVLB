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
    public class LightTransformFixture: StageLightExtension
    {
        public LightTransformType lightTransformType = LightTransformType.Pan;
        public RollProperty rollProperty = new RollProperty();
        public Transform rotateTransform;
        public override void UpdateFixture(float currentTime)
        {
            base.UpdateFixture(currentTime);
            if(rotateTransform == null) return;
            var time = GetNormalizedTime(currentTime);
            float angle = 0f;
            if(rollProperty.lighttransformControlType.value == LightTransformControlType.Ease)
            {
                var start = rollProperty.startRoll.value;
                var end = rollProperty.endRoll.value;
                var total = 1f;
                switch (rollProperty.easeType.value)
                {
                    case EaseType.Linear:
                        angle = EaseUtil.EaseLinear(time,total,start,end);
                        break;
                    
                    case EaseType.InQuad:
                        angle = EaseUtil.EaseInQuad(time,total,start,end);
                        break;
                    case EaseType.OutQuad:
                        angle = EaseUtil.EaseOutQuad(time,total,start,end);
                        break;
                    case EaseType.InOutQuad:
                        angle = EaseUtil.EaseInOutQuad(time,total,start,end);
                        break;
                    
                    case EaseType.InCubic:
                        angle = EaseUtil.EaseInCubic(time,total,start,end);
                        break;
                    case EaseType.OutCubic:
                        angle = EaseUtil.EaseOutCubic(time,total,start,end);
                        break;
                    case EaseType.InOutCubic:
                        angle = EaseUtil.EaseInOutCubic(time,total,start,end);
                        break;
                    
                    case EaseType.InQuart:
                        angle = EaseUtil.EaseInQuart(time,total,start,end);
                        break;
                    case EaseType.OutQuart:
                        angle = EaseUtil.EaseOutQuart(time,total,start,end);
                        break;
                    case EaseType.InOutQuart:
                        angle = EaseUtil.EaseInOutQuart(time,total,start,end);
                        break;
                    
                    case EaseType.InQuint:
                        angle = EaseUtil.EaseInQuint(time,total,start,end);
                        break;
                    case EaseType.OutQuint:
                        angle = EaseUtil.EaseOutQuint(time,total,start,end);
                        break;
                    case EaseType.InOutQuint:
                        angle = EaseUtil.EaseInOutQuint(time,total,start,end);
                        break;
                    
                    case EaseType.InSine:
                        angle = EaseUtil.EaseInSine(time,total,start,end);
                        break;
                    case EaseType.OutSine:
                        angle = EaseUtil.EaseOutSine(time,total,start,end);
                        break;
                    case EaseType.InOutSine:
                        angle = EaseUtil.EaseInOutSine(time,total,start,end);
                        break;

                    case EaseType.InExpo:
                        angle = EaseUtil.EaseInExpo(time,total,start,end);
                        break;
                    case EaseType.OutExpo:
                        angle = EaseUtil.EaseOutExpo(time,total,start,end);
                        break;
                    case EaseType.InOutExpo:
                        angle = EaseUtil.EaseInOutExpo(time,total,start,end);
                        break;
                    
                    case EaseType.InCirc:
                        angle = EaseUtil.EaseInCirc(time,total,start,end);
                        break;
                    case EaseType.OutCirc:
                        angle = EaseUtil.EaseOutCirc(time,total,start,end);
                        break;
                    case EaseType.InOutCirc:
                        angle = EaseUtil.EaseInOutCirc(time,total,start,end);
                        break;
                    
                    case EaseType.InBack:
                        angle = EaseUtil.EaseInBack(time,total,start,end);
                        break;
                    case EaseType.OutBack:
                        angle = EaseUtil.EaseOutBack(time,total,start,end);
                        break;
                    case EaseType.InOutBack:
                        angle = EaseUtil.EaseInOutBack(time,total,start,end);
                        break;
                    
                    case EaseType.InBounce:
                        angle = EaseUtil.EaseInBounce(time,total,start,end);
                        break;
                    case EaseType.OutBounce:
                        angle = EaseUtil.EaseOutBounce(time,total,start,end);
                        break;
                    case EaseType.InOutBounce:
                        angle = EaseUtil.EaseInOutBounce(time,total,start,end);
                        break;
                    
                    case EaseType.InElastic:
                        angle = EaseUtil.EaseInElastic(time,total,start,end);
                        break;
                    case EaseType.OutElastic:
                        angle = EaseUtil.EaseOutElastic(time,total,start,end);
                        break;
                    case EaseType.InOutElastic:
                        angle = EaseUtil.EaseInOutElastic(time,total,start,end);
                        break;
                }
                
                Debug.Log(angle);
            }
            else
            {
                angle = rollProperty.animationCurve.value.Evaluate(currentTime);
            }
            
            var vec = lightTransformType == LightTransformType.Pan ? Vector3.up : Vector3.left;
            rotateTransform.localEulerAngles =  vec * angle;
        }
    }
}