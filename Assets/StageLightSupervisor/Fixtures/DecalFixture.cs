using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace StageLightSupervisor
{
    [ExecuteAlways]
    public class DecalFixture: StageLightExtension
    {
        public LightFixture lightFixture;
        public Color decalColor = Color.white;
        public float decalSizeScaler = 1f;
        public float floorHeight = 0f;
        public float decalDepthScaler = 1f;
        public float fadeFactor = 1f;
        public float opacity = 1f;
        public DecalProjector decalProjector;
        public Material decalMaterial;
        private Material _instancedDecalMaterial = null;
        private float _radius = 1f;
        private float _depth = 1f;
        private void Start()
        {
            Init();
        }

        private void OnEnable()
        {
            Init();
        }
        
        
        
        public override void UpdateFixture(float time)
        {
            if(decalMaterial == null || decalProjector == null || decalProjector.material == null) return;
            

            opacity = 0f;
            fadeFactor = 0f;
            decalSizeScaler = 0f;
            decalDepthScaler = 0f;
            floorHeight = 0f;
            
            
            while (stageLightDataQueue.Count >0)
            {
                
                var queueData = stageLightDataQueue.Dequeue();
                var qDecalProperty = queueData.stageLightSetting.decalProperty;
                var weight = queueData.weight;
                if (qDecalProperty == null) continue;
                var t = GetNormalizedTime(time,queueData.stageLightSetting.stageLightBaseProperty, qDecalProperty.LoopType);
                opacity += qDecalProperty.opacity.value * weight;
                fadeFactor += qDecalProperty.fadeFactor.value * weight;
                decalSizeScaler += qDecalProperty.decalSizeScaler.value * weight;
                decalDepthScaler += qDecalProperty.decalDepthScaler.value * weight;
                floorHeight += qDecalProperty.floorHeight.value * weight;
            }

            decalColor = lightFixture.lightColor;

        }

        private void Update()
        {
            if(decalProjector ==null) return;
            
            var floor = new Vector3(0,floorHeight,0);
            var distance = Vector3.Distance(transform.position,floor);
            var angle = lightFixture.spotAngle;
            _radius = Mathf.Tan(angle * Mathf.Deg2Rad) * distance * decalSizeScaler;
            _depth = distance * decalDepthScaler;
            
            decalProjector.size = new Vector3(_radius,_radius, _depth);
            decalProjector.fadeFactor = fadeFactor;
            decalProjector.pivot = new Vector3(0, 0, _depth / 2f);

            decalProjector.material.SetFloat("_Alpha",opacity);
            decalProjector.material.SetColor("_Color",decalColor);
        }
        public override void Init()
        {
            if(_instancedDecalMaterial != null) DestroyImmediate(_instancedDecalMaterial);
            if (decalProjector != null)
            {
                _instancedDecalMaterial = Material.Instantiate(decalMaterial);
                decalProjector.material = _instancedDecalMaterial;     
            }
            
            lightFixture = GetComponent<LightFixture>();
        }
        
        
    }
}
