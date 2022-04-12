
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace VLVLB
{

#if UNITY_EDITOR
    using UnityEditor;
    using UnityEditor.UIElements;
    [CanEditMultipleObjects]
    [CustomEditor(typeof(PanTiltLightGroup), true)]
    public class PTLUniverseEditor : Editor
    {

        public override VisualElement CreateInspectorGUI()
        {
            // Inspector拡張の場合、VisualElementはnewする   
            var root = new VisualElement();
            var target = serializedObject.targetObject as PanTiltLightGroup;
            root.Bind(serializedObject);


            var visualTree = Resources.Load<VisualTreeAsset>("PTLUniverseUI");
            visualTree.CloneTree(root);

            var ptlButton = root.Q<Button>("SearchPtlButton");
            ptlButton.clicked += target.SearchPTLInChild;
            
            var universeButton = root.Q<Button>("SearchUniverseButton");
            universeButton.clicked += target.SearchUniverseInChild;
            return root;
        }
    }
#endif

    
    public class PanTiltLightGroup : MonoBehaviour
    {
        // Start is called before the first frame updat

        public bool searchOnAwake = false;
        // private List<PTLProps> cue;
        public List<PanTiltLight> ptls = new List<PanTiltLight>();
        public List<PanTiltLightGroup> ptlUniverses = new List<PanTiltLightGroup>();
        void Start()
        {
            if(searchOnAwake) SearchPTLInChild();
        }

        private void OnEnable()
        {
            SearchPTLInChild();
        }

        public void SearchPTLInChild()
        {
            ptls.Clear();
            foreach (Transform child in transform)
            {
                var ptl = child.gameObject.GetComponent<PanTiltLight>();
                if (ptl != null) ptls.Add(ptl);
            }
        }
        
        public void SearchUniverseInChild()
        {
            
            ptlUniverses.Clear();
            
            foreach (Transform child in transform)
            {
                var universe = child.gameObject.GetComponent<PanTiltLightGroup>();
                if (universe != null) ptlUniverses.Add(universe);
            }
        }

        public void AddCue(PTLProps ptlProps)
        {
            // if (cue == null) cue = new List<PTLProps>();
            // cue.Add(ptlProps);
        }

        public void UpdatePTL(List<PTLProps> ptlPropsCues)
        {


            var count = 0;
            foreach (var ptl in ptls)
            {


                ptl.UpdatePtl(0,count, ptlPropsCues);
                count++;
            }

            var univerCount = 0;
            foreach (var unv in ptlUniverses)
            {


                count = 0;
                foreach (var ptl in unv.ptls)
                {


                    ptl.UpdatePtl(univerCount,count, ptlPropsCues);
                    count++;
                }
                univerCount++;
            }

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}