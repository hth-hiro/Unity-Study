using System;

public sealed class DialogueSystem
{
    public DialogueSession CurrentSession { get; private set; }

    public void StartDialogue(DialogueData dialogueData)
    {
        if (dialogueData == null)
        {
            throw new ArgumentNullException(nameof(dialogueData));
        }

        if (HasActiveSession())
        {
            throw new InvalidOperationException("A dialogue session is already active.");
        }

        CurrentSession = new DialogueSession(dialogueData);
    }

    public DialogueNode GetCurrentNode()
    {
        if (CurrentSession == null)
        {
            return null;
        }

        return CurrentSession.CurrentNode;
    }

    public bool HasActiveSession()
    {
        return CurrentSession != null && CurrentSession.IsActive;
    }

    public bool CanAdvance()
    {
        if (!HasActiveSession())
        {
            return false;
        }

        DialogueNode currentNode = CurrentSession.CurrentNode;
        if (currentNode == null || currentNode.IsEnd || currentNode.HasChoices)
        {
            return false;
        }

        return !string.IsNullOrEmpty(currentNode.NextNodeId);
    }

    public bool Advance()
    {
        if (!CanAdvance())
        {
            return false;
        }

        DialogueNode currentNode = CurrentSession.CurrentNode;
        DialogueNode nextNode = CurrentSession.DialogueData.GetNode(currentNode.NextNodeId);
        if (nextNode == null)
        {
            return false;
        }

        CurrentSession.MoveToNode(nextNode);
        return true;
    }

    public bool CanChoose(int choiceIndex)
    {
        if (!HasActiveSession())
        {
            return false;
        }

        DialogueNode currentNode = CurrentSession.CurrentNode;
        if (currentNode == null || !currentNode.HasChoices)
        {
            return false;
        }

        return choiceIndex >= 0 && choiceIndex < currentNode.Choices.Count;
    }

    public bool Choose(int choiceIndex)
    {
        if (!CanChoose(choiceIndex))
        {
            return false;
        }

        DialogueChoice choice = CurrentSession.CurrentNode.Choices[choiceIndex];
        if (choice == null || string.IsNullOrEmpty(choice.NextNodeId))
        {
            return false;
        }

        DialogueNode nextNode = CurrentSession.DialogueData.GetNode(choice.NextNodeId);
        if (nextNode == null)
        {
            return false;
        }

        CurrentSession.MoveToNode(nextNode);
        return true;
    }

    public void EndDialogue()
    {
        if (CurrentSession == null)
        {
            return;
        }

        CurrentSession.EndSession();
    }
}
