using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MoreMountains.Tools
{
    /// <summary>
    /// A class used to describe tab contents
    /// </summary>
    public class MMDebugMenuTabContents : MonoBehaviour
    {
        /// the index of the tab, setup by MMDebugMenu
        public int Index = 0;
        /// the parent of the tab, setup by MMDebugMenu
        public Transform Parent;
    }
}
