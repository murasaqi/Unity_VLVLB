using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace StageLightSupervisor
{
    [ExecuteAlways]
    public class DecalFixture: StageLightExtension
    {
        public LightFixture lightFixture;
        public DecalProperty decalProperty = new DecalProperty();
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

        private void Update()
        {
            if(decalProjector ==null) return;
            
            var floor = new Vector3(0,decalProperty.floorHeight.value,0);
            var distance = Vector3.Distance(transform.position,floor);
            var angle = lightFixture.lightProperty.spotAngle.value;
            _radius = Mathf.Tan(angle * Mathf.Deg2Rad) * distance * decalProperty.decalSizeScaler.value;
            _depth = distance * decalProperty.decalDepthScaler.value;
            
            decalProjector.size = new Vector3(_radius,_radius, _depth);
            decalProjector.fadeFactor = decalProperty.fadeFactor.value;
            decalProjector.pivot = new Vector3(0, 0, _depth / 2f);

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
        
        public override void UpdateFixture(float time)
        {
            if(decalMaterial == null || decalProjector == null || decalProjector.material == null) return;
            var t = GetNormalizedTime(time);
            decalProjector.material.SetFloat("_Alpha",decalProperty.opacity.value);
            decalProjector.material.SetColor("_Color",lightFixture.lightProperty.lightColor.value.Evaluate(t));

           
            
        }
    }
}
