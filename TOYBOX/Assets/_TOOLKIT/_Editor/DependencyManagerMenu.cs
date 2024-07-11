using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LLGD.EditorSystems;
using UnityEditor;


namespace Toolkit.DependencyResolution
{
    public class DependencyManagerMenu : EditorMenu
    {
        [MenuItem("LLGD/Dependency Manager/Reset Dependencies")]
        public static void ResetDependencies()
        {
            DependencyManager.Reset();
        }
    }
}
