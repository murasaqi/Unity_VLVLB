using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace StageLightSupervisor
{
    [CreateAssetMenu(fileName = "New StageLightSetting", menuName = "ScriptableObject/StageLightSetting")]
    public class StageLightProfile: ScriptableObject
    {
        public StageLightBaseProperty stageLightBaseProperty = new StageLightBaseProperty();
        public LightProperty lightProperty = new LightProperty();
        public PanProperty panProperty = new PanProperty();
        public TiltProperty tiltProperty = new TiltProperty();
        public DecalProperty decalProperty = new DecalProperty();
        [SerializeField]public List<StageLightProperty> stageLightProperties = new List<StageLightProperty>();
        
        [ContextMenu("Init")]
        public void Init()
        {
            stageLightProperties.Clear();
            stageLightProperties.Add(stageLightBaseProperty);
            stageLightProperties.Add(lightProperty);
            stageLightProperties.Add(panProperty);
            stageLightProperties.Add(tiltProperty);
            stageLightProperties.Add(decalProperty);
        }
        // public Dictionary<Type,StageLightData> stageLightDataDictionary = new Dictionary<Type, StageLightData>();
        
        // public Dictionary<Type,StageLightData> GetStageLightDataDictionary()
        // {
        //     Dictionary<Type, StageLightData> stageLightDataDictionary = new Dictionary<Type, StageLightData>();
        //     foreach (var data in stageLightData)
        //     {
        //         stageLightDataDictionary.Add(data.GetType(),data);
        //     }
        //
        //     return stageLightDataDictionary;
        // }

    }
}
