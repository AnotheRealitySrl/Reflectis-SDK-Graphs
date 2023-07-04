using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;


namespace Reflectis.PLG.Graphs
{
    ///////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// A "port" is an input or output entry-point of a node for connections
    /// with other nodes. This class defines the basic abstraction of a port
    /// </summary>
    public abstract class Port
    {
        [SerializeField, HideInInspector]
        private string name;

        [SerializeField, HideInInspector]
        private string label;

        [SerializeReference]
        protected List<IPortReference> links = new List<IPortReference>();

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>The port's name</summary>
        public string Name
        {
            get => name;
            set => name = value;
        }

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>The port's label</summary>
        public string Label
        {
            get => label;
            set => label = value;
        }

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>The port's type</summary>
        public abstract Type PortType { get; }

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>The node containing this port</summary>
        public Node OwnerNode { get; set; }

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>The collection of ports linked to this port</summary>
        public IReadOnlyCollection<Port> LinkedPorts => links
            .Select(r => r.Port)
            .Where(p => p != null)
            .ToList();

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>The collection of nodes linked to this port</summary>
        public IReadOnlyCollection<Node> LinkedNodes => LinkedPorts
            .Select(p => p.OwnerNode)
            .ToList();

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>Links this port to another port</summary>
        /// <param name="portReference">The port reference</param>
        public void AddLinkTo(IPortReference portReference)
        {
            if (IsLinkableTo(portReference.Port))
                links.Add(portReference);
        }

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>Tells if this port is linkable to another port</summary>
        /// <param name="port">The port to link to</param>
        /// <returns>True if the link is possible, false otherwise</returns>
        public bool IsLinkableTo(Port port) => PortType == port.PortType;

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>Deletes all the links between this port and another port</summary>
        /// <param name="port"></param>
        public void RemoveLinkTo(Port port) => links.RemoveAll(l => l.Port == port);

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>Deletes all the links between this port and any other port</summary>
        public void RemoveAllLinks() => links.Clear();
    }


    ///////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Generic version of the port abstract class
    /// </summary>
    public abstract class Port<T> : Port where T : Node
    {
        public new T OwnerNode
        {
            get => base.OwnerNode as T;
            set => base.OwnerNode = value;
        }

        public override Type PortType => typeof(T);

        public new IReadOnlyCollection<T> LinkedNodes => base.LinkedNodes
            .Cast<T>()
            .ToList();
    }


    ///////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Abstract class defining a port that allows multiple connection
    /// </summary>
    public abstract class PortToMultiPort<T> : Port<T> where T : Node { }


    ///////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Abstract class defining a port that allows only one connection
    /// </summary>
    public abstract class PortToSinglePort<T> : Port<T> where T : Node
    {

        public new void AddLinkTo(IPortReference portReference)
        {
            RemoveAllLinks();
            base.AddLinkTo(portReference);
        }
    }


    ///////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Abstract class defining an input port that allows only one connection
    /// </summary>
    [Serializable]
    public class InputPort<T> : PortToSinglePort<T>, IInputPort, ISinglePort where T : Node { }

    ///////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Abstract class defining an input port that allows multiple connections
    /// </summary>
    [Serializable]
    public class MultiInputPort<T> : PortToMultiPort<T>, IInputPort, IMultiPort where T : Node { }

    ///////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Abstract class defining an output port that allows only one connection
    /// </summary>
    [Serializable]
    public class OutputPort<T> : PortToSinglePort<T>, IOutputPort, ISinglePort where T : Node { }

    ///////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Abstract class defining an output port that allows multiple connections
    /// </summary>
    [Serializable]
    public class MultiOutputPort<T> : PortToMultiPort<T>, IOutputPort, IMultiPort where T : Node { }
}