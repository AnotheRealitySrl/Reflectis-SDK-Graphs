using System;
using UnityEngine;

namespace SPACS.PLG.Graphs
{

    ///////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// MonoBehaviour used to contain a graph inside a scene component
    /// </summary>
    public class GraphBehaviour : MonoBehaviour, IContainer<IGraph>
    {
        [SerializeField]
        private GraphImplementation graph;

        ///////////////////////////////////////////////////////////////////////////
        /// <summary> The graph </summary>
        public IGraph Value
        {
            get
            {
                graph.GraphBehaviour = this;
                return graph;
            }

            set => graph = value as GraphImplementation;
        }

        ///////////////////////////////////////////////////////////////////////////
        [Serializable]
        private class GraphImplementation : Graph<NodeBehaviour, GroupImplementation, PortReferenceImplementation>
        {

            [SerializeField, HideInInspector]
            private GraphBehaviour graphBehaviour;

            ///////////////////////////////////////////////////////////////////////////
            public GraphBehaviour GraphBehaviour
            {
                get => graphBehaviour;
                set => graphBehaviour = value;
            }

            ///////////////////////////////////////////////////////////////////////////
            public override NodeBehaviour CreateContainer(Node node)
            {
                Type containerType = GetContainerType(node);
                if (containerType == null)
                    return null;

                GameObject go = new GameObject(node.Name);
                NodeBehaviour bhv = go.AddComponent(containerType) as NodeBehaviour;
                bhv.Value = node;
                go.transform.SetParent(GraphBehaviour.transform);
                return bhv;
            }

            ///////////////////////////////////////////////////////////////////////////
            public override bool RenameNode(Node node, string newName)
            {
                bool validRename = base.RenameNode(node, newName);
                if (validRename && TryGetContainer(node, out NodeBehaviour nodeBehaviour))
                    nodeBehaviour.gameObject.name = newName;
                return validRename;
            }

            ///////////////////////////////////////////////////////////////////////////
            public override bool RemoveNode(Node node)
            {
                TryGetContainer(node, out NodeBehaviour nodeBehaviour);
                bool validRemoval = base.RemoveNode(node);
                if (validRemoval && nodeBehaviour != null)
                    DestroyImmediate(nodeBehaviour.gameObject);
                return validRemoval;
            }
        }

        ///////////////////////////////////////////////////////////////////////////
        [Serializable]
        private class PortReferenceImplementation : PortReference<NodeBehaviour> { }

        ///////////////////////////////////////////////////////////////////////////
        [Serializable]
        private class GroupImplementation : Group<NodeBehaviour, GraphBehaviour> { }
    }
}
