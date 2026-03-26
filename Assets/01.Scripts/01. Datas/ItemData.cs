using UnityEngine;

public enum ItemType { Charm, Grimoire }

[CreateAssetMenu(fileName = "New Item", menuName = "Game/Item")]
public class ItemData : ScriptableObject
{
    [Header("Basic")]
    public string ItemName;
    public Sprite Icon;
    [TextArea]
    public string Description;

    [Header("Type")]
    public ItemType itemType = ItemType.Charm;

    [Header("Price")]
    public int BuyPrice;
    public int SellPrice;

    [Header("Gameplay")]
    public float Cooldown;
    public float Duration;
}

