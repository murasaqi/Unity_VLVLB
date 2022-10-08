using UnityEngine;

namespace StageLightManeuver
{
    [ExecuteAlways]
    public class MovingStageLight:StageLight
    {
        [ContextMenu("Init")]
        public void Init()
        {

            for (int i = stageLightFixtures.Count-1; i >= 0; i--)  
            {
                DestroyImmediate(stageLightFixtures[i]);
            }
            stageLightFixtures.Clear();

            var pan = gameObject.AddComponent<LightPanFixture>();
            stageLightFixtures.Add(pan);
            var tilt = gameObject.AddComponent<LightTiltFixture>();
            stageLightFixtures.Add(tilt);
            stageLightFixtures.Add(gameObject.AddComponent<LightFixture>());
            stageLightFixtures.Add(gameObject.AddComponent<MaterialColorFixture>());
            stageLightFixtures.Add(gameObject.AddComponent<DecalFixture>());
            stageLightFixtures.Add(gameObject.AddComponent<GoboFixture>());
        }
    }
}