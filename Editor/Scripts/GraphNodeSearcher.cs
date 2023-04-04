using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.Experimental.GraphView;

namespace SPACS.PLG.Graphs.Editor
{
    ///////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// This object provides data needed by the graph system in order to
    /// populate the node creation menu
    /// </summary>
    public class GraphNodeSearcher : ScriptableObject, ISearchWindowProvider
    {
        private EditorWindow window;
        private GraphPanel graphPanel;
        private Texture2D icon;

        ///////////////////////////////////////////////////////////////////////////
        public void Configure(EditorWindow window, GraphPanel graphPanel)
        {
            this.window = window;
            this.graphPanel = graphPanel;

            // Empty icon
            icon = new Texture2D(1, 1);
            icon.SetPixel(0, 0, new Color(0, 0, 0, 0));
            icon.Apply();
        }

        ///////////////////////////////////////////////////////////////////////////
        /// Creates the menu
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            // Create a SearchTreeEntry with a default group entry
            var tree = new List<SearchTreeEntry>
            {
                new SearchTreeGroupEntry(new GUIContent("New Node"), 0)
            };

            // Loop over all types deriving from the Node class and them to the tree
            foreach (System.Type nodeType in TypeCache.GetTypesDerivedFrom<Node>())
            {
                string typeName = ObjectNames.NicifyVariableName(nodeType.Name);
                tree.Add(new SearchTreeEntry(new GUIContent(typeName, icon))
                {
                    level = 1,
                    userData = nodeType
                });
            }
            return tree;
        }

        ///////////////////////////////////////////////////////////////////////////
        /// Called when an entry is selected
        public bool OnSelectEntry(SearchTreeEntry entry, SearchWindowContext context)
        {
            if (entry.userData is System.Type nodeType)
            {
                // Generate a node of the selected type
                Node node = Node.GenerateNode(nodeType);
                if (node != null)
                {
                    // By default, use the type name as node name
                    node.Name = ObjectNames.NicifyVariableName(nodeType.Name);

                    // Use the mouse position as first node position
                    Vector2 mousePosition = context.screenMousePosition - window.position.position;
                    mousePosition = window.rootVisualElement.ChangeCoordinatesTo(window.rootVisualElement.parent, mousePosition);
                    node.Position = graphPanel.contentViewContainer.WorldToLocal(mousePosition);

                    // Add the node to the graph
                    graphPanel.AddNode(node);
                    return true;
                }
            }

            return false;
        }

        ///////////////////////////////////////////////////////////////////////////
        void OnDestroy()
        {
            if (icon != null)
            {
                DestroyImmediate(icon);
                icon = null;
            }
        }
    }
}