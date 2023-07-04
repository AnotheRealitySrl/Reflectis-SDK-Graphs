using System.Collections.Generic;

namespace Reflectis.PLG.Graphs
{
    ///////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Graph interface
    /// </summary>
    public interface IGraph
    {
        /// <summary>The collection of nodes that this graph contains</summary>
        IReadOnlyCollection<Node> Nodes { get; }

        /// <summary>The collection of groups that this graph contains</summary>
        IReadOnlyCollection<IGroup> Groups { get; }


        /// <summary>Adds a node</summary>
        /// <param name="node">The node to add</param>
        /// <returns>The successfulness of the operation</returns>
        bool AddNode(Node node);

        /// <summary>Renames a node</summary>
        /// <param name="node">The node to be renamed</param>
        /// <param name="newName">The new name to assign</param>
        /// <returns>True if the renaming was successful. False if the specified
        /// node is invalid or not contained in this graph</returns>
        bool RenameNode(Node node, string newName);

        /// <summary>Removes a node from the graph</summary>
        /// <param name="node">The node to remove</param>
        /// <returns>True if the removal was successful. False if the specified
        /// node is not valid or not contained in this graph</returns>
        bool RemoveNode(Node node);

        /// <summary>Retrieves the nodes of a specified type</summary>
        /// <typeparam name="T">A node subclass type</typeparam>
        /// <returns>A collection of nodes</returns>
        IReadOnlyCollection<T> GetNodes<T>() where T : Node;


        /// <summary>Links two ports together</summary>
        /// <param name="port1">The output port</param>
        /// <param name="port2">The input port</param>
        /// <returns>True if the link was successful. False if the specified
        /// ports are not valid or not contained in this graph</returns>
        bool AddLinkBetween(Port port1, Port port2);


        /// <summary>Creates a group for this graph</summary>
        /// <returns>The created group</returns>
        IGroup CreateGroup();

        /// <summary>Adds a group to this graph</summary>
        /// <param name="group">The group to add</param>
        void AddGroup(IGroup group);

        /// <summary>Removes a group from this graph</summary>
        /// <param name="group">The group to remove</param>
        void RemoveGroup(IGroup group);

        /// <summary>Finds the group that contains a specified node</summary>
        /// <param name="node">The node to search for</param>
        /// <returns>The containing group</returns>
        IGroup FindContainingGroup(Node node);


        /// <summary>Tries to find the container for a specified node</summary>
        /// <param name="node">The node to search for</param>
        /// <param name="container">The output container</param>
        /// <returns>The successfulness of the search</returns>
        bool TryGetContainer(Node node, out IContainer<Node> reference);
    }
}