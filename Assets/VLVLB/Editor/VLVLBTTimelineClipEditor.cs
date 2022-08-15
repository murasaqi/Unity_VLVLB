
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace VLVLB
{
    // internal enum TextureType
    // {
    //     Gradient,
    //     Pan,
    //     Tilt
    // }

        [CustomTimelineEditor(typeof(VLVLBTimelineClip))]
    public class VLVLBTTimelineClipEditor: ClipEditor
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
        Dictionary<VLVLBTimelineClip, Texture2D> _gradientTextures = new Dictionary<VLVLBTimelineClip, Texture2D>();
        // Dictionary<VLVLBTimelineClip, Texture2D> _beatTextures = new Dictionary<VLVLBTimelineClip, Texture2D>();

        private Dictionary<VLVLBTimelineClip, List<float>>
            _beatPoint = new Dictionary<VLVLBTimelineClip, List<float>>();
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
        
            var vlvlbTimelineClip = (VLVLBTimelineClip)clip.asset;
            if (vlvlbTimelineClip == null)
                return;
            GetGradientTexture(clip, true);
            GetPanCurve(clip, true);
            if (vlvlbTimelineClip.resolvedVlvlbClipProfile != null)
                clip.displayName = vlvlbTimelineClip.resolvedVlvlbClipProfile.name;
            

        }
        public override void OnCreate(TimelineClip clip, TrackAsset track, TimelineClip clonedFrom)
        {
            base.OnCreate(clip, track, clonedFrom);
       
        }


        public override void DrawBackground(TimelineClip clip, ClipBackgroundRegion region)
        {
            base.DrawBackground(clip, region);


            var vlvlbTimelineClip = (VLVLBTimelineClip) clip.asset;
            var update = vlvlbTimelineClip.forceTimelineClipUpdate;
            var tex = GetGradientTexture(clip, update);
            vlvlbTimelineClip.forceTimelineClipUpdate = false;
            var colorHeight = region.position.height * vlvlbTimelineClip.track.colorLineHeight;
            var beatHeight = 2f;

            if (vlvlbTimelineClip.track.drawBeat)
            {
                foreach (var p in _beatPoint[vlvlbTimelineClip])
                {
                    EditorGUI.DrawRect(new Rect(region.position.width * p, 0, 1, region.position.height),
                        vlvlbTimelineClip.track.beatLineColor);
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
            if (vlvlbTimelineClip.useProfile)
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


        AnimationCurve GetPanCurve(TimelineClip clip, bool update = false)
        {

            AnimationCurve animationCurve = new AnimationCurve();
            if (update)
            {
                _panCurves.Remove(clip);
            }
            else
            {
                _panCurves.TryGetValue(clip, out animationCurve);
                if(animationCurve != null) return animationCurve;
            }

            
            
            var vlvlbTimelineClip = (VLVLBTimelineClip) clip.asset;
            animationCurve = vlvlbTimelineClip.behaviour.props.pan;
            _panCurves.Add(clip, animationCurve);

            return animationCurve;

        }
        
        Texture2D GetGradientTexture(TimelineClip clip, bool update = false)
        {
            Texture2D tex = Texture2D.whiteTexture;

            var customClip = clip.asset as VLVLBTimelineClip;
            
            if (!customClip) return tex;

            var props = customClip.behaviour.props;
            if (customClip.useProfile && customClip.resolvedVlvlbClipProfile != null)
            {
                props = customClip.resolvedVlvlbClipProfile.ptlProps;
            }
            var gradient = props.lightColor;
            var intensity = props.intensity;


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
                
                if (customClip.behaviour.props.loopType != LoopType.Fixed)
                {
                    colorT = GetNormalizedTime(props.BPM, props.timeScaleColor, (float)(t * clip.duration + clip.start +customClip.offsetClipTime),
                        props.loopType);
                    intensityT = GetNormalizedTime(props.BPM, props.timeScaleIntensity, (float)(t * clip.duration + clip.start+customClip.offsetClipTime),
                        props.loopType);
                }
                var color = gradient.Evaluate(colorT);
                if (b > 0f) color.a = Mathf.Min(colorT / b, 1f);
                var intensityValue = Mathf.Clamp(intensity.Evaluate(intensityT),0,1);
                color = new Color(color.r,
                    color.g,
                    color.b,
                    color.a*intensityValue);
                tex.SetPixel(i, 0, color);     
           
                
                
                if (props.loopType != LoopType.Fixed)
                {
                    var normalisedTime = GetNormalizedTime(props.BPM, 1, (float)(t * clip.duration + clip.start+customClip.offsetClipTime),
                        LoopType.Loop);
                    if (preBeat> normalisedTime)
                    {
                        beatPointList.Add(t);
                    }
                    
                    preBeat = normalisedTime;
                }
                
   
                
                    
            }

            
            
            tex.Apply();
            _gradientTextures.Add(customClip, tex);
            _beatPoint.Add(customClip,beatPointList);

            return tex;
        }

        public override void GetSubTimelines(TimelineClip clip, PlayableDirector director, List<PlayableDirector> subTimelines)
        {
            base.GetSubTimelines(clip, director, subTimelines);
        }
    }
    
}