using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StageLightSupervisor
{
    [ExecuteAlways]
    public class StageLight: MonoBehaviour,IStageLight
    {
        public List<StageLightExtension> stageLightFixtures = new List<StageLightExtension>();
        [SerializeField]private int index = 0;
        public int Index { get => index; set => index = value; }
        [SerializeField]private List<StageLight> stageLightChild = new List<StageLight>();

        public List<StageLight> StageLightChild
        {
            get => stageLightChild;
            set=> stageLightChild = value;
        }

        public void Update()
        {
        }

        public void AddQue(StageLightProfile stageLightProfile, float weight)
        {
            var que = new StageLightDataQueue()
            {
                stageLightProfile = stageLightProfile,
                weight = weight
            };
            foreach (var stageLightFixture in stageLightFixtures)
            {
                stageLightFixture.stageLightDataQueue.Enqueue(que);
            }

            foreach (var stageLight in StageLightChild)
            {
                stageLight.AddQue(stageLightProfile,weight);
            }
        }

        public void UpdateFixture(float time)
        {
            var i = 0;
            foreach (var stageLight in StageLightChild)
            {
                // Debug.Log(stageLight.name);
                stageLight.Index = i;
                stageLight.UpdateFixture(time);
                i++;
            }
            foreach (var stageLightFixture in stageLightFixtures)
            {
                stageLightFixture.UpdateFixture(time);
                stageLightFixture.Index = index;
            }
        }
        
        [ContextMenu("Get StageLights in Children")]
        public void AddStageLightInChild()
        {
            StageLightChild.Clear();
            StageLightChild = GetComponentsInChildren<StageLight>().ToList();
            
        }

    }
}
