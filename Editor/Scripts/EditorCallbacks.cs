using UnityEditor;
using UnityEditor.Callbacks;

using UnityEngine;

namespace Reflectis.PLG.Graphs.Editor
{
    ///////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Static callbacks called by the Unity Editor
    /// </summary>
    public static class EditorCallbacks
    {

        ///////////////////////////////////////////////////////////////////////////
        /// Callback called when any asset is opened
        [OnOpenAsset(0)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            // Is the asset an instance of GraphAsset?
            Object asset = EditorUtility.InstanceIDToObject(instanceID);
            if (asset is GraphAsset graphAsset)
            {
                // Open it in the GraphWindow
                GraphWindow.OpenGraphViewWindow(graphAsset);
                return true;
            }
            return false;
        }
    }
}