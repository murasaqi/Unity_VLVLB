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
        private MaterialPropertyBlock materialPropertyBlock;

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
            materialPropertyBlock = new MaterialPropertyBlock();
            if(meshRenderer)meshRenderer.GetPropertyBlock(materialPropertyBlock);
        }
        public override void UpdateFixture(float currentTime)
        {
            if(meshRenderer == null || materialPropertyBlock == null) return;
            base.UpdateFixture(currentTime);
            
            var t = GetNormalizedTime(currentTime);
            
            if(fromLightFixture.value && lightFixture != null)
            {
                materialPropertyBlock.SetColor(materialPropertyName.value,lightFixture.lightProperty.lightColor.value.Evaluate(t));
            }
            else
            {
                materialPropertyBlock.SetColor(materialPropertyName.value,lightProperty.lightColor.value.Evaluate(t));
            }
            meshRenderer.SetPropertyBlock(materialPropertyBlock);
        
        }
    }

}
