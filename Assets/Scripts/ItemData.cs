using UnityEngine;

// ScriptableObject는 기존 Monobeavior와 다르게 씬이 파괴되어도 유지되는 스크립트
// 데이터를 담는 스크립트이며, Project 내에서 .asset 형태로 존재
// 씬에 붙지 않기 때문에 생애주기 함수는 사용 불가능

public enum ItemType { Equipment, Consumable, Ingredient }  // 장비, 소모품, 재료

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public string description;
    public int maxStack = 64;
    public ItemType itemType;
}

