using UnityEngine;

[CreateAssetMenu(fileName = "NewCrosshair", menuName = "KuroAka/Crosshair")]
public class CrosshairData : ScriptableObject
{
    public enum CrosshairType { Dot, Cross, Circle, Square }

    [Header("Type Settings")]
    public CrosshairType type = CrosshairType.Cross;
    public Sprite centerSprite; // 조준점 도형

    [Header("Visual Settings")]
    public Color color = Color.white;
    [Range(0f, 1f)] public float opacity = 1.0f;
    public float thickness = 4.0f;
    public float length = 8.0f;
    public float gap = 10f;
}
