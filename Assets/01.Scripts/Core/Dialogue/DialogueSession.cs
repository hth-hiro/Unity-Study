using System;

public sealed class DialogueSession
{
    public DialogueSession(DialogueData dialogueData)
    {
        DialogueData = dialogueData ?? throw new ArgumentNullException(nameof(dialogueData));

        DialogueNode startNode = DialogueData.GetNode(DialogueData.StartNodeId);
        if (startNode == null)
        {
            throw new InvalidOperationException("Start node was not found.");
        }

        CurrentNode = startNode;
        IsActive = true;
        IsEnded = startNode.IsEnd;

        if (IsEnded)
        {
            IsActive = false;
        }
    }

    public DialogueData DialogueData { get; }
    public DialogueNode CurrentNode { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsEnded { get; private set; }

    public void MoveToNode(DialogueNode node)
    {
        if (node == null)
        {
            throw new ArgumentNullException(nameof(node));
        }

        CurrentNode = node;
        IsEnded = node.IsEnd;
        IsActive = !node.IsEnd;
    }

    public void EndSession()
    {
        IsActive = false;
        IsEnded = true;
    }
}
