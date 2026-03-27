using System;
using System.Collections.Generic;

namespace Core.Dialogue
{
    /// <summary>
    /// Pure logic system that advances dialogue sessions.
    /// </summary>
    public sealed class DialogueSystem
    {
        /// <summary>
        /// Current running dialogue session.
        /// </summary>
        public DialogueSession CurrentSession { get; private set; }

        /// <summary>
        /// Starts a new dialogue session.
        /// </summary>
        /// <param name="dialogueData">Dialogue data to run.</param>
        public void StartDialogue(DialogueData dialogueData)
        {
            CurrentSession = new DialogueSession(dialogueData);
        }

        /// <summary>
        /// Advances the current dialogue through a non-choice node.
        /// </summary>
        /// <returns>The new current node, or null when the dialogue ended.</returns>
        public DialogueNode Advance()
        {
            DialogueSession session = GetRequiredActiveSession();
            DialogueNode currentNode = session.CurrentNode;

            if (currentNode.IsEnd)
            {
                session.EndSession();
                return null;
            }

            if (currentNode.HasChoices)
            {
                throw new InvalidOperationException("Cannot advance automatically from a node that requires a choice.");
            }

            if (string.IsNullOrEmpty(currentNode.NextNodeId))
            {
                session.EndSession();
                return null;
            }

            DialogueNode nextNode = session.DialogueData.GetNode(currentNode.NextNodeId);
            if (nextNode == null)
            {
                throw new InvalidOperationException("Failed to advance dialogue because the next node was not found.");
            }

            session.MoveToNode(nextNode);
            return session.CurrentNode;
        }

        /// <summary>
        /// Selects a choice from the current node.
        /// </summary>
        /// <param name="choiceIndex">Zero-based choice index.</param>
        /// <returns>The new current node, or null when the dialogue ended.</returns>
        public DialogueNode SelectChoice(int choiceIndex)
        {
            DialogueSession session = GetRequiredActiveSession();
            DialogueNode currentNode = session.CurrentNode;

            if (!currentNode.HasChoices)
            {
                throw new InvalidOperationException("Cannot select a choice on a node without choices.");
            }

            List<DialogueChoice> choices = currentNode.Choices;
            if (choiceIndex < 0 || choiceIndex >= choices.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(choiceIndex));
            }

            DialogueChoice selectedChoice = choices[choiceIndex];
            if (selectedChoice == null)
            {
                throw new InvalidOperationException("The selected choice is null.");
            }

            if (string.IsNullOrEmpty(selectedChoice.NextNodeId))
            {
                session.EndSession();
                return null;
            }

            DialogueNode nextNode = session.DialogueData.GetNode(selectedChoice.NextNodeId);
            if (nextNode == null)
            {
                throw new InvalidOperationException("Failed to resolve the next node for the selected choice.");
            }

            session.MoveToNode(nextNode);
            return session.CurrentNode;
        }

        /// <summary>
        /// Ends the current dialogue session.
        /// </summary>
        public void EndDialogue()
        {
            if (CurrentSession == null)
            {
                return;
            }

            CurrentSession.EndSession();
        }

        /// <summary>
        /// Returns whether a dialogue can advance linearly from the current node.
        /// </summary>
        public bool CanAdvance
        {
            get
            {
                if (CurrentSession == null || !CurrentSession.IsActive || CurrentSession.CurrentNode == null)
                {
                    return false;
                }

                DialogueNode currentNode = CurrentSession.CurrentNode;
                return !currentNode.IsEnd && !currentNode.HasChoices;
            }
        }

        /// <summary>
        /// Returns whether the current node expects a choice.
        /// </summary>
        public bool CanSelectChoice
        {
            get
            {
                if (CurrentSession == null || !CurrentSession.IsActive || CurrentSession.CurrentNode == null)
                {
                    return false;
                }

                return CurrentSession.CurrentNode.HasChoices;
            }
        }

        private DialogueSession GetRequiredActiveSession()
        {
            if (CurrentSession == null)
            {
                throw new InvalidOperationException("There is no active dialogue session.");
            }

            if (!CurrentSession.IsActive)
            {
                throw new InvalidOperationException("The current dialogue session is not active.");
            }

            if (CurrentSession.CurrentNode == null)
            {
                throw new InvalidOperationException("The current dialogue node is not available.");
            }

            return CurrentSession;
        }
    }
}
