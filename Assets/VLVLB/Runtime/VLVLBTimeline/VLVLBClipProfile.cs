using System.Collections.Generic;
using UnityEngine;

namespace VLVLB
{

    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PTLPropsObject", order = 1)]
    public class VLVLBClipProfile : ScriptableObject
    {
        public VLVLBClipProfile(PTLProps props)
        {
            this.ptlProps = new PTLProps(props);
            // copy PTLProps properties to this ptlsProps properties
        }
        public PTLProps ptlProps = new PTLProps();
    }

}