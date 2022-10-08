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
    public bool syncReferenceProfile = false;
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

        if (syncReferenceProfile)
        {
            InitSyncData();
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
                as SlmProperty);
        }
        referenceStageLightProfile.isUpdateGuiFlag = true;
        EditorUtility.SetDirty(referenceStageLightProfile);
        AssetDatabase.SaveAssets();
#endif
    }

    public void InitSyncData()
    {
        if (syncReferenceProfile)
        {
            if(referenceStageLightProfile != null)
            {
                
                foreach (var stageLightProperty in referenceStageLightProfile.stageLightProperties)
                {
                    stageLightProperty.ToggleOverride(false);
                    stageLightProperty.propertyOverride = true;
                }
                stageLightTimelineBehaviour.stageLightQueData.stageLightProperties =
                    referenceStageLightProfile.stageLightProperties;    
            }
        }
        else
        {
            var cloneProperties = new List<SlmProperty>();
            foreach (var stageLightProperty in stageLightTimelineBehaviour.stageLightQueData.stageLightProperties)
            {
                var type = stageLightProperty.GetType();
                cloneProperties.Add(Activator.CreateInstance(type, BindingFlags.CreateInstance, null, new object[]{stageLightProperty}, null)
                    as SlmProperty);
            }

            stageLightTimelineBehaviour.stageLightQueData.stageLightProperties = cloneProperties;
        }
    }
}
