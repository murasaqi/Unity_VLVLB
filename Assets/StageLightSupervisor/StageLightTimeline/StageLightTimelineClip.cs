using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using StageLightSupervisor;
using UnityEditor;


[Serializable]
public class StageLightTimelineClip : PlayableAsset, ITimelineClipAsset
{
    
    public StageLightProfile referenceStageLightProfile;
    [HideInInspector] public StageLightProfile stageLightProfile;
    [HideInInspector]public StageLightTimelineBehaviour template = new StageLightTimelineBehaviour ();
    public bool forceTimelineClipUpdate;

    public StageLightTimelineTrack track;

    public bool useProfile = false;
    // public StageLight trackBinding;
    public ClipCaps clipCaps
    {
        get { return ClipCaps.Blending; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        
        var playable = ScriptPlayable<StageLightTimelineBehaviour>.Create (graph, template);
        StageLightTimelineBehaviour clone = playable.GetBehaviour ();
        if (stageLightProfile == null)
        {
            stageLightProfile = ScriptableObject.CreateInstance<StageLightProfile>();
            stageLightProfile.Init();
        }
        
        // ApplySetting();
        return playable;
    }
    
    
    [ContextMenu("Apply")]
    public void LoadProfile()
    {
        if (referenceStageLightProfile == null) return;
        stageLightProfile = referenceStageLightProfile.Clone();
        
    }

    public void SaveProfile()
    {
#if UNITY_EDITOR
        Undo.RegisterCompleteObjectUndo(referenceStageLightProfile, referenceStageLightProfile.name);
        referenceStageLightProfile.stageLightProperties.Clear();
        foreach (var stageLightProperty in stageLightProfile.stageLightProperties)
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
