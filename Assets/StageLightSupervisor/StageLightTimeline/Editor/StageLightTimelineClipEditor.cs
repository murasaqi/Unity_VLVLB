using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using VLVLB;

namespace StageLightSupervisor.StageLightTimeline.Editor
{
    [CustomTimelineEditor(typeof(StageLightTimelineClip))]
    public class StageLightTimelineClipEditor:ClipEditor
    {
         [InitializeOnLoad]
        class EditorInitialize
        {
            static EditorInitialize()
            {
                playableDirector = GetMasterDirector();  
                backgroundTexture = new Texture2D(1, 1);
                syncIconTexture = Resources.Load<Texture2D>("VLVLBMaterials/icon_sync");
                backgroundTexture.SetPixel(0, 0, Color.white);
                backgroundTexture.Apply();
           
            } 
            static PlayableDirector GetMasterDirector() { return TimelineEditor.masterDirector; }
        }
        Dictionary<StageLightTimelineClip, Texture2D> _gradientTextures = new Dictionary<StageLightTimelineClip, Texture2D>();
        // Dictionary<StageLightTimelineClip, Texture2D> _beatTextures = new Dictionary<StageLightTimelineClip, Texture2D>();

        private Dictionary<StageLightTimelineClip, List<float>>
            _beatPoint = new Dictionary<StageLightTimelineClip, List<float>>();
        private static PlayableDirector playableDirector;
        private static Texture2D backgroundTexture;
        private static Texture2D syncIconTexture;
        private float min;
        private float max;
        private Dictionary<TimelineClip, AnimationCurve>
            _panCurves = new Dictionary<TimelineClip, AnimationCurve>();
        public override ClipDrawOptions GetClipOptions(TimelineClip clip)
        {
            return new ClipDrawOptions
            {
                errorText = GetErrorText(clip),
                highlightColor = GetDefaultHighlightColor(clip),
                icons = Enumerable.Empty<Texture2D>(),
                tooltip = "Tooltip"
            };
        }
        
        public override void OnClipChanged(TimelineClip clip)
        {
        
            var stageLightTimelineClip = (StageLightTimelineClip)clip.asset;
            if (stageLightTimelineClip == null)
                return;
            GetGradientTexture(clip, true);
            if (stageLightTimelineClip.referenceStageLightProfile != null)
                clip.displayName = stageLightTimelineClip.referenceStageLightProfile.name;
            

        }
        public override void OnCreate(TimelineClip clip, TrackAsset track, TimelineClip clonedFrom)
        {
            base.OnCreate(clip, track, clonedFrom);
       
        }


        public override void DrawBackground(TimelineClip clip, ClipBackgroundRegion region)
        {
            base.DrawBackground(clip, region);


            if(syncIconTexture == null)syncIconTexture = Resources.Load<Texture2D>("VLVLBMaterials/icon_sync");
            var stageLightTimelineClip = (StageLightTimelineClip) clip.asset;
            var update = stageLightTimelineClip.forceTimelineClipUpdate;
            var tex = GetGradientTexture(clip, update);
            stageLightTimelineClip.forceTimelineClipUpdate = false;
            if(stageLightTimelineClip.track == null) return;
            var colorHeight = region.position.height * stageLightTimelineClip.track.colorLineHeight;
            var beatHeight = 2f;

            if (stageLightTimelineClip.track.drawBeat)
            {
                foreach (var p in _beatPoint[stageLightTimelineClip])
                {
                    EditorGUI.DrawRect(new Rect(region.position.width * p, 0, 1, region.position.height),
                        stageLightTimelineClip.track.beatLineColor);
                }
            }

            if (tex)
                GUI.DrawTexture(
                    new Rect(region.position.x, region.position.height - colorHeight, region.position.width,
                        colorHeight), tex);




            // GUI.HorizontalSlider()
            // GUI.HorizontalSlider(region.position, 0, 0, 1);
            // var curve = EditorGUILayout.CurveField(new AnimationCurve(), Color.white, new Rect(region.position.x,region.position.y,100,40),new []{GUILayout.Height(40),GUILayout.Width(100)});

            var iconSize = 12;
            var margin = 4;
            if (stageLightTimelineClip.useProfile)
                GUI.DrawTexture(new Rect(region.position.width - iconSize - margin, margin, iconSize, iconSize),
                    syncIconTexture, ScaleMode.ScaleAndCrop,
                    true,
                    0,
                    new Color(1, 1, 1, 0.5f), 0, 0);
            
        }

