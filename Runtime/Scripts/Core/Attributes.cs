using System;

namespace Reflectis.PLG.Graphs
{
    ///////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Use this attribute to specify a custom label for a Node Port
    /// </summary>
    public class PortLabel : Attribute
    {
        public string label;

        ///////////////////////////////////////////////////////////////////////////
        public PortLabel(string label)
        {
            this.label = label;
        }
    }

    ///////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Use this attribute to show a field on the Node itself
    /// </summary>
    public class NodeData : Attribute
    {
    }
}