using System.Collections.Generic;
using UnityEngine;
using GraphView = UnityEditor.Experimental.GraphView;

namespace SPACS.PLG.Graphs.Editor
{
    ///////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Graphic representation in the graph window of a group
    /// </summary>
    public class GroupElement : GraphView.Group
    {
        ///////////////////////////////////////////////////////////////////////////
        /// <summary> The graph panel </summary>
        public GraphPanel Panel { get; set; }

        ///////////////////////////////////////////////////////////////////////////
        /// <summary> The core group variable </summary>
        public IGroup Group { private set; get; }

        ///////////////////////////////////////////////////////////////////////////
        /// Constructor
        public GroupElement(GraphPanel panel, IGroup group)
        {
            Panel = panel;
            Group = group;

            autoUpdateGeometry = true;
            title = Group.Title;

            SetPosition(new Rect(Group.Position, Vector2.zero));
        }

        ///////////////////////////////////////////////////////////////////////////
        /// Called when the user adds one or more nodes inside this group
        protected override void OnElementsAdded(IEnumerable<GraphView.GraphElement> elements)
        {
            // Try to add each node to the group (and count the successes)
            int addedNodes = 0;
            foreach (var element in elements)
                if (element is NodeElement nodeElement)
                    if (Group.AddNode(nodeElement.Node))
                        addedNodes++;

            // Save the graph if at least one node has ben successfully added
            if (addedNodes > 0)
                Panel.SaveGraphAsset();

            // Update the group position
            Rect rect = GetPosition();
            rect.position = Group.Position;
            SetPosition(rect);

            // Call the base callback
            base.OnElementsAdded(elements);
        }

        ///////////////////////////////////////////////////////////////////////////
        /// Called when the user removes one or more nodes from this group
        protected override void OnElementsRemoved(IEnumerable<GraphView.GraphElement> elements)
        {
            if (parent != null)
            {
                // Try to remove each node from the group (and count the successes)
                int removedNodes = 0;
                foreach (var element in elements)
                    if (element is NodeElement nodeElement)
                        if (Group.RemoveNode(nodeElement.Node))
                            removedNodes++;

                // Save the graph if at least one node has ben successfully removed
                if (removedNodes > 0)
                    Panel.SaveGraphAsset();

                // Update the group position
                Rect rect = GetPosition();
                rect.position = Group.Position;
                SetPosition(rect);
            }

            // Call the base callback
            base.OnElementsRemoved(elements);
        }

        ///////////////////////////////////////////////////////////////////////////
        /// Group rename
        protected override void OnGroupRenamed(string oldName, string newName)
        {
            Group.Title = newName;
            base.OnGroupRenamed(oldName, newName);
        }
    }
}