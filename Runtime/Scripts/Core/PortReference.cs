using System;
using System.Linq;

using UnityEngine;

namespace Reflectis.PLG.Graphs
{
    ///////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// A utility class used to serialize a reference to a port as a pair of
    /// node container + port name
    /// </summary>
    [Serializable]
    public abstract class PortReference<NodeReference> : IPortReference
        where NodeReference : class, IContainer<Node>
    {

        [SerializeReference]
        private NodeReference nodeContainer;

        [SerializeField]
        private string portName;

        ///////////////////////////////////////////////////////////////////////////
        [NonSerialized]
        private Port port;


        ///////////////////////////////////////////////////////////////////////////
        /// <summary>The container of the node having this port</summary>
        public IContainer<Node> NodeContainer
        {
            get => nodeContainer;
            set => nodeContainer = value as NodeReference;
        }

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>The node having this port</summary>
        public Node Node => NodeContainer?.Value;

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>The name of the references port</summary>
        public string PortName
        {
            get => portName;
            set => portName = value;
        }

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>The referenced port</summary>
        public Port Port
        {
            get
            {
                if (port == null && Node != null)
                    port = Node.Ports.Where(f => f.Name == PortName).FirstOrDefault();

                return port;
            }
            set
            {
                port = value;
                PortName = value.Name;
            }
        }
    }
}