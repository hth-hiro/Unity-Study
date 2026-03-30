using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DialogueCsvLoader
{
    public Dictionary<string, DialogueData> Load(TextAsset nodeCsv, TextAsset choiceCsv)
    {
        if (nodeCsv == null)
        {
            throw new ArgumentNullException(nameof(nodeCsv));
        }

        if (choiceCsv == null)
        {
            throw new ArgumentNullException(nameof(choiceCsv));
        }

        Dictionary<string, DialogueData> dialogueMap = new Dictionary<string, DialogueData>();
        ParseNodes(nodeCsv.text, dialogueMap);
        ParseChoices(choiceCsv.text, dialogueMap);
        return dialogueMap;
    }

    private void ParseNodes(string csvText, Dictionary<string, DialogueData> dialogueMap)
    {
        List<List<string>> rows = ParseCsv(csvText);
        if (rows.Count <= 1)
        {
            return;
        }

        Dictionary<string, int> headerMap = BuildHeaderMap(rows[0]);

        for (int i = 1; i < rows.Count; i++)
        {
            List<string> row = rows[i];
            string dialogueId = GetValue(row, headerMap, "DialogueId");
            string nodeId = GetValue(row, headerMap, "NodeId");

            if (string.IsNullOrWhiteSpace(dialogueId) || string.IsNullOrWhiteSpace(nodeId))
            {
                continue;
            }

            if (!dialogueMap.TryGetValue(dialogueId, out DialogueData dialogueData))
            {
                dialogueData = new DialogueData();
                dialogueData.DialogueId = dialogueId;
                dialogueData.StartNodeId = nodeId;
                dialogueMap.Add(dialogueId, dialogueData);
            }

            DialogueNode dialogueNode = new DialogueNode();
            dialogueNode.NodeId = nodeId;
            dialogueNode.SpeakerName = GetValue(row, headerMap, "SpeakerName");
            dialogueNode.Text = GetValue(row, headerMap, "Text");
            dialogueNode.NextNodeId = GetValue(row, headerMap, "NextNodeId");
            dialogueNode.IsEnd = ParseBool(GetValue(row, headerMap, "IsEnd"));
            dialogueNode.EnsureChoices();

            if (dialogueData.Nodes == null)
            {
                dialogueData.Nodes = new List<DialogueNode>();
            }

            dialogueData.Nodes.Add(dialogueNode);
        }
    }

    private void ParseChoices(string csvText, Dictionary<string, DialogueData> dialogueMap)
    {
        List<List<string>> rows = ParseCsv(csvText);
        if (rows.Count <= 1)
        {
            return;
        }

        Dictionary<string, int> headerMap = BuildHeaderMap(rows[0]);

        for (int i = 1; i < rows.Count; i++)
        {
            List<string> row = rows[i];
            string dialogueId = GetValue(row, headerMap, "DialogueId");
            string nodeId = GetValue(row, headerMap, "NodeId");
            string choiceText = GetValue(row, headerMap, "ChoiceText");
            string nextNodeId = GetValue(row, headerMap, "NextNodeId");

            if (string.IsNullOrWhiteSpace(dialogueId) || string.IsNullOrWhiteSpace(nodeId))
            {
                continue;
            }

            if (string.IsNullOrWhiteSpace(choiceText) || string.IsNullOrWhiteSpace(nextNodeId))
            {
                continue;
            }

            if (!dialogueMap.TryGetValue(dialogueId, out DialogueData dialogueData))
            {
                Debug.LogWarning($"DialogueCsvLoader: DialogueId '{dialogueId}' was not found.");
                continue;
            }

            DialogueNode dialogueNode = dialogueData.GetNode(nodeId);
            if (dialogueNode == null)
            {
                Debug.LogWarning($"DialogueCsvLoader: Node '{nodeId}' was not found in '{dialogueId}'.");
                continue;
            }

            dialogueNode.EnsureChoices();
            dialogueNode.Choices.Add(new DialogueChoice
            {
                ChoiceText = choiceText,
                NextNodeId = nextNodeId
            });
        }
    }

    private List<List<string>> ParseCsv(string csvText)
    {
        List<List<string>> rows = new List<List<string>>();
        List<string> currentRow = new List<string>();
        StringBuilder currentValue = new StringBuilder();
        bool isInsideQuotes = false;

        for (int i = 0; i < csvText.Length; i++)
        {
            char currentChar = csvText[i];

            if (currentChar == '"')
            {
                bool isEscapedQuote = isInsideQuotes && i + 1 < csvText.Length && csvText[i + 1] == '"';
                if (isEscapedQuote)
                {
                    currentValue.Append('"');
                    i++;
                }
                else
                {
                    isInsideQuotes = !isInsideQuotes;
                }

                continue;
            }

            if (currentChar == ',' && !isInsideQuotes)
            {
                currentRow.Add(currentValue.ToString());
                currentValue.Length = 0;
                continue;
            }

            if ((currentChar == '\n' || currentChar == '\r') && !isInsideQuotes)
            {
                if (currentChar == '\r' && i + 1 < csvText.Length && csvText[i + 1] == '\n')
                {
                    i++;
                }

                currentRow.Add(currentValue.ToString());
                currentValue.Length = 0;

                if (!IsEmptyRow(currentRow))
                {
                    rows.Add(currentRow);
                }

                currentRow = new List<string>();
                continue;
            }

            currentValue.Append(currentChar);
        }

        if (currentValue.Length > 0 || currentRow.Count > 0)
        {
            currentRow.Add(currentValue.ToString());
            if (!IsEmptyRow(currentRow))
            {
                rows.Add(currentRow);
            }
        }

        return rows;
    }

    private Dictionary<string, int> BuildHeaderMap(List<string> headerRow)
    {
        Dictionary<string, int> headerMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        for (int i = 0; i < headerRow.Count; i++)
        {
            string key = headerRow[i].Trim();
            if (!string.IsNullOrEmpty(key) && !headerMap.ContainsKey(key))
            {
                headerMap.Add(key, i);
            }
        }

        return headerMap;
    }

    private string GetValue(List<string> row, Dictionary<string, int> headerMap, string key)
    {
        if (!headerMap.TryGetValue(key, out int index))
        {
            return string.Empty;
        }

        if (index < 0 || index >= row.Count)
        {
            return string.Empty;
        }

        return row[index].Trim();
    }

    private bool ParseBool(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        if (bool.TryParse(value, out bool boolValue))
        {
            return boolValue;
        }

        if (int.TryParse(value, out int intValue))
        {
            return intValue != 0;
        }

        return string.Equals(value, "y", StringComparison.OrdinalIgnoreCase);
    }

    private bool IsEmptyRow(List<string> row)
    {
        for (int i = 0; i < row.Count; i++)
        {
            if (!string.IsNullOrWhiteSpace(row[i]))
            {
                return false;
            }
        }

        return true;
    }
}
