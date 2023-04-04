using System;
using System.Collections.Generic;
using System.Reflection;

using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;

using UnityEngine;
using UnityEngine.UIElements;

using GraphViewNode = UnityEditor.Experimental.GraphView.Node;
using GraphViewPort = UnityEditor.Experimental.GraphView.Port;
using Object = UnityEngine.Object;


namespace SPACS.PLG.Graphs.Editor
{
    ///////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Graphic representation in the graph window of a node
    /// </summary>
    public class NodeElement : GraphViewNode
    {
        private const string hiddenClass = "hidden";

        // This dictionary maps ports to the corresponding GraphElement
        private readonly Dictionary<Port, GraphViewPort> portToElement = new Dictionary<Port, GraphViewPort>();

        // This dictionary maps a GraphField to the action that applies that field value to the element
        private readonly Dictionary<FieldInfo, Action> dataFieldsSetters = new Dictionary<FieldInfo, Action>();


        ///////////////////////////////////////////////////////////////////////////
        public IReadOnlyDictionary<Port, GraphViewPort> PortToElement => portToElement;

        ///////////////////////////////////////////////////////////////////////////
        public GraphPanel Panel { get; set; }

        ///////////////////////////////////////////////////////////////////////////
        public Node Node { private set; get; }

        ///////////////////////////////////////////////////////////////////////////
        public NodeElement(GraphPanel panel, Node node)
        {
            Panel = panel;
            Node = node;
            title = Node.Name;

            // Add the node ports
            foreach (Port port in Node.Ports)
            {
                var portOrientation = Orientation.Horizontal;
                var direction = port is IInputPort ? Direction.Input : Direction.Output;
                var portCapacity = port is ISinglePort ? GraphViewPort.Capacity.Single : GraphViewPort.Capacity.Multi;
                var nodeType = port.PortType;
                var portElement = InstantiatePort(portOrientation, direction, portCapacity, nodeType);
                portElement.portName = ObjectNames.NicifyVariableName(port.Label);
                portToElement[port] = portElement;
                (direction == Direction.Input ? inputContainer : outputContainer).Add(portElement);
            }

            // Refresh and prepare visual stuff
            RefreshExpandedState();
            RefreshPorts();
            PrepareTitle();
            PrepareGraphFields();
            Node.OnDrawNodeElementInEditor(mainContainer);

            // Set the node position
            SetPosition(new Rect(Node.Position, Vector2.zero));

            // Open the inspector on click
            RegisterCallback<MouseDownEvent>(e =>
            {
                if (Panel.selection.Count == 1)
                    Panel.SelectNode(Node);
            });
        }

        ///////////////////////////////////////////////////////////////////////////
        private void PrepareTitle()
        {
            // Create the text field used to edit the title
            var titleEditor = new TextField();
            titleEditor.AddToClassList("titleEditor");
            titleEditor.AddToClassList(hiddenClass);
            titleContainer.Add(titleEditor);

            // Show the text field when the title label is clicked
            var titleLabel = titleContainer.Q<Label>("title-label");
            titleLabel.RegisterCallback<MouseDownEvent>(e =>
            {
                titleEditor.value = title;
                titleEditor.RemoveFromClassList(hiddenClass);
                titleEditor.Focus();
                titleEditor.SelectAll();
                titleLabel.visible = false;
            });

            // Update the label when text field's content changes
            titleEditor.RegisterCallback<ChangeEvent<string>>(evt =>
            {
                titleLabel.text = evt.newValue;
            });

            // Rename the node and update the graph when the text field loses focus
            titleEditor.RegisterCallback<FocusOutEvent>(evt =>
            {
                if (Node == null)
                    return;
                Panel.Graph.RenameNode(Node, titleEditor.text);
                Panel.SaveGraphAsset();
                title = titleEditor.text;
                titleEditor.AddToClassList(hiddenClass);
                titleLabel.visible = true;
            });
        }

