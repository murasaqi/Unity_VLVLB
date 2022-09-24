using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;


namespace StageLightSupervisor
{
    public class MaterialColorFixture : StageLightExtension
    {
        public LightProperty lightProperty = new LightProperty();
        public StageLightProperty<bool> fromLightFixture = new StageLightProperty<bool>();
        public MeshRenderer meshRenderer;
        public StageLightProperty<string> materialPropertyName = new StageLightProperty<string>(){value = "_Color"};
        private MaterialPropertyBlock _materialPropertyBlock;

        public LightFixture lightFixture;
        private void Start()
        {
            Init();
        }

        private void OnEnable()
        {
            Init(); 
            lightFixture = GetComponent<LightFixture>();
        }

        public override void Init()
        {
            _materialPropertyBlock = new MaterialPropertyBlock();
            if(meshRenderer)meshRenderer.GetPropertyBlock(_materialPropertyBlock);
        }
        public override void UpdateFixture(float currentTime)
        {
            if(meshRenderer == null || _materialPropertyBlock == null) return;
            base.UpdateFixture(currentTime);
            
            var t = GetNormalizedTime(currentTime);
            
            if(fromLightFixture.value && lightFixture != null)
            {
                _materialPropertyBlock.SetColor(materialPropertyName.value,lightFixture.lightProperty.lightColor.value.Evaluate(t));
            }
            else
            {
                _materialPropertyBlock.SetColor(materialPropertyName.value,lightProperty.lightColor.value.Evaluate(t));
            }
            meshRenderer.SetPropertyBlock(_materialPropertyBlock);
        
        }
    }

}
