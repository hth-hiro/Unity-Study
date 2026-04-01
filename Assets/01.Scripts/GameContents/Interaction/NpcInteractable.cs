using UnityEngine;

public class NpcInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string m_interactionName = "Talk";
    [SerializeField] private bool m_canInteract = true;
    [SerializeField] private string m_dialogueId;
    [SerializeField] private DialogueManager m_dialogueManager;
    [SerializeField] private LobbyMenuManager m_lobbyMenuManager;
    [SerializeField] private bool m_openLobbyMenuAfterDialogue = true;

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

        if (m_lobbyMenuManager == null)
        {
            m_lobbyMenuManager = FindFirstObjectByType<LobbyMenuManager>();
        }
    }

    private void OnDestroy()
    {
        UnsubscribeDialogueEnded();
    }

    private void OnDisable()
    {
        UnsubscribeDialogueEnded();
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

        UnsubscribeDialogueEnded();

        bool started = m_dialogueManager.StartDialogue(m_dialogueId);
        if (!started)
        {
            Debug.LogWarning($"{name}: Failed to start dialogue '{m_dialogueId}'.");
            return;
        }

        if (m_openLobbyMenuAfterDialogue)
        {
            m_dialogueManager.OnDialogueEnded += OnDialogueEnded;
        }

        PlayerController.Instance?.SetInputBlock(true);
    }

    private void OnDialogueEnded()
    {
        UnsubscribeDialogueEnded();

        if (!m_openLobbyMenuAfterDialogue || m_lobbyMenuManager == null)
        {
            return;
        }

        m_lobbyMenuManager.OpenLobbyMenu();
        m_lobbyMenuManager.ShowMainPanel();
    }

    private void UnsubscribeDialogueEnded()
    {
        if (m_dialogueManager == null)
        {
            return;
        }

        m_dialogueManager.OnDialogueEnded -= OnDialogueEnded;
    }
}
