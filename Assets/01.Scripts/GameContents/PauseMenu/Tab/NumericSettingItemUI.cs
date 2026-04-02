using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NumericSettingItemUI : MonoBehaviour
{
    [SerializeField] private Slider m_slider;
    [SerializeField] private TMP_InputField m_inputField;
    [SerializeField] private TextMeshProUGUI m_valueText;
    [SerializeField] private Button m_valueButton;
    [SerializeField] private GameObject m_valueDisplayRoot;
    [SerializeField] private GameObject m_inputRoot;
    [SerializeField] private bool m_usePercentDisplay;
    [SerializeField] private string m_numberFormat = "0.0";

    private bool m_isRefreshingUI;
    private bool m_isEditing;
    private float m_currentValue;
    private Action<float> m_onValueCommitted;

    public void Initialize(float currentValue, Action<float> onValueCommitted)
    {
        m_onValueCommitted = onValueCommitted;
        m_currentValue = currentValue;
        RefreshValue(currentValue);
        ExitInputMode();
    }

    public void RefreshValue(float value)
    {
        m_currentValue = value;
        m_isRefreshingUI = true;

        if (m_slider != null)
        {
            m_slider.value = value;
        }

        string displayValue = GetDisplayString(value);

        if (m_inputField != null)
        {
            m_inputField.text = displayValue;
        }

        if (m_valueText != null)
        {
            m_valueText.text = displayValue;
        }

        ExitInputMode();
        m_isRefreshingUI = false;
    }

    public void OnSliderChanged(float value)
    {
        if (m_isRefreshingUI)
        {
            return;
        }

        CommitValue(value);
    }

    public void OnInputEndEdit(string value)
    {
        if (m_isRefreshingUI)
        {
            return;
        }

        if (!TryParseInputValue(value, out float parsedValue))
        {
            RefreshValue(m_currentValue);
            return;
        }

        CommitValue(parsedValue);
    }

    public void OnClickValueText()
    {
        EnterInputMode();
    }

    public void EnterInputMode()
    {
        if (m_valueDisplayRoot != null)
        {
            m_valueDisplayRoot.SetActive(false);
        }

        if (m_inputRoot != null)
        {
            m_inputRoot.SetActive(true);
        }

        if (m_inputField != null)
        {
            m_inputField.Select();
            m_inputField.ActivateInputField();
        }

        m_isEditing = true;
    }

    public void ExitInputMode()
    {
        if (m_inputRoot != null)
        {
            m_inputRoot.SetActive(false);
        }

        if (m_valueDisplayRoot != null)
        {
            m_valueDisplayRoot.SetActive(true);
        }

        m_isEditing = false;
    }

    private void CommitValue(float value)
    {
        float committedValue = m_usePercentDisplay ? Mathf.Clamp01(value) : Mathf.Max(0.0f, value);
        m_currentValue = committedValue;
        m_onValueCommitted?.Invoke(committedValue);
        RefreshValue(committedValue);
    }

    private string GetDisplayString(float value)
    {
        if (!m_usePercentDisplay)
        {
            return value.ToString(m_numberFormat);
        }

        int percent = Mathf.RoundToInt(Mathf.Clamp01(value) * 100f);
        return percent.ToString();
    }

    private bool TryParseInputValue(string value, out float parsedValue)
    {
        parsedValue = m_currentValue;

        if (!float.TryParse(value, out float rawValue))
        {
            return false;
        }

        if (!m_usePercentDisplay)
        {
            parsedValue = Mathf.Max(0.0f, rawValue);
            return true;
        }

        float percent = Mathf.Clamp(rawValue, 0.0f, 100.0f);
        parsedValue = percent / 100f;
        return true;
    }
}