        ///////////////////////////////////////////////////////////////////////////
        private void PrepareGraphFields()
        {
            // Create the graph fields container
            var graphFieldsContainer = new VisualElement { name = "graphFieldsContainer" };
            graphFieldsContainer.AddToClassList("propertiesContainer");
            mainContainer.Add(graphFieldsContainer);

            // Add the graph fields in the node bottom part
            foreach (FieldInfo field in Node.GraphFields)
            {
                Type fieldType = field.FieldType;
                if (fieldType.IsSubclassOf(typeof(Port)))
                    continue;

                VisualElement visualElement = null;

                // Choose the correct tipe of edit field
                if (fieldType == typeof(Object) || fieldType.IsSubclassOf(typeof(Object)))
                    visualElement = NewObjectField(field);
                else if (fieldType.IsEnum)
                    visualElement = NewEnumField(field);
                else
                    visualElement = NewTextField(field);

                // Add it to the container
                if (visualElement != null)
                {
                    visualElement.name = field.Name;
                    visualElement.AddToClassList("nodeElementDataField");
                    graphFieldsContainer.Add(visualElement);
                }
            }

            // Hide the graph fields container when the node is collpased
            m_CollapseButton.RegisterCallback<MouseUpEvent>(e =>
            {
                if (ClassListContains("collapsed"))
                    graphFieldsContainer.AddToClassList(hiddenClass);
                else
                    graphFieldsContainer.RemoveFromClassList(hiddenClass);
            });
        }

        ///////////////////////////////////////////////////////////////////////////
        /// Text field for string data
        private TextField NewTextField(FieldInfo field)
        {
            string fieldName = ObjectNames.NicifyVariableName(field.Name);
            TextField textField = new TextField(fieldName);
            dataFieldsSetters[field] = () =>
            {
                object fieldValue = field.GetValue(Node);
                string stringValue = fieldValue != null ? fieldValue.ToString() : string.Empty;
                textField.SetValueWithoutNotify(stringValue);
            };
            dataFieldsSetters[field].Invoke();
            textField.RegisterCallback<FocusOutEvent>(evt =>
            {
                field.SetValue(Node, textField.value);
                Panel.SaveGraphAsset();
            });
            return textField;
        }

        ///////////////////////////////////////////////////////////////////////////
        /// Enum field for enums types
        private EnumField NewEnumField(FieldInfo field)
        {
            string fieldName = ObjectNames.NicifyVariableName(field.Name);
            EnumField enumField = new EnumField(fieldName);
            enumField.Init((Enum)field.GetValue(Node));
            dataFieldsSetters[field] = () =>
            {
                Enum newValue = (Enum)field.GetValue(Node);
                enumField.SetValueWithoutNotify(newValue);
            };
            enumField.RegisterCallback<ChangeEvent<Enum>>((evt) =>
            {
                enumField.value = evt.newValue;
                field.SetValue(Node, enumField.value);
                Panel.SaveGraphAsset();
            });
            return enumField;
        }

        ///////////////////////////////////////////////////////////////////////////
        /// Object field for unity object types
        private ObjectField NewObjectField(FieldInfo field)
        {
            string fieldName = ObjectNames.NicifyVariableName(field.Name);
            Panel.Graph.TryGetContainer(Node, out var nodeObject);
            ObjectField objectField = new ObjectField(fieldName)
            {
                objectType = field.FieldType,
                allowSceneObjects = nodeObject is MonoBehaviour
            };
            dataFieldsSetters[field] = () =>
            {
                Object newValue = (Object)field.GetValue(Node);
                objectField.SetValueWithoutNotify(newValue); ;
            };
            dataFieldsSetters[field].Invoke();
            objectField.RegisterCallback<ChangeEvent<Object>>(evt =>
            {
                field.SetValue(Node, objectField.value);
                Panel.SaveGraphAsset();
            });
            return objectField;
        }

        ///////////////////////////////////////////////////////////////////////////
        public void UpdateElement()
        {
            // Update all the GraphFields by calling all the actions stored in the
            // ataFieldsSetters dictionary
            foreach (Action setter in dataFieldsSetters.Values)
                setter.Invoke();
        }

        ///////////////////////////////////////////////////////////////////////////
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            base.BuildContextualMenu(evt);

            // Add the "Remove from group" action in the right click menu
            evt.menu.InsertAction(1, "Remove from group", e => Panel.RemoveNodesFromGroup(Panel.GetSelectedNodes()), DropdownMenuAction.Status.Normal);
        }
    }
}