using UnityEngine;

public enum ItemType
{
    Weapon, Ring, Necklace
}

public enum ItemUpType
{
    Atk, Hp, Speed, Def
}

public enum ItemGrade
{
    Normal, Rare, Unique, Legendary, myth
}

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/Item")]
public class Item : ScriptableObject
{
    public Sprite itemImage;
    public ItemType itemType;
    public ItemUpType[] upType;
    public ItemGrade grade;
    public string itemName;
    public float[] upGradeValue;
    public string[] value;
}
