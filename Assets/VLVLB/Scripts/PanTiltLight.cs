using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using VLB;

namespace VLVLB
{
    
    public static class VLVLBUtils
    {
        public static Color32 DecomposeHdrColor(Color linearColorHdr)
        {
            byte  k_MaxByteForOverexposedColor = 191;
            var color = new Color32();
            var exposure = 0f;
            var maxColorComponent = linearColorHdr.maxColorComponent;
            // replicate Photoshops's decomposition behaviour
            if (maxColorComponent == 0f || maxColorComponent <= 1f && maxColorComponent >= 1 / 255f)
            {
                exposure = 0f;
                color.r = (byte)Mathf.RoundToInt(linearColorHdr.r * 255f);
                color.g = (byte)Mathf.RoundToInt(linearColorHdr.g * 255f);
                color.b = (byte)Mathf.RoundToInt(linearColorHdr.b * 255f);
                color.a = (byte)Mathf.RoundToInt(linearColorHdr.a * 255f);
            }
            else
            {
                // calibrate exposure to the max float color component
                var scaleFactor = k_MaxByteForOverexposedColor / maxColorComponent;
                exposure = Mathf.Log(255f / scaleFactor) / Mathf.Log(2f);
                // maintain maximal integrity of byte values to prevent off-by-one errors when scaling up a color one component at a time
                color.r = Math.Min(k_MaxByteForOverexposedColor, (byte)Mathf.CeilToInt(scaleFactor * linearColorHdr.r));
                color.g = Math.Min(k_MaxByteForOverexposedColor, (byte)Mathf.CeilToInt(scaleFactor * linearColorHdr.g));
                color.b = Math.Min(k_MaxByteForOverexposedColor, (byte)Mathf.CeilToInt(scaleFactor * linearColorHdr.b));
                color.a = Math.Min(k_MaxByteForOverexposedColor, (byte)Mathf.CeilToInt(scaleFactor * linearColorHdr.a));
            }

            return color;
        }
    }

    public class PanTiltLight : MonoBehaviour
    {
        public int elementIndex = 0;
        public Transform pan;
        public Transform tilt;
        public List<VolumetricLightBeam> volumetricLightBeams = new List<VolumetricLightBeam>();
        public List<Light> lights = new List<Light>();
        public MeshRenderer emissionMatMeshRenderer;
        public DecalProjector decalProjector;
        private MaterialPropertyBlock materialPropertyBlock;
        public string emissionPropertyName = "_EmissionColor";
        // public float materialEmissionPower = 1.2f;
        public Material decalMaterial;
        private Material instancedDecalMaterial = null;
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

            DestroyImmediate(instancedDecalMaterial);

            foreach (var i in instanced)
            {
                DestroyImmediate(i);
            }
        }

        public void InitDecal()
        {
            if(instancedDecalMaterial != null) DestroyImmediate(instancedDecalMaterial);
            if (decalProjector != null)
            {
                instancedDecalMaterial = Material.Instantiate(decalMaterial);
                decalProjector.material = instancedDecalMaterial;     
            }
           
        }
        



