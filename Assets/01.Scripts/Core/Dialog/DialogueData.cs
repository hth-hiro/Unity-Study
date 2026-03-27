using System;
using System.Collections.Generic;

namespace Core.Dialogue
{
    /// <summary>
    /// Represents a full dialogue data set.
    /// </summary>
    [Serializable]
    public sealed class DialogueData
    {
        /// <summary>
        /// Unique identifier of the dialogue.
        /// </summary>
        public string DialogueId = string.Empty;

        /// <summary>
        /// Node id used as the first node of the dialogue.
        /// </summary>
        public string StartNodeId = string.Empty;

        /// <summary>
        /// All nodes contained in the dialogue.
        /// </summary>
        public List<DialogueNode> Nodes = new List<DialogueNode>();

        /// <summary>
        /// Returns the first node that matches the provided node id.
        /// </summary>
        /// <param name="nodeId">Target node id.</param>
        /// <returns>The matching node or null when not found.</returns>
        public DialogueNode GetNode(string nodeId)
        {
            if (string.IsNullOrEmpty(nodeId))
            {
                return null;
            }

            if (Nodes == null)
            {
                Nodes = new List<DialogueNode>();
                return null;
            }

            for (int i = 0; i < Nodes.Count; i++)
            {
                DialogueNode node = Nodes[i];
                if (node == null)
                {
                    continue;
                }

                node.EnsureChoices();

                if (string.Equals(node.NodeId, nodeId, StringComparison.Ordinal))
                {
                    return node;
                }
            }

            return null;
        }
    }

    /// <summary>
    /// Represents one dialogue node.
    /// </summary>
    [Serializable]
    public sealed class DialogueNode
    {
        /// <summary>
        /// Unique identifier of the node.
        /// </summary>
        public string NodeId = string.Empty;

        /// <summary>
        /// Speaker display name.
        /// </summary>
        public string SpeakerName = string.Empty;

        /// <summary>
        /// Dialogue text shown for this node.
        /// </summary>
        public string Text = string.Empty;

        /// <summary>
        /// Choices available from this node.
        /// </summary>
        public List<DialogueChoice> Choices = new List<DialogueChoice>();

        /// <summary>
        /// Next node id for linear progression.
        /// </summary>
        public string NextNodeId = string.Empty;

        /// <summary>
        /// Whether this node ends the dialogue.
        /// </summary>
        public bool IsEnd;

        /// <summary>
        /// Returns true when the node has one or more choices.
        /// </summary>
        public bool HasChoices
        {
            get
            {
                EnsureChoices();
                return Choices.Count > 0;
            }
        }

        /// <summary>
        /// Ensures the choices list is never null.
        /// </summary>
        public void EnsureChoices()
        {
            if (Choices == null)
            {
                Choices = new List<DialogueChoice>();
            }
        }
    }

    /// <summary>
    /// Represents one dialogue choice.
    /// </summary>
    [Serializable]
    public sealed class DialogueChoice
    {
        /// <summary>
        /// Text shown to the player for this choice.
        /// </summary>
        public string ChoiceText = string.Empty;

        /// <summary>
        /// Next node id reached when this choice is selected.
        /// </summary>
        public string NextNodeId = string.Empty;
    }
}