        private float GetNormalizedTime(float BPM, float timeScale, float time, LoopType loopType)
        {
            var bpm = BPM * timeScale;
            var duration = 60 / bpm;
            var t = (float) time % duration;
            var inv = Mathf.CeilToInt((float) time / duration) % 2 != 0;
            var normalisedTime = t / duration;
            if (loopType == LoopType.Loop) return normalisedTime;
            return inv ? 1f - normalisedTime : normalisedTime;
        }


        
        Texture2D GetGradientTexture(TimelineClip clip, bool update = false)
        {
            Texture2D tex = Texture2D.whiteTexture;

            var customClip = clip.asset as StageLightTimelineClip;
            
            if (!customClip) return tex;

            // var props = customClip.behaviour.props;
            // if (customClip.useProfile && customClip.resolvedVlvlbClipProfile != null)
            // {
            //     props = customClip.resolvedVlvlbClipProfile.ptlProps;
            // }
            var gradient = customClip.stageLightProfile.lightProperty.lightColor.value;
            var intensity = customClip.stageLightProfile.lightProperty.lightIntensity.value;


            // var _textures = isGrad ? _gradientTextures : _beatTextures;

            if (update) 
            {
                _gradientTextures.Remove(customClip);
                _beatPoint.Remove(customClip);
            }
            else
            {
                _gradientTextures.TryGetValue(customClip, out tex);
                if (tex) return tex;
            }

            var b = (float)(clip.blendInDuration / clip.duration);
            tex = new Texture2D(64, 1);
            
            var beatPointList = new List<float>();

            var preBeat = -1f;
            for (int i = 0; i < tex.width; ++i)
            {

                var t = (float)i / tex.width;

               
                var colorT = t;
                var intensityT = t;

                var lightProfile = customClip.stageLightProfile.lightProperty;
                var stageLightBaseProfile = customClip.stageLightProfile.stageLightBaseProperty;
                
                var bpm = stageLightBaseProfile.bpm.value;
                var bpmOffset = lightProfile.bpmOverrideData.value.bpmOverride
                    ? lightProfile.bpmOverrideData.value.bpmOffset
                    : stageLightBaseProfile.bpmOffset.value;
                var bpmScale = lightProfile.bpmOverrideData.value.bpmOverride
                    ? lightProfile.bpmOverrideData.value.bpmScale
                    : stageLightBaseProfile.bpmScale.value;
                var loopType = lightProfile.bpmOverrideData.value.bpmOverride
                    ? lightProfile.bpmOverrideData.value.loopType
                    : stageLightBaseProfile.loopType.value;
                float offsetTime = (60 / bpmScale) * bpmOffset;
                if (loopType != LoopType.Fixed)
                {
                    colorT = GetNormalizedTime(bpm, bpmScale, (float)(t * clip.duration + clip.start+offsetTime), loopType);
                    intensityT = GetNormalizedTime(bpm, bpmScale, (float)(t * clip.duration + clip.start+offsetTime), loopType);
                }
                var color = gradient.Evaluate(colorT);
                if (b > 0f) color.a = Mathf.Min(colorT / b, 1f);
                var intensityValue = Mathf.Clamp(intensity.Evaluate(intensityT),0,1);
                color = new Color(color.r,
                    color.g,
                    color.b,
                    color.a*intensityValue);
                tex.SetPixel(i, 0, color);     
           
                
                
                if (loopType != LoopType.Fixed)
                {
                    var normalisedTime = GetNormalizedTime(bpm, 1, (float)(t * clip.duration + clip.start+offsetTime),
                        LoopType.Loop);
                    if (preBeat> normalisedTime)
                    {
                        beatPointList.Add(t);
                    }
                    
                    preBeat = normalisedTime;
                }
                
   
                
                    
            }

            
            
            tex.Apply();
            if (_gradientTextures.ContainsKey(customClip))
            {
                _gradientTextures[customClip] = tex;
            }
            else
            {
                _gradientTextures.Add(customClip, tex);    
            }
            
            _beatPoint.Add(customClip,beatPointList);

            return tex;
        }

        public override void GetSubTimelines(TimelineClip clip, PlayableDirector director, List<PlayableDirector> subTimelines)
        {
            base.GetSubTimelines(clip, director, subTimelines);
        }
    }
    
    
}