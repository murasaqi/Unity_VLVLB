using UnityEngine;

namespace StageLightSupervisor
{
    public class GoboProperty:StageLightAdditionalProperty
    {
        public StageLightValue<Texture2D> goboTexture;
        public StageLightValue<string> goboPropertyName;

        public GoboProperty()
        {
            bpmOverrideData = new StageLightValue<BpmOverrideData>(){value = new BpmOverrideData()};
            goboTexture = new StageLightValue<Texture2D>(){value = null};
            goboPropertyName = new StageLightValue<string>(){value = "_GoboTexture"};
        }
        
        public GoboProperty(GoboProperty other)
        {
            bpmOverrideData = other.bpmOverrideData;
            goboTexture = other.goboTexture;
            goboPropertyName = other.goboPropertyName;
        }
    }
}