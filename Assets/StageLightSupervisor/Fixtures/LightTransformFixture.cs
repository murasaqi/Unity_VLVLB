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
    public class LightTransformFixture: StageLightExtension
    {
        public LightTransformType lightTransformType = LightTransformType.Pan;
        public RollProperty rollProperty = new RollProperty();
        public Transform rotateTransform;
        public override void Update(float currentTime)
        {
            if(rotateTransform == null) return;
            base.Update(currentTime);
            var t = GetNormalizedTime(currentTime);
            var vec = lightTransformType == LightTransformType.Pan ? Vector3.up : Vector3.left;
            rotateTransform.localEulerAngles =  vec * rollProperty.animationCurve.value.Evaluate(t);
        }
    }
}