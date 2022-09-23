using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StageLightSupervisor
{
    public class StageLightUniverse: StageLightBase
    {
        public List<StageLight> stageLights = new List<StageLight>();
        
        
        [ContextMenu("Get StageLights in Children")]
        public void AddStageLightInChild()
        {
            stageLights.Clear();
            stageLights = GetComponentsInChildren<StageLight>().ToList();
            
        }


        public override void UpdateFixture(float time)
        {
            foreach (StageLight stageLight in stageLights)
            {
                stageLight.UpdateFixture(time);
            }
        }
    }
}
