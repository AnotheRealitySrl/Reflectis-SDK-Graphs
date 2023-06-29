using System.Collections.Generic;

using UnityEngine;

namespace SPACS.PLG.Graphs
{
    ///////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Graph group interface
    /// </summary>
    public interface IGroup
    {
        /// <summary>The title of the group</summary>
        string Title { get; set; }

        /// <summary>The position of the group in the graph</summary>
        Vector2 Position { get; set; }

        /// <summary>The collection of nodes contained in this group</summary>
        IReadOnlyCollection<Node> InnerNodes { get; }

        /// <summary>The container of the graph</summary>
        IContainer<IGraph> GraphContainer { get; set; }


        /// <summary>Adds a node to this group</summary>
        /// <param name="node">The node to add to this group</param>
        /// <returns>The successfulness of the operation</returns>
        bool AddNode(Node node);

        /// <summary>Removes a node from this group</summary>
        /// <param name="node">The node to remove from this group</param>
        /// <returns>The successfulness of the operation</returns>
        bool RemoveNode(Node node);
    }
}