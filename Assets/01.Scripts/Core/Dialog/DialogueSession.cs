using System;

namespace Core.Dialogue
{
    /// <summary>
    /// Stores runtime state for one active dialogue.
    /// </summary>
    public sealed class DialogueSession
    {
        /// <summary>
        /// Initializes a new dialogue session.
        /// </summary>
        /// <param name="dialogueData">Dialogue data to run.</param>
        public DialogueSession(DialogueData dialogueData)
        {
            DialogueData = dialogueData ?? throw new ArgumentNullException(nameof(dialogueData));

            DialogueNode startNode = DialogueData.GetNode(DialogueData.StartNodeId);
            if (startNode == null)
            {
                throw new InvalidOperationException("Failed to start dialogue because the start node was not found.");
            }

            CurrentNode = startNode;
            IsActive = true;
            IsEnded = startNode.IsEnd;

            if (IsEnded)
            {
                IsActive = false;
            }
        }

        /// <summary>
        /// Dialogue data that owns this session.
        /// </summary>
        public DialogueData DialogueData { get; }

        /// <summary>
        /// Current node of the running session.
        /// </summary>
        public DialogueNode CurrentNode { get; private set; }

        /// <summary>
        /// Whether the session is currently active.
        /// </summary>
        public bool IsActive { get; private set; }

        /// <summary>
        /// Whether the dialogue has reached an ended state.
        /// </summary>
        public bool IsEnded { get; private set; }

        /// <summary>
        /// Moves the session to the provided node.
        /// </summary>
        /// <param name="node">Target node.</param>
        public void MoveToNode(DialogueNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            CurrentNode = node;
            IsEnded = node.IsEnd;
            IsActive = !IsEnded;
        }

        /// <summary>
        /// Ends the current session.
        /// </summary>
        public void EndSession()
        {
            IsActive = false;
            IsEnded = true;
        }
    }
}
