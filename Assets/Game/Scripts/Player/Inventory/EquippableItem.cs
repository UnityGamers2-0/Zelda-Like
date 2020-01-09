using UnityEngine;

public enum EquipmentType
{
    Helmet,
    Chestplate,
    Leggings,
    Boots,
    Weapon1,
    Weapon2
}


[CreateAssetMenu]
public class EquippableItem : Item
{
    public int StrengthBonus;
    public int AgillityBonus;
    public int IntelligenceBonus;
    public int VitalityBonus;
    [Space]
    public float StrengthPercentBonus;
    public float AgillityPercentBonus;
    public float IntelligencePercentBonus;
    public float VitalityPercentBonus;
    [Space]
    public EquipmentType EquipmentType;
}