using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using VLB;

namespace VLVLB
{

    public class PanTiltLight : MonoBehaviour
    {
        public Transform pan;
        public Transform tilt;
        public List<VolumetricLightBeam> volumetricLightBeams = new List<VolumetricLightBeam>();
        public MeshRenderer emissionMatMeshRenderer;
        public DecalProjector decalProjector;
        private MaterialPropertyBlock materialPropertyBlock;
        public string emissionPropertyName = "_Emission";
        // public float materialEmissionPower = 1.2f;

        private Material decalMaterial = null;
        // public MeshRenderer MeshRenderer;
        private List<Material> instanced = new List<Material>();

        void Start()
        {
            InitDecal();
        }

        [ContextMenu("Init")]
        public void Init()
        {
            InitDecal();
        }

        private void OnEnable()
        {
            InitDecal();
        }

        private void OnDestroy()
        {

            DestroyImmediate(decalMaterial);

            foreach (var i in instanced)
            {
                DestroyImmediate(i);
            }
        }

        public void InitDecal()
        {
            if(decalMaterial != null) DestroyImmediate(decalMaterial);
            if (decalProjector != null)
            {
                decalMaterial = Material.Instantiate(Resources.Load<Material>("VLVLBMaterials/DecalMat"));
                decalProjector.material = decalMaterial;     
            }
           
        }



        public void UpdatePtl(int universe, int index, List<PTLProps> propsCue)
        {

          
            if (materialPropertyBlock == null) materialPropertyBlock = new MaterialPropertyBlock();

            float p = 0f, t = 0f, intensity = 0f, spotAngle = 0f, rangeLimit = 0f, truncatedRadius = 0f, decalOpacity = 0f,decalDepth = 0f;
            var color = new Color();
            var decalSize = new Vector2(0, 0);
            var rootEmissionPower = 0f;
            var count = 0;
            foreach (var cue in propsCue)
            {
                var offsetTime = cue.GetNormalizedTime(universe,index, 1);
                p += cue.pan.Evaluate(cue.ignoreOffsetPan
                    ? cue.GetNormalizedTime(universe,0, cue.timeScalePan)
                    : cue.GetNormalizedTime(universe,index, cue.timeScalePan)) * cue.weight;
                t += cue.tilt.Evaluate(cue.ignoreOffsetTilt
                    ? cue.GetNormalizedTime(universe,0, cue.timeScaleTilt)
                    : cue.GetNormalizedTime(universe,index, cue.timeScaleTilt)) * cue.weight;
                intensity += cue.intensity.Evaluate(cue.ignoreOffsetIntensity
                    ? cue.GetNormalizedTime(universe,0, cue.timeScaleIntensity)
                    : cue.GetNormalizedTime(universe,index, cue.timeScaleIntensity)) * cue.weight;
                // Debug.Log(color);
                // Debug.Log(color);
                color += cue.color.Evaluate(cue.ignoreOffsetColor
                    ? cue.GetNormalizedTime(universe,0, cue.timeScaleColor)
                    : cue.GetNormalizedTime(universe,index, cue.timeScaleColor)) * cue.weight;
                spotAngle += cue.spotAngle * cue.weight;
                rangeLimit += cue.rangeLimit * cue.weight;
                truncatedRadius += cue.truncatedRadius * cue.weight;
                decalOpacity += cue.decalOpacity * cue.weight;
                decalDepth += cue.decalDepth * cue.weight;
                decalSize += cue.decalSize * cue.weight;
                rootEmissionPower += cue.rootEmissionPower * cue.weight;
                // Debug.Log($"{ cue.decalSize},{decalSize}");
                count++;
            }

            if (pan != null) pan.localEulerAngles = Vector3.up * p;
            if (tilt != null) tilt.localEulerAngles = Vector3.left * t;
            if (volumetricLightBeams != null)
            {
                foreach (var volumetricLightBeam in volumetricLightBeams)
                {
                    volumetricLightBeam.intensityGlobal = intensity;
                    volumetricLightBeam.color = color;
                    volumetricLightBeam.spotAngleFromLight = false;
                    volumetricLightBeam.fallOffEnd = rangeLimit;
                    volumetricLightBeam.coneRadiusStart = truncatedRadius;
                    volumetricLightBeam.spotAngle = spotAngle;    
                }
            }

            if (decalProjector != null)
            {
                if (decalMaterial == null) InitDecal();

                decalProjector.material.SetFloat("_Alpha",color.a);
                decalProjector.material.SetColor("_Color",color);
                decalProjector.size = new Vector3(decalSize.x, decalSize.y, decalDepth);
                // decalProjector.
                decalProjector.fadeFactor = decalOpacity;
                decalProjector.pivot = new Vector3(0, 0, decalDepth / 2f);
                // decalProjector.size = decalSize;
                // decalProjector.ma
                // decalProjector.material.pro
                // decalProjector.fadeFactor = alpha;
            }

            if (emissionMatMeshRenderer)
            {
                 // MeshRenderer = decalProjector.GetComponent<MeshRenderer>();
                            emissionMatMeshRenderer.GetPropertyBlock(materialPropertyBlock);
                            materialPropertyBlock.SetColor(emissionPropertyName,
                                new Color(Mathf.Pow(color.r, rootEmissionPower)*intensity, Mathf.Pow(color.g, rootEmissionPower)*intensity,
                                    Mathf.Pow(color.b, rootEmissionPower)*intensity));
                            emissionMatMeshRenderer.SetPropertyBlock(materialPropertyBlock);
            }
           

        }

        void Update()
        {

        }
    }
}