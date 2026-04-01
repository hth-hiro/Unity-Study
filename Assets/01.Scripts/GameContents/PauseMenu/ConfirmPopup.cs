using System;
using TMPro;
using UnityEngine;

public class ConfirmPopup : MonoBehaviour
{
    [SerializeField] private GameObject m_root;
    [SerializeField] private TextMeshProUGUI m_messageText;

    private Action m_onConfirm;

    public void Show(string message, Action onConfirm)
    {
        if (m_root != null)
        {
            m_root.SetActive(true);
        }
        else
        {
            gameObject.SetActive(true);
        }

        if (m_messageText != null)
        {
            m_messageText.text = message ?? string.Empty;
        }

        m_onConfirm = onConfirm;
    }

    public void Hide()
    {
        if (m_root != null)
        {
            m_root.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }

        m_onConfirm = null;
    }

    public void OnClickConfirm()
    {
        Action onConfirm = m_onConfirm;
        Hide();
        onConfirm?.Invoke();
    }

    public void OnClickCancel()
    {
        Hide();
    }

    public bool IsOpen()
    {
        if (m_root != null)
        {
            return m_root.activeSelf;
        }

        return gameObject.activeSelf;
    }
}
