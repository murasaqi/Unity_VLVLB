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
        private Material instancedDecalMaterial = null;
        private float radius = 1f;
        private float depth = 1f;
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
            var floor = new Vector3(0,decalProperty.floorHeight.value,0);
            var distance = Vector3.Distance(transform.position,floor);
            var angle = lightFixture.lightProperty.spotAngle.value;
            radius = Mathf.Tan(angle * Mathf.Deg2Rad) * distance * decalProperty.decalSizeScaler.value;
            depth = distance * decalProperty.decalDepthScaler.value;
            
            decalProjector.size = new Vector3(radius,radius, depth);
            decalProjector.fadeFactor = decalProperty.fadeFactor.value;
            decalProjector.pivot = new Vector3(0, 0, depth / 2f);

        }
        public override void Init()
        {
            if(instancedDecalMaterial != null) DestroyImmediate(instancedDecalMaterial);
            if (decalProjector != null)
            {
                instancedDecalMaterial = Material.Instantiate(decalMaterial);
                decalProjector.material = instancedDecalMaterial;     
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
