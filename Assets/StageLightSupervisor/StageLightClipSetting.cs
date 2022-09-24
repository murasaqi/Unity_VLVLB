using UnityEngine;

namespace StageLightSupervisor
{
    [CreateAssetMenu(fileName = "New StageLightClipSetting", menuName = "StageLightClipSetting")]
    public class StageLightClipSetting: ScriptableObject
    {
        public StageLightExtensionProperty extensionProperty;
        public LightProperty lightProperty;
        public RollProperty rollPropertyPan;
        public RollProperty rollPropertyTilt;
        public DecalProperty decalProperty;
        
    }
}
