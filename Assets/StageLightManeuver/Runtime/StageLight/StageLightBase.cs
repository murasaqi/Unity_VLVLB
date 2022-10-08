using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StageLightManeuver
{

    public interface IStageLight
    {
        public int Index { get; set; }
        public List<StageLight> StageLightChild { get; set; }
        public void UpdateFixture(float time);
        public void AddStageLightInChild(){}
        public void AddQue(StageLightData stageLightData, float weight){}
    }
}