        public void UpdatePtl(int universe, int index, List<PTLProps> propsCue)
        {

          
            if (materialPropertyBlock == null) materialPropertyBlock = new MaterialPropertyBlock();

            float p = 0f, t = 0f, intensity = 0f, spotAngle = 0f, rangeLimit = 0f, truncatedRadius = 0f, decalOpacity = 0f,decalDepth = 0f, innerSpotAngle =0f, outerSpotAngle=0f;
            var lightColor = new Color();
            var materialEmissionColor = new Color();
            var decalSize = new Vector2(0, 0);
            var rootEmissionPower = 0f;
            var count = 0;
            foreach (var cue in propsCue)
            {
                var offsetTime = cue.GetNormalizedTime(universe,index, 1);

                if (cue.useManualTransform)
                {
                    p += cue.manualTransforms[elementIndex].pan * cue.weight;
                    t += cue.manualTransforms[elementIndex].tilt * cue.weight;
                }
                else
                {
                    p += cue.pan.Evaluate(cue.ignoreOffsetPan
                        ? cue.GetNormalizedTime(universe,0, cue.timeScalePan)
                        : cue.GetNormalizedTime(universe,index, cue.timeScalePan)) * cue.weight;
                    t += cue.tilt.Evaluate(cue.ignoreOffsetTilt
                        ? cue.GetNormalizedTime(universe,0, cue.timeScaleTilt)
                        : cue.GetNormalizedTime(universe,index, cue.timeScaleTilt)) * cue.weight;
                }
               
                
                intensity += cue.intensity.Evaluate(cue.ignoreOffsetIntensity
                    ? cue.GetNormalizedTime(universe,0, cue.timeScaleIntensity)
                    : cue.GetNormalizedTime(universe,index, cue.timeScaleIntensity)) * cue.weight;
                // Debug.Log(color);
                // Debug.Log(color);
                materialEmissionColor += cue.lightColor.Evaluate(cue.ignoreOffsetColor
                    ? cue.GetNormalizedTime(universe,0, cue.timeScaleColor)
                    : cue.GetNormalizedTime(universe,index, cue.timeScaleColor)) * cue.weight;
                var exposure = 0f;
                lightColor = VLVLBUtils.DecomposeHdrColor(materialEmissionColor);
                spotAngle += cue.spotAngle * cue.weight;
                rangeLimit += cue.rangeLimit * cue.weight;
                truncatedRadius += cue.truncatedRadius * cue.weight;
                decalOpacity += cue.decalOpacity * cue.weight;
                decalDepth += cue.decalDepth * cue.weight;
                decalSize += cue.decalSize * cue.weight;
                innerSpotAngle += cue.innerSpotAngle * cue.weight;
                // rootEmissionPower += cue.rootEmissionPower * cue.weight;
                // Debug.Log($"{ cue.decalSize},{decalSize}");
                count++;
            }

            if (pan != null)
            {
                pan.localEulerAngles = Vector3.up * p;
            }
            if (tilt != null) tilt.localEulerAngles = Vector3.left * t;
            if (volumetricLightBeams != null)
            {
                foreach (var volumetricLightBeam in volumetricLightBeams)
                {
                    volumetricLightBeam.intensityGlobal = intensity;
                    volumetricLightBeam.color = lightColor;
                    volumetricLightBeam.spotAngleFromLight = false;
                    volumetricLightBeam.fallOffEnd = rangeLimit;
                    volumetricLightBeam.coneRadiusStart = truncatedRadius;
                    volumetricLightBeam.spotAngle = spotAngle;    
                }
            }
            
            if(lights != null)
            {
                foreach (var light in lights)
                {
                    light.intensity = intensity;
                    light.color = lightColor;
                    light.spotAngle = spotAngle;
                    light.range = rangeLimit;
                    light.innerSpotAngle = innerSpotAngle;
                    // light.spotAngle = outerSpotAngle;
                }
            }
            var emissionIntensity = Mathf.Clamp(intensity, 0,1);
            if (decalProjector != null)
            {
                if (instancedDecalMaterial == null) InitDecal();
                decalProjector.material.SetFloat("_Alpha",materialEmissionColor.a);
                decalProjector.material.SetColor("_Color",materialEmissionColor);
                // decalProjector.material.SetFloat("_Intensity",intensity);
                decalProjector.size = new Vector3(decalSize.x, decalSize.y, decalDepth);
                decalProjector.fadeFactor =emissionIntensity* decalOpacity*Vector3.Distance(new Vector3(materialEmissionColor.r,materialEmissionColor.g,materialEmissionColor.b),Vector3.zero);
                decalProjector.pivot = new Vector3(0, 0, decalDepth / 2f);
            }

            if (emissionMatMeshRenderer)
            {
                emissionMatMeshRenderer.GetPropertyBlock(materialPropertyBlock);
                materialPropertyBlock.SetColor(emissionPropertyName,
                    new Color(materialEmissionColor.r*emissionIntensity, materialEmissionColor.g*emissionIntensity,
                        materialEmissionColor.b*emissionIntensity));
                emissionMatMeshRenderer.SetPropertyBlock(materialPropertyBlock);
            }
        }

        void Update()
        {

        }
    }
}