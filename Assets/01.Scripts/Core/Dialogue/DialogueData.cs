using System;
using System.Collections.Generic;

namespace Core.Dialog
{
    [Serializable]
    public sealed class DialogueData
    {
        public string DialogueId = string.Empty;
        public string StartNodeId = string.Empty;
        public List<DialogueNode> Nodes = new List<DialogueNode>();

        public DialogueNode GetNode(string nodeId)
        {
            if (string.IsNullOrEmpty(nodeId) || Nodes == null)
            {
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

                if (node.NodeId == nodeId)
                {
                    return node;
                }
            }

            return null;
        }
    }

    [Serializable]
    public sealed class DialogueNode
    {
        public string NodeId = string.Empty;
        public string SpeakerName = string.Empty;
        public string Text = string.Empty;
        public List<DialogueChoice> Choices = new List<DialogueChoice>();
        public string NextNodeId = string.Empty;
        public bool IsEnd;

        public bool HasChoices
        {
            get
            {
                EnsureChoices();
                return Choices.Count > 0;
            }
        }

        public void EnsureChoices()
        {
            if (Choices == null)
            {
                Choices = new List<DialogueChoice>();
            }
        }
    }

    [Serializable]
    public sealed class DialogueChoice
    {
        public string ChoiceText = string.Empty;
        public string NextNodeId = string.Empty;
    }
}
