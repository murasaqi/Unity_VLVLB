using System;
using UnityEngine;

namespace StageLightSupervisor
{
    [ExecuteAlways]
    public class GoboFixture:StageLightExtension
    
    {
        public MeshRenderer vlbMeshRenderer;
        public Texture2D goboTexture;
        public string goboPropertyName = "_GoboTexture";
        private MaterialPropertyBlock _materialPropertyBlock;
        
        
        
        private void Start()
        {
            Init();
        }

        private void OnEnable()
        {
            Init();
        }
        
        [ContextMenu("Init")]
        public override void Init()
        {
            _materialPropertyBlock = new MaterialPropertyBlock();
            if(vlbMeshRenderer)vlbMeshRenderer.GetPropertyBlock(_materialPropertyBlock);
        }

        public override void UpdateFixture(float time)
        {
            while (stageLightDataQueue.Count > 0)
            {
                var queueData = stageLightDataQueue.Dequeue();
                var stageLightBaseProperties = queueData.stageLightProfile.TryGet<StageLightBaseProperty>() as StageLightBaseProperty;
                var goboProperty = queueData.stageLightProfile.TryGet<GoboProperty>() as GoboProperty;
                
                if(goboProperty ==null || stageLightBaseProperties == null) continue;
                if (queueData.weight > 0.5f)
                {
                    goboTexture = goboProperty.goboTexture.value;
                    goboPropertyName = goboProperty.goboPropertyName.value;
                }
                

            }
        }

        private void Update()
        {
            if (vlbMeshRenderer != null && goboTexture != null && _materialPropertyBlock != null)
            {
                _materialPropertyBlock.SetTexture(goboPropertyName,goboTexture);
                vlbMeshRenderer.SetPropertyBlock(_materialPropertyBlock);
            }
           
        }
    
    }
}