using UnityEditor;
using UnityEngine.UIElements;

namespace SPACS.Graphs.Editor
{
    ///////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Custom inspector editor for GraphBehaviour
    /// </summary>
    [CustomEditor(typeof(GraphBehaviour)), CanEditMultipleObjects]
    public class GraphBehaviourEditor : UnityEditor.Editor
    {

        ///////////////////////////////////////////////////////////////////////////
        public override VisualElement CreateInspectorGUI()
        {
            // Retrieve the UXML visual tree template
            VisualTreeAsset template = GraphSystemResources.GetTemplate("GraphBehaviour");
            TemplateContainer rootElement = template.CloneTree();

            // Draw the default inspector
            VisualElement defaultInspector = rootElement.Q<VisualElement>("defaultInspector");
            defaultInspector.Add(new IMGUIContainer(() => DrawDefaultInspector()));

            // Add the "Open Graph" button
            Button button = rootElement.Q<Button>("openGraphButton");
            button.clicked += () =>
            {
                GraphBehaviour graphBehaviour = (GraphBehaviour)target;
                GraphWindow.OpenGraphViewWindow(graphBehaviour);
            };

            return rootElement;
        }
    }
}