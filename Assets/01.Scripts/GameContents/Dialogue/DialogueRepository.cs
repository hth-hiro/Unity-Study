using System.Collections.Generic;

public class DialogueRepository
{
    private readonly Dictionary<string, DialogueData> m_dialogueDataMap = new Dictionary<string, DialogueData>();

    public void SetData(Dictionary<string, DialogueData> dialogueDataMap)
    {
        m_dialogueDataMap.Clear();

        if (dialogueDataMap == null)
        {
            return;
        }

        foreach (KeyValuePair<string, DialogueData> pair in dialogueDataMap)
        {
            if (string.IsNullOrWhiteSpace(pair.Key) || pair.Value == null)
            {
                continue;
            }

            m_dialogueDataMap[pair.Key] = pair.Value;
        }
    }

    public DialogueData GetDialogue(string dialogueId)
    {
        if (string.IsNullOrWhiteSpace(dialogueId))
        {
            return null;
        }

        if (m_dialogueDataMap.TryGetValue(dialogueId, out DialogueData dialogueData))
        {
            return dialogueData;
        }

        return null;
    }

    public bool HasDialogue(string dialogueId)
    {
        if (string.IsNullOrWhiteSpace(dialogueId))
        {
            return false;
        }

        return m_dialogueDataMap.ContainsKey(dialogueId);
    }

    public void Clear()
    {
        m_dialogueDataMap.Clear();
    }
}
