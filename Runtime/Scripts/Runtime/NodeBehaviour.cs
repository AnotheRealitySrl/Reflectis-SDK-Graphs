using UnityEngine;

namespace Reflectis.PLG.Graphs
{
    ///////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Abstract MonoBehaviour that contains a node in a scene component
    /// </summary>
    public abstract class NodeBehaviour : MonoBehaviour, IContainer<Node>
    {
        [SerializeReference]
        private Node node = default;

        ///////////////////////////////////////////////////////////////////////////
        public Node Value
        {
            get => node;
            set => node = value;
        }
    }


    ///////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Generic abstract MonoBehaviour that contains a node in a scene component
    /// </summary>
    public abstract class NodeBehaviour<TNode> : NodeBehaviour, IContainer<TNode>
        where TNode : Node
    {
        ///////////////////////////////////////////////////////////////////////////
        public new TNode Value
        {
            get => base.Value as TNode;
            set => base.Value = value;
        }

        ///////////////////////////////////////////////////////////////////////////
        public TNode Node => Value;
    }
}