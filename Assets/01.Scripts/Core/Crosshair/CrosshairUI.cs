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

    [SerializeField] private CrosshairData m_data;

    public void ApplySettings(PlaySettingsData settings)
    {
        if (settings == null)
        {
            return;
        }

        EnsureData();

        m_data.type = ConvertType(settings.CrosshairType);
        m_data.color = settings.CrosshairColor;
        m_data.thickness = ClampThickness(settings.CrosshairThickness);
        m_data.length = ClampLength(settings.CrosshairLength);
        m_data.opacity = ClampOpacity(settings.CrosshairOpacity);
        m_data.gap = ClampGap(settings.CrosshairGap);

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
        EnsureData();
        m_data.type = ConvertType(crosshairType);
        Refresh();
    }

    public void SetCrosshairColor(Color color)
    {
        EnsureData();
        m_data.color = color;
        Refresh();
    }

    public void SetCrosshairThickness(float thickness)
    {
        EnsureData();
        m_data.thickness = ClampThickness(thickness);
        Refresh();
    }

    public void SetCrosshairLength(float length)
    {
        EnsureData();
        m_data.length = ClampLength(length);
        Refresh();
    }

    public void SetCrosshairOpacity(float opacity)
    {
        EnsureData();
        m_data.opacity = ClampOpacity(opacity);
        Refresh();
    }

    private void UpdateShape()
    {
        EnsureData();

        bool useCenterSprite =
            m_data.type == CrosshairData.CrosshairType.Dot ||
            m_data.type == CrosshairData.CrosshairType.Circle ||
            m_data.type == CrosshairData.CrosshairType.Square;

        if (m_centerDotImage != null && useCenterSprite)
        {
            m_centerDotImage.sprite = m_data.centerSprite;
        }

        SetDotActive(useCenterSprite);
        SetLineActive(m_data.type == CrosshairData.CrosshairType.Cross);
    }

    private void UpdateColor()
    {
        EnsureData();

        Color appliedColor = m_data.color;
        appliedColor.a = m_data.opacity;

        SetImageColor(m_topLineImage, appliedColor);
        SetImageColor(m_bottomLineImage, appliedColor);
        SetImageColor(m_leftLineImage, appliedColor);
        SetImageColor(m_rightLineImage, appliedColor);
        SetImageColor(m_centerDotImage, appliedColor);
    }

    private void UpdateSize()
    {
        EnsureData();

        SetSize(m_centerDot, new Vector2(m_data.thickness, m_data.thickness));
        SetSize(m_topLine, new Vector2(m_data.thickness, m_data.length));
        SetSize(m_bottomLine, new Vector2(m_data.thickness, m_data.length));
        SetSize(m_leftLine, new Vector2(m_data.length, m_data.thickness));
        SetSize(m_rightLine, new Vector2(m_data.length, m_data.thickness));
    }

    private void UpdatePosition()
    {
        EnsureData();

        SetPosition(m_centerDot, Vector2.zero);
        SetPosition(m_topLine, new Vector2(0.0f, m_data.gap));
        SetPosition(m_bottomLine, new Vector2(0.0f, -m_data.gap));
        SetPosition(m_leftLine, new Vector2(-m_data.gap, 0.0f));
        SetPosition(m_rightLine, new Vector2(m_data.gap, 0.0f));
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

    private float ClampGap(float gap)
    {
        return Mathf.Max(0.0f, gap);
    }

    private CrosshairData.CrosshairType ConvertType(PlaySettingsData.CrosshairTypeOption type)
    {
        return type == PlaySettingsData.CrosshairTypeOption.Dot
            ? CrosshairData.CrosshairType.Dot
            : CrosshairData.CrosshairType.Cross;
    }

    private void EnsureData()
    {
        if (m_data != null)
        {
            return;
        }

        m_data = ScriptableObject.CreateInstance<CrosshairData>();
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
