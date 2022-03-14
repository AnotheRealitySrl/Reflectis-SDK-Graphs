
namespace SPACS.Graphs
{
    ///////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Interface for PortReference
    /// </summary>
    public interface IPortReference
    {
        /// <summary>The container of the node having this port</summary>
        IContainer<Node> NodeContainer { get; set; }

        /// <summary>The referenced port</summary>
        Port Port { get; set; }
    }
}