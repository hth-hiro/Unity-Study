using UnityEngine;
using UnityEngine.UI;

public class CrosshairUI : MonoBehaviour
{
    [SerializeField] private RectTransform m_topLine;
    [SerializeField] private RectTransform m_bottomLine;
    [SerializeField] private RectTransform m_leftLine;
    [SerializeField] private RectTransform m_rightLine;
    [SerializeField] private RectTransform m_centerDot;

    [SerializeField] private Image m_topLineImage;
    [SerializeField] private Image m_bottomLineImage;
    [SerializeField] private Image m_leftLineImage;
    [SerializeField] private Image m_rightLineImage;
    [SerializeField] private Image m_centerDotImage;

    private PlaySettingsData.CrosshairTypeOption m_currentType = PlaySettingsData.CrosshairTypeOption.Cross;
    private Color m_currentColor = Color.white;
    private float m_currentThickness = 4.0f;
    private float m_currentLength = 8.0f;
    private float m_currentOpacity = 1.0f;
    private float m_crossGap = 50f;

    public void ApplySettings(PlaySettingsData settings)
    {
        if (settings == null)
        {
            return;
        }

        m_currentType = settings.CrosshairType;
        m_currentColor = settings.CrosshairColor;
        m_currentThickness = ClampThickness(settings.CrosshairThickness);
        m_currentLength = ClampLength(settings.CrosshairLength);
        m_currentOpacity = ClampOpacity(settings.CrosshairOpacity);
        m_crossGap = settings.CrosshairGap;

        Refresh();
    }

    public void Refresh()
    {
        UpdateShape();
        UpdateColor();
        UpdateSize();
        UpdatePosition();
    }

    public void Clear()
    {
        SetLineActive(false);
        SetDotActive(false);
    }

    public void SetCrosshairType(PlaySettingsData.CrosshairTypeOption crosshairType)
    {
        m_currentType = crosshairType;
        Refresh();
    }

    public void SetCrosshairColor(Color color)
    {
        m_currentColor = color;
        Refresh();
    }

    public void SetCrosshairThickness(float thickness)
    {
        m_currentThickness = ClampThickness(thickness);
        Refresh();
    }

    public void SetCrosshairLength(float length)
    {
        m_currentLength = ClampLength(length);
        Refresh();
    }

    public void SetCrosshairOpacity(float opacity)
    {
        m_currentOpacity = ClampOpacity(opacity);
        Refresh();
    }

    private void UpdateShape()
    {
        bool isDot = m_currentType == PlaySettingsData.CrosshairTypeOption.Dot;
        //SetDotActive(isDot);
        SetLineActive(!isDot);
    }

    private void UpdateColor()
    {
        Color appliedColor = m_currentColor;
        appliedColor.a = m_currentOpacity;

        SetImageColor(m_topLineImage, appliedColor);
        SetImageColor(m_bottomLineImage, appliedColor);
        SetImageColor(m_leftLineImage, appliedColor);
        SetImageColor(m_rightLineImage, appliedColor);
        SetImageColor(m_centerDotImage, appliedColor);
    }

    private void UpdateSize()
    {
        SetSize(m_centerDot, new Vector2(m_currentThickness, m_currentThickness));
        SetSize(m_topLine, new Vector2(m_currentThickness, m_currentLength));
        SetSize(m_bottomLine, new Vector2(m_currentThickness, m_currentLength));
        SetSize(m_leftLine, new Vector2(m_currentLength, m_currentThickness));
        SetSize(m_rightLine, new Vector2(m_currentLength, m_currentThickness));
    }

    private void UpdatePosition()
    {
        SetPosition(m_centerDot, Vector2.zero);
        SetPosition(m_topLine, new Vector2(0.0f, m_crossGap));
        SetPosition(m_bottomLine, new Vector2(0.0f, -m_crossGap));
        SetPosition(m_leftLine, new Vector2(-m_crossGap, 0.0f));
        SetPosition(m_rightLine, new Vector2(m_crossGap, 0.0f));
    }

    private void SetLineActive(bool isActive)
    {
        SetActive(m_topLine, isActive);
        SetActive(m_bottomLine, isActive);
        SetActive(m_leftLine, isActive);
        SetActive(m_rightLine, isActive);
    }

    private void SetDotActive(bool isActive)
    {
        SetActive(m_centerDot, isActive);
    }

    private float ClampThickness(float thickness)
    {
        return Mathf.Max(0.0f, thickness);
    }

    private float ClampLength(float length)
    {
        return Mathf.Max(0.0f, length);
    }

    private float ClampOpacity(float opacity)
    {
        return Mathf.Clamp01(opacity);
    }

    private void SetImageColor(Image image, Color color)
    {
        if (image == null)
        {
            return;
        }

        image.color = color;
    }

    private void SetSize(RectTransform target, Vector2 size)
    {
        if (target == null)
        {
            return;
        }

        target.sizeDelta = size;
    }

    private void SetPosition(RectTransform target, Vector2 position)
    {
        if (target == null)
        {
            return;
        }

        target.anchoredPosition = position;
    }

    private void SetActive(RectTransform target, bool isActive)
    {
        if (target == null)
        {
            return;
        }

        target.gameObject.SetActive(isActive);
    }
}
