using Reflectis.SDK.Utilities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UnityEngine;
using UnityEngine.UIElements;

namespace Reflectis.PLG.Graphs
{
    ///////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Base class for all Nodes in the GraphSystem
    /// </summary>
    [Serializable]
    public class Node
    {

        ///////////////////////////////////////////////////////////////////////////
        [SerializeField, HideInInspector]
        private string guid;

        [SerializeField, Tooltip("The name of the node")]
        private string name;

        [SerializeField, HideInInspector]
        private Vector2 position;

        ///////////////////////////////////////////////////////////////////////////
        [NonSerialized, HideInInspector]
        private List<Port> ports = default;

        [NonSerialized, HideInInspector]
        private List<FieldInfo> graphFields = default;

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>The unique id for this node</summary>
        public string Guid
        {
            get => guid;
            set => guid = value;
        }

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>The name of this node</summary>
        public string Name
        {
            get => name;
            set => name = value;
        }

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>The position of this node in the graph</summary>
        public Vector2 Position
        {
            get => position;
            set => position = value;
        }

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>The collection of input and output ports of this node</summary>
        public IReadOnlyCollection<Port> Ports
        {
            get
            {
                if (ports == null)
                {
                    ports = new List<Port>();
                    foreach (FieldInfo field in ReflectionUtilities.GetSerializedFields(GetType()))
                    {
                        if (field.FieldType.IsSubclassOf(typeof(Port)))
                        {
                            Port port = (Port)field.GetValue(this);
                            if (port == null)
                            {
                                port = (Port)Activator.CreateInstance(field.FieldType);
                                field.SetValue(this, port);
                            }

                            PortLabel labelAttribute = field.GetCustomAttribute<PortLabel>();

                            port.Name = field.Name;
                            port.Label = labelAttribute != null ? labelAttribute.label : field.Name;
                            port.OwnerNode = this;
                            ports.Add(port);
                        }
                    }
                }
                return ports;
            }
        }

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>The collection of input ports of this node</summary>
        public IReadOnlyCollection<Port> InputPorts => Ports.Where(p => p is IInputPort).ToList();

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>The collection of output ports of this node</summary>
        public IReadOnlyCollection<Port> OutputPorts => Ports.Where(p => p is IOutputPort).ToList();

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>The fields that this node shows in the graph</summary>
        public IReadOnlyCollection<FieldInfo> GraphFields
        {
            get
            {
                if (graphFields == null)
                {
                    graphFields = new List<FieldInfo>();
                    foreach (FieldInfo field in ReflectionUtilities.GetSerializedFields(GetType()))
                    {
                        NodeData dataAttribute = field.GetCustomAttribute<NodeData>();
                        if (dataAttribute != null)
                            graphFields.Add(field);
                    }
                }
                return graphFields;
            }
        }

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>Method called by the editor to customize the appearance of
        /// this node in the graph view</summary>
        /// <param name="element">The VisualElement containing the node</param>
        public virtual void OnDrawNodeElementInEditor(VisualElement element) { }

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>Static generation of a node of a specified type</summary>
        /// <param name="nodeType">A subclass node type</param>
        /// <returns>The generated node</returns>
        public static Node GenerateNode(Type nodeType)
        {
            Node node = null;
            if (nodeType.IsSubclassOf(typeof(Node)))
            {
                node = (Node)Activator.CreateInstance(nodeType);
                node.Guid = System.Guid.NewGuid().ToString();
                node.Name = $"New {nodeType.Name}";
            }

            return node;
        }

        #region Generated Code
        ///////////////////////////////////////////////////////////////////////////
        public override bool Equals(object obj) => obj is Node node && Guid == node.Guid;

        ///////////////////////////////////////////////////////////////////////////
        public override int GetHashCode() => -1324198676 + EqualityComparer<string>.Default.GetHashCode(Guid);

        ///////////////////////////////////////////////////////////////////////////
        public static bool operator ==(Node left, Node right) => EqualityComparer<Node>.Default.Equals(left, right);

        ///////////////////////////////////////////////////////////////////////////
        public static bool operator !=(Node left, Node right) => !(left == right);
        #endregion
    }
}