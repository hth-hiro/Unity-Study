// DialogueManager.cs
using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TextAsset m_nodeCsv;
    [SerializeField] private TextAsset m_choiceCsv;

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
            return false;
        }

        try
        {
            m_dialogueSystem.StartDialogue(dialogueData);
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

        return m_dialogueSystem.Advance();
    }

    public bool Choose(int choiceIndex)
    {
        if (m_dialogueSystem == null)
        {
            return false;
        }

        return m_dialogueSystem.Choose(choiceIndex);
    }

    public void EndDialogue()
    {
        if (m_dialogueSystem == null)
        {
            return;
        }

        m_dialogueSystem.EndDialogue();
    }

    public bool HasDialogue(string dialogueId)
    {
        if (m_dialogueRepository == null || string.IsNullOrWhiteSpace(dialogueId))
        {
            return false;
        }

        return m_dialogueRepository.HasDialogue(dialogueId);
    }
}
