using UnityEngine;

namespace Reflectis.SDK.Graphs
{
    ///////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Abstract ScriptableObject that contains a node in a project asset
    /// </summary>
    public abstract class NodeAsset : ScriptableObject, IContainer<Node>
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
    /// Generic abstract ScriptableObject that contains a node in a project asset
    /// </summary>
    public abstract class NodeAsset<TNode> : NodeAsset, IContainer<TNode>
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