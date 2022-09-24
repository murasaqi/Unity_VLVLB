using System.Collections.Generic;
using UnityEngine;

namespace StageLightSupervisor
{
    public class StageLight: StageLightBase
    {
        public List<StageLightExtension> stageLightFixtures = new List<StageLightExtension>();
        
        [ContextMenu("Init")]
        public void Init()
        {

            for (int i = stageLightFixtures.Count-1; i >= 0; i--)  
            {
                DestroyImmediate(stageLightFixtures[i]);
            }
            stageLightFixtures.Clear();

            var pan = gameObject.AddComponent<LightTransformFixture>();
            pan.lightTransformType = LightTransformType.Pan;
            stageLightFixtures.Add(pan);
                
            var tilt = gameObject.AddComponent<LightTransformFixture>();
            tilt.lightTransformType = LightTransformType.Tilt;
            stageLightFixtures.Add(tilt);
            
            
            stageLightFixtures.Add(gameObject.AddComponent<LightFixture>());
        }
        public void Update()
        {
            
        }

        public override void UpdateFixture(float time)
        {
            foreach (var stageLightFixture in stageLightFixtures)
            {
                stageLightFixture.UpdateFixture(time);
            }
        }

    }
}
