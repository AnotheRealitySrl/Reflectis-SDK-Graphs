namespace Reflectis.PLG.Graphs
{
    public interface ITaskNode<TNode> : IGraphTaskNode, IContainer<TNode> where TNode : class
    {
        ///////////////////////////////////////////////////////////////////////////
        public new TNode Value
        {
            get => Value as TNode;
            set => Value = value;
        }

        ///////////////////////////////////////////////////////////////////////////
        public TNode Node => Value;

        public TNode getTValue()
        {
            return Value;
        }

        public void AddDetector();

    }
}
