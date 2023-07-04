using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Reflectis.PLG.Graphs
{
    ///////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Abstract class used as base implementation of the IGroup interface
    /// </summary>
    [Serializable]
    public abstract class Group<TNodeContainer, TGraphContainer> : IGroup
        where TNodeContainer : class, IContainer<Node>
        where TGraphContainer : class, IContainer<IGraph>
    {

        [SerializeField, Tooltip("The title of the group")]
        private string title;

        [SerializeField, Tooltip("The position of the group")]
        private Vector2 position;

        [SerializeReference, Tooltip("The nodes contained in the group")]
        protected List<TNodeContainer> innerNodes = new List<TNodeContainer>();

        [SerializeReference, Tooltip("The graph that contains this group")]
        private TGraphContainer graphContainer;


        ///////////////////////////////////////////////////////////////////////////
        /// <summary>The title of the group</summary>
        public string Title
        {
            get => title;
            set => title = value;
        }

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>The position of the group in the graph</summary>
        public Vector2 Position
        {
            get => position;
            set => position = value;
        }

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>The collection of nodes contained in this group</summary>
        public IReadOnlyCollection<Node> InnerNodes => innerNodes.Where(c => c != null).Select(c => c.Value).ToList();

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>The container of the graph</summary>
        public IContainer<IGraph> GraphContainer
        {
            get => graphContainer;
            set => graphContainer = value as TGraphContainer;
        }

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>Adds a node to this group</summary>
        /// <param name="node">The node to add to this group</param>
        /// <returns>The successfulness of the operation</returns>
        public bool AddNode(Node node)
        {
            bool validAddition = TryGetContainer(node, out TNodeContainer container) && !innerNodes.Contains(container);
            if (validAddition)
                innerNodes.Add(container);
            return validAddition;
        }

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>Removes a node from this group</summary>
        /// <param name="node">The node to remove from this group</param>
        /// <returns>The successfulness of the operation</returns>
        public bool RemoveNode(Node node)
        {
            bool validDelete = TryGetContainer(node, out TNodeContainer container);
            if (validDelete)
                innerNodes.Remove(container);
            return validDelete;
        }

        ///////////////////////////////////////////////////////////////////////////
        private bool TryGetContainer(Node node, out TNodeContainer castedContainer)
        {
            GraphContainer.Value.TryGetContainer(node, out var container);
            castedContainer = container as TNodeContainer;
            return castedContainer != null;
        }
    }
}