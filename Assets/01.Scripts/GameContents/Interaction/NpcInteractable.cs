using UnityEngine;

public class NpcInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string m_interactionName = "Talk";
    [SerializeField] private bool m_canInteract = true;
    [SerializeField] private string m_dialogueId;
    [SerializeField] private DialogueManager m_dialogueManager;

    public bool CanInteract
    {
        get { return m_canInteract; }
    }

    public string InteractionName
    {
        get { return m_interactionName; }
    }

    private void Awake()
    {
        if (m_dialogueManager == null)
        {
            m_dialogueManager = FindFirstObjectByType<DialogueManager>();
        }
    }

    public void Interact()
    {
        if (!m_canInteract)
        {
            return;
        }

        if (m_dialogueManager == null)
        {
            Debug.LogWarning($"{name}: DialogueManager is not assigned.");
            return;
        }

        if (string.IsNullOrWhiteSpace(m_dialogueId))
        {
            Debug.LogWarning($"{name}: DialogueId is empty.");
            return;
        }

        Debug.Log($"{name}: Try Start Dialogue '{m_dialogueId}'");

        bool started = m_dialogueManager.StartDialogue(m_dialogueId);
        if (!started)
        {
            Debug.LogWarning($"{name}: Failed to start dialogue '{m_dialogueId}'.");
            return;
        }
    }
}
