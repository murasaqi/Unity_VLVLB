using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StageLightSupervisor
{

    public interface IStageLight
    {
        public int Index { get; set; }
        public List<StageLight> StageLightGroup { get; set; }
        public void UpdateFixture(float time);
        public void AddStageLightInChild(){}
        public void AddQue(StageLightData stageLightData, float weight){}
    }
}
