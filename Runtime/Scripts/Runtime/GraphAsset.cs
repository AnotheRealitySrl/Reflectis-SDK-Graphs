using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SPACS.PLG.Graphs
{

    ///////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// ScriptableObject used to contain a graph inside a project asset
    /// </summary>
    [CreateAssetMenu(fileName = "New Graph", menuName = "Graph Asset", order = 1)]
    public class GraphAsset : ScriptableObject, IContainer<IGraph>
    {
        [SerializeField]
        private GraphImplementation graph;

        ///////////////////////////////////////////////////////////////////////////
        /// <summary> The graph </summary>
        public IGraph Value
        {
            get
            {
                graph.GraphAsset = this;
                return graph;
            }

            set => graph = value as GraphImplementation;
        }


        ///////////////////////////////////////////////////////////////////////////
        [Serializable]
        private class GraphImplementation : Graph<NodeAsset, GroupImplementation, PortReferenceImplementation>
        {
            [SerializeField, HideInInspector]
            private GraphAsset graphAsset;

            ///////////////////////////////////////////////////////////////////////////
            public GraphAsset GraphAsset
            {
                get => graphAsset;
                set => graphAsset = value;
            }

            ///////////////////////////////////////////////////////////////////////////
            public override NodeAsset CreateContainer(Node node)
            {
                Type containerType = GetContainerType(node);
                NodeAsset asset = (NodeAsset)CreateInstance(containerType);
                asset.Value = node;
                asset.name = node.Name;
                return asset;
            }

#if UNITY_EDITOR

            ///////////////////////////////////////////////////////////////////////////
            public override bool AddNode(Node node)
            {
                bool validAdd = base.AddNode(node);
                if (validAdd && TryGetContainer(node, out NodeAsset nodeAsset))
                    AssetDatabase.AddObjectToAsset(nodeAsset, GraphAsset);
                return validAdd;
            }

            ///////////////////////////////////////////////////////////////////////////
            public override bool RemoveNode(Node node)
            {
                bool validRemoval = base.RemoveNode(node);
                if (validRemoval && TryGetContainer(node, out NodeAsset nodeAsset))
                    AssetDatabase.RemoveObjectFromAsset(nodeAsset);
                return validRemoval;
            }
#endif

        }

        ///////////////////////////////////////////////////////////////////////////
        [Serializable]
        private class GroupImplementation : Group<NodeAsset, GraphBehaviour> { }

        ///////////////////////////////////////////////////////////////////////////
        [Serializable]
        private class PortReferenceImplementation : PortReference<NodeAsset> { }
    }
}