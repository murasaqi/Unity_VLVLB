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
        public List<StageLightData> stageLightData = new List<StageLightData>();

        

        [ContextMenu("Init")]
        public void Init()
        {
            stageLightData.Clear();
            stageLightData.Add(stageLightBaseProperty);
            stageLightData.Add(lightProperty);
            stageLightData.Add(panProperty);
            stageLightData.Add(tiltProperty);
            stageLightData.Add(decalProperty);
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
