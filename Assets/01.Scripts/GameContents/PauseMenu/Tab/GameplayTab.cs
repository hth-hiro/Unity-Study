using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayTab : MonoBehaviour
{
    [Header("Numeric Items")]
    [SerializeField] private NumericSettingItemUI m_sensitivityItem;
    [SerializeField] private NumericSettingItemUI m_crosshairThicknessItem;
    [SerializeField] private NumericSettingItemUI m_crosshairLengthItem;
    [SerializeField] private NumericSettingItemUI m_crosshairOpacityItem;

    [Header("Crosshair Type")]
    [SerializeField] private Button m_prevCrosshairTypeButton;
    [SerializeField] private Button m_nextCrosshairTypeButton;
    [SerializeField] private TextMeshProUGUI m_crosshairTypeValueText;
    [SerializeField] private string[] m_crosshairTypeLabels;

    [Header("Crosshair Color")]
    [SerializeField] private Button m_prevCrosshairColorButton;
    [SerializeField] private Button m_nextCrosshairColorButton;
    [SerializeField] private TextMeshProUGUI m_crosshairColorValueText;
    [SerializeField] private string[] m_crosshairColorLabels;
    [SerializeField] private Color[] m_crosshairColorOptions;

    private bool m_isInitialized;

    private void OnEnable()
    {
        if (SettingsManager.Instance == null)
        {
            return;
        }

        if (!m_isInitialized)
        {
            BindNumericItems();
            m_isInitialized = true;
        }

        RefreshUIFromSettings();
    }

    public void RefreshUIFromSettings()
    {
        if (SettingsManager.Instance == null || SettingsManager.Instance.Play == null)
        {
            return;
        }

        PlaySettingsData play = SettingsManager.Instance.Play;

        if (m_sensitivityItem != null)
        {
            m_sensitivityItem.RefreshValue(play.MouseSensitivity);
        }

        if (m_crosshairThicknessItem != null)
        {
            m_crosshairThicknessItem.RefreshValue(play.CrosshairThickness);
        }

        if (m_crosshairLengthItem != null)
        {
            m_crosshairLengthItem.RefreshValue(play.CrosshairLength);
        }

        if (m_crosshairOpacityItem != null)
        {
            m_crosshairOpacityItem.RefreshValue(play.CrosshairOpacity);
        }

        UpdateCrosshairTypeText(play.CrosshairType);
        UpdateCrosshairColorText(play.CrosshairColor);
    }

    public void OnClickPrevCrosshairType()
    {
        if (SettingsManager.Instance == null || SettingsManager.Instance.Play == null)
        {
            return;
        }

        int typeCount = System.Enum.GetValues(typeof(PlaySettingsData.CrosshairTypeOption)).Length;
        int currentIndex = (int)SettingsManager.Instance.Play.CrosshairType;
        int nextIndex = GetPrevWrappedIndex(currentIndex, typeCount);
        PlaySettingsData.CrosshairTypeOption nextType = (PlaySettingsData.CrosshairTypeOption)nextIndex;

        SettingsManager.Instance.SetCrosshairType(nextType);
        SettingsManager.Instance.ApplyPlay();
        UpdateCrosshairTypeText(nextType);
    }

    public void OnClickNextCrosshairType()
    {
        if (SettingsManager.Instance == null || SettingsManager.Instance.Play == null)
        {
            return;
        }

        int typeCount = System.Enum.GetValues(typeof(PlaySettingsData.CrosshairTypeOption)).Length;
        int currentIndex = (int)SettingsManager.Instance.Play.CrosshairType;
        int nextIndex = GetNextWrappedIndex(currentIndex, typeCount);
        PlaySettingsData.CrosshairTypeOption nextType = (PlaySettingsData.CrosshairTypeOption)nextIndex;

        SettingsManager.Instance.SetCrosshairType(nextType);
        SettingsManager.Instance.ApplyPlay();
        UpdateCrosshairTypeText(nextType);
    }

    public void OnClickPrevCrosshairColor()
    {
        if (SettingsManager.Instance == null || SettingsManager.Instance.Play == null || m_crosshairColorOptions == null || m_crosshairColorOptions.Length == 0)
        {
            return;
        }

        int currentIndex = GetClosestCrosshairColorIndex(SettingsManager.Instance.Play.CrosshairColor);
        int nextIndex = GetPrevWrappedIndex(currentIndex, m_crosshairColorOptions.Length);
        Color nextColor = m_crosshairColorOptions[nextIndex];

        SettingsManager.Instance.SetCrosshairColor(nextColor);
        SettingsManager.Instance.ApplyPlay();
        UpdateCrosshairColorText(nextColor);
    }

    public void OnClickNextCrosshairColor()
    {
        if (SettingsManager.Instance == null || SettingsManager.Instance.Play == null || m_crosshairColorOptions == null || m_crosshairColorOptions.Length == 0)
        {
            return;
        }

        int currentIndex = GetClosestCrosshairColorIndex(SettingsManager.Instance.Play.CrosshairColor);
        int nextIndex = GetNextWrappedIndex(currentIndex, m_crosshairColorOptions.Length);
        Color nextColor = m_crosshairColorOptions[nextIndex];

        SettingsManager.Instance.SetCrosshairColor(nextColor);
        SettingsManager.Instance.ApplyPlay();
        UpdateCrosshairColorText(nextColor);
    }

    private void BindNumericItems()
    {
        if (SettingsManager.Instance == null || SettingsManager.Instance.Play == null)
        {
            return;
        }

        PlaySettingsData play = SettingsManager.Instance.Play;

        if (m_sensitivityItem != null)
        {
            m_sensitivityItem.Initialize(play.MouseSensitivity, OnSensitivityCommitted);
        }

        if (m_crosshairThicknessItem != null)
        {
            m_crosshairThicknessItem.Initialize(play.CrosshairThickness, OnCrosshairThicknessCommitted);
        }

        if (m_crosshairLengthItem != null)
        {
            m_crosshairLengthItem.Initialize(play.CrosshairLength, OnCrosshairLengthCommitted);
        }

        if (m_crosshairOpacityItem != null)
        {
            m_crosshairOpacityItem.Initialize(play.CrosshairOpacity, OnCrosshairOpacityCommitted);
        }
    }

    private void OnSensitivityCommitted(float value)
    {
        if (SettingsManager.Instance == null)
        {
            return;
        }

        SettingsManager.Instance.SetMouseSensitivity(value);
        SettingsManager.Instance.ApplyPlay();
    }

    private void OnCrosshairThicknessCommitted(float value)
    {
        if (SettingsManager.Instance == null)
        {
            return;
        }

        SettingsManager.Instance.SetCrosshairThickness(value);
        SettingsManager.Instance.ApplyPlay();
    }

    private void OnCrosshairLengthCommitted(float value)
    {
        if (SettingsManager.Instance == null)
        {
            return;
        }

        SettingsManager.Instance.SetCrosshairLength(value);
        SettingsManager.Instance.ApplyPlay();
    }

    private void OnCrosshairOpacityCommitted(float value)
    {
        if (SettingsManager.Instance == null)
        {
            return;
        }

        SettingsManager.Instance.SetCrosshairOpacity(value);
        SettingsManager.Instance.ApplyPlay();
    }

    private void UpdateCrosshairTypeText(PlaySettingsData.CrosshairTypeOption type)
    {
        if (m_crosshairTypeValueText == null)
        {
            return;
        }

        int index = (int)type;
        if (m_crosshairTypeLabels != null && index >= 0 && index < m_crosshairTypeLabels.Length)
        {
            m_crosshairTypeValueText.text = m_crosshairTypeLabels[index];
            return;
        }

        m_crosshairTypeValueText.text = type.ToString();
    }

    private void UpdateCrosshairColorText(Color color)
    {
        if (m_crosshairColorValueText == null)
        {
            return;
        }

        int index = GetClosestCrosshairColorIndex(color);
        if (m_crosshairColorLabels != null && index >= 0 && index < m_crosshairColorLabels.Length)
        {
            m_crosshairColorValueText.text = m_crosshairColorLabels[index];
            return;
        }

        m_crosshairColorValueText.text = $"RGB({Mathf.RoundToInt(color.r * 255f)}, {Mathf.RoundToInt(color.g * 255f)}, {Mathf.RoundToInt(color.b * 255f)})";
    }

    private int GetClosestCrosshairColorIndex(Color targetColor)
    {
        if (m_crosshairColorOptions == null || m_crosshairColorOptions.Length == 0)
        {
            return 0;
        }

        int closestIndex = 0;
        float closestDistance = float.MaxValue;

        for (int i = 0; i < m_crosshairColorOptions.Length; i++)
        {
            float distance = Vector4.Distance(m_crosshairColorOptions[i], targetColor);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        return closestIndex;
    }

    private int GetPrevWrappedIndex(int currentIndex, int count)
    {
        if (count <= 0)
        {
            return 0;
        }

        return (currentIndex - 1 + count) % count;
    }

    private int GetNextWrappedIndex(int currentIndex, int count)
    {
        if (count <= 0)
        {
            return 0;
        }

        return (currentIndex + 1) % count;
    }
}
