// DialogueManager.cs
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("CSV")]
    [SerializeField] private TextAsset m_nodeCsv;
    [SerializeField] private TextAsset m_choiceCsv;

    [Header("UI")]
    [SerializeField] private GameObject m_dialogueUI;
    [SerializeField] private TextMeshProUGUI m_speakerNameText;
    [SerializeField] private TextMeshProUGUI m_dialogueText;

    private DialogueRepository m_dialogueRepository;
    private DialogueSystem m_dialogueSystem;

    private void Awake()
    {
        m_dialogueRepository = new DialogueRepository();
        m_dialogueSystem = new DialogueSystem();

        if (m_nodeCsv == null || m_choiceCsv == null)
        {
            Debug.LogWarning("DialogueManager: Node CSV or Choice CSV is not assigned.");
            return;
        }

        try
        {
            DialogueCsvLoader dialogueCsvLoader = new DialogueCsvLoader();
            Dictionary<string, DialogueData> dialogueDataMap = dialogueCsvLoader.Load(m_nodeCsv, m_choiceCsv);
            m_dialogueRepository.SetData(dialogueDataMap);
        }
        catch (Exception exception)
        {
            Debug.LogError($"DialogueManager: Failed to load dialogue CSV. {exception.Message}");
            m_dialogueRepository.Clear();
        }
    }

    private void Start()
    {
        if (m_dialogueUI != null)
        {
            m_dialogueUI.SetActive(false);
        }

        ClearDialogueText();
    }

    public bool StartDialogue(string dialogueId)
    {
        if (string.IsNullOrWhiteSpace(dialogueId))
        {
            return false;
        }

        if (m_dialogueRepository == null || m_dialogueSystem == null)
        {
            return false;
        }

        if (m_dialogueSystem.HasActiveSession())
        {
            return false;
        }

        DialogueData dialogueData = m_dialogueRepository.GetDialogue(dialogueId);
        if (dialogueData == null)
        {
            Debug.LogWarning($"DialogueManager: DialogueId '{dialogueId}' was not found.");
            return false;
        }

        try
        {
            m_dialogueSystem.StartDialogue(dialogueData);
            DialogueNode currentNode = m_dialogueSystem.GetCurrentNode();

            Debug.Log($"DialogueManager: Request='{dialogueId}', Found='{dialogueData.DialogueId}', StartNode='{dialogueData.StartNodeId}', Text='{currentNode?.Text}'");

            if (m_dialogueUI != null)
            {
                m_dialogueUI.SetActive(true);
            }

            RefreshCurrentNode();
            return true;
        }
        catch (Exception exception)
        {
            Debug.LogWarning($"DialogueManager: Failed to start dialogue '{dialogueId}'. {exception.Message}");
            return false;
        }
    }

    public DialogueNode GetCurrentNode()
    {
        if (m_dialogueSystem == null)
        {
            return null;
        }

        return m_dialogueSystem.GetCurrentNode();
    }

    public bool HasActiveDialogue()
    {
        if (m_dialogueSystem == null)
        {
            return false;
        }

        return m_dialogueSystem.HasActiveSession();
    }

    public bool Advance()
    {
        if (m_dialogueSystem == null)
        {
            return false;
        }

        bool result = m_dialogueSystem.Advance();
        if (result)
        {
            RefreshCurrentNode();
        }
        else if (!m_dialogueSystem.HasActiveSession())
        {
            HideDialogueUI();
        }

        return result;
    }

    public bool Choose(int choiceIndex)
    {
        if (m_dialogueSystem == null)
        {
            return false;
        }

        bool result = m_dialogueSystem.Choose(choiceIndex);
        if (result)
        {
            RefreshCurrentNode();
        }
        else if (!m_dialogueSystem.HasActiveSession())
        {
            HideDialogueUI();
        }

        return result;
    }

    public void EndDialogue()
    {
        if (m_dialogueSystem == null)
        {
            return;
        }

        m_dialogueSystem.EndDialogue();
        HideDialogueUI();

        PlayerController.Instance?.SetInputBlock(false);
    }

    public bool HasDialogue(string dialogueId)
    {
        if (m_dialogueRepository == null || string.IsNullOrWhiteSpace(dialogueId))
        {
            return false;
        }

        return m_dialogueRepository.HasDialogue(dialogueId);
    }

    private void RefreshCurrentNode()
    {
        DialogueNode currentNode = GetCurrentNode();
        if (currentNode == null)
        {
            ClearDialogueText();
            return;
        }

        if (m_speakerNameText != null)
        {
            m_speakerNameText.text = currentNode.SpeakerName ?? string.Empty;
        }

        if (m_dialogueText != null)
        {
            m_dialogueText.text = currentNode.Text ?? string.Empty;
        }
    }

    private void HideDialogueUI()
    {
        if (m_dialogueUI != null)
        {
            m_dialogueUI.SetActive(false);
        }

        ClearDialogueText();
    }

    private void ClearDialogueText()
    {
        if (m_speakerNameText != null)
        {
            m_speakerNameText.text = string.Empty;
        }

        if (m_dialogueText != null)
        {
            m_dialogueText.text = string.Empty;
        }
    }
}
