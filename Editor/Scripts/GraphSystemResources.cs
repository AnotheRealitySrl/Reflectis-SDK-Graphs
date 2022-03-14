using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SPACS.Graphs.Editor
{
    ///////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Utility static class used to get resources used in the graph editor
    /// </summary>
    public static class GraphSystemResources
    {
        private static readonly Dictionary<string, object> resourcesDict = new Dictionary<string, object>();


        ///////////////////////////////////////////////////////////////////////////
        /// <summary>Loads a StyleSheet object from a USS file name</summary>
        /// <param name="name">The name of the USS file in the Stylesheets folder</param>
        /// <returns>The StyleSheet object</returns>
        public static StyleSheet GetStylesheet(string name) => Get<StyleSheet>($"Stylesheets/{name}");


        ///////////////////////////////////////////////////////////////////////////
        /// <summary>Loads a VisualTreeAsset object from a UXML file name</summary>
        /// <param name="name">The name of the UXML file in the Templates folder</param>
        /// <returns>The VisualTreeAsset object</returns>
        public static VisualTreeAsset GetTemplate(string name) => Get<VisualTreeAsset>($"Templates/{name}");


        ///////////////////////////////////////////////////////////////////////////
        /// <summary>Loads a Texture2D object from a texture file name</summary>
        /// <param name="name">The name of the texture file in the Textures folder</param>
        /// <returns>The Texture2D object</returns>
        public static Texture2D GetTexture(string name) => Get<Texture2D>($"Textures/{name}");


        ///////////////////////////////////////////////////////////////////////////
        /// <summary>Loads an object from a resource file name</summary>
        /// <typeparam name="T">The type of the object</typeparam>
        /// <param name="path">The full path of the file in a Resources folder</param>
        /// <returns>The loaded object</returns>
        public static T Get<T>(string path) where T : Object
        {
            if (resourcesDict.TryGetValue(path, out object obj))
            {
                return obj as T;
            }
            else
            {
                T styleSheet = Resources.Load<T>(path);
                resourcesDict[path] = styleSheet;
                return styleSheet;
            }
        }
    }
}