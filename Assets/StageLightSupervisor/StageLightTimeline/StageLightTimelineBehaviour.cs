using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using StageLightSupervisor;

[Serializable]
public class StageLightTimelineBehaviour : PlayableBehaviour
{
    public StageLightBaseProperty stageLightBaseProperty;
    public LightProperty lightProperty;
    public RollProperty panProperty;
    public RollProperty tiltProperty;
    public DecalProperty decalProperty;

    public override void OnPlayableCreate (Playable playable)
    {
        
    }
}
