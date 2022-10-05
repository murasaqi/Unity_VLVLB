using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace StageLightSupervisor
{
    [CreateAssetMenu(fileName = "New StageLightSetting", menuName = "ScriptableObject/StageLightSetting")]
    public class StageLightProfile: ScriptableObject
    {
       
        [SerializeField]public List<StageLightProperty> stageLightProperties = new List<StageLightProperty>();
        
        [ContextMenu("Init")]
        public void Init()
        {
            stageLightProperties.Clear();
            stageLightProperties.Add(new StageLightBaseProperty());
            stageLightProperties.Add(new LightProperty());
            stageLightProperties.Add(new PanProperty());
            stageLightProperties.Add(new TiltProperty());
            stageLightProperties.Add(new DecalProperty());
            stageLightProperties.Add(new GoboProperty());
        }

        public StageLightProperty TryGet<T>() where T : StageLightProperty
        {
            var result = stageLightProperties.Find(x => x.GetType() == typeof(T));
            return result;
        }
        public StageLightProfile()
        {
            Init();
        }
        public StageLightProfile Clone()
        {
            var result = CreateInstance<StageLightProfile>();
            result.stageLightProperties = new List<StageLightProperty>();
            foreach (var stageLightProperty in stageLightProperties)
            {
                var type = stageLightProperty.GetType();
                Debug.Log(type);
                result.stageLightProperties.Add(Activator.CreateInstance(type, BindingFlags.CreateInstance, null, new object[]{stageLightProperty}, null)
                     as StageLightProperty);
            }
            return result;
        }
        
        // public StageLightProfile(StageLightProfile profile)
        // {
        //     stageLightProperties = new List<StageLightProperty>();
        //     foreach (var stageLightProperty in profile.stageLightProperties)
        //     {
        //         stageLightProperties.Add(stageLightProperty.Clone());
        //     }
        // }
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
