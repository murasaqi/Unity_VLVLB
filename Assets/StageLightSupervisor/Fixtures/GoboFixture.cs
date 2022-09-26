using System;
using UnityEngine;

namespace StageLightSupervisor
{
    [ExecuteAlways]
    public class GoboFixture:StageLightExtension
    
    {
        public MeshRenderer vlbMeshRenderer;
        public Texture2D goboTexture;
        private MaterialPropertyBlock _materialPropertyBlock;
        [SerializeField] private string goboPropertyName = "_GoboTexture";
        
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
        
        private void Update()
        {
            if (vlbMeshRenderer != null && goboTexture != null && _materialPropertyBlock != null)
            {
                _materialPropertyBlock.SetTexture(goboPropertyName, goboTexture);
                
            }
            vlbMeshRenderer.SetPropertyBlock(_materialPropertyBlock);
        }
    
    }
}