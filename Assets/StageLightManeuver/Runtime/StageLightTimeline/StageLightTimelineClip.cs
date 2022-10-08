using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using StageLightManeuver;
using UnityEditor;


[Serializable]
public class StageLightTimelineClip : PlayableAsset, ITimelineClipAsset
{
    
    public StageLightProfile referenceStageLightProfile;
    [HideInInspector]public StageLightTimelineBehaviour stageLightTimelineBehaviour = new StageLightTimelineBehaviour ();
    public bool forceTimelineClipUpdate;
   
   
    public bool useProfile = false;
    
    public StageLightTimelineTrack track;
    public ClipCaps clipCaps
    {
        get { return ClipCaps.Blending; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<StageLightTimelineBehaviour>.Create (graph, stageLightTimelineBehaviour);
        stageLightTimelineBehaviour = playable.GetBehaviour ();

        var queData = stageLightTimelineBehaviour.stageLightQueData;
        if(queData.TryGet<TimeProperty>() == null)
        {
            queData.TryAdd(typeof(TimeProperty));
        }
        

        
        return playable;
    }
    
    
    [ContextMenu("Apply")]
    public void LoadProfile()
    {
        if (referenceStageLightProfile == null) return;
        var stageLightProfile = referenceStageLightProfile.Clone();
        stageLightTimelineBehaviour.stageLightQueData.stageLightProperties.Clear();
        stageLightTimelineBehaviour.stageLightQueData.stageLightProperties = stageLightProfile.stageLightProperties;

    }

    public void SaveProfile()
    {
#if UNITY_EDITOR
        Undo.RegisterCompleteObjectUndo(referenceStageLightProfile, referenceStageLightProfile.name);
        referenceStageLightProfile.stageLightProperties.Clear();
        foreach (var stageLightProperty in stageLightTimelineBehaviour.stageLightQueData.stageLightProperties)
        {
            var type = stageLightProperty.GetType();
            referenceStageLightProfile.stageLightProperties.Add(Activator.CreateInstance(type, BindingFlags.CreateInstance, null, new object[]{stageLightProperty}, null)
                as StageLightProperty);
        }
        
        referenceStageLightProfile.ApplyListToProperties();
        // referenceStageLightProfile.Serialize();
        // Set dirty flag
        EditorUtility.SetDirty(referenceStageLightProfile);
        AssetDatabase.SaveAssets();
#endif
    }
}
