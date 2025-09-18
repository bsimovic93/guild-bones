public class EquipmentStats
{
    protected double damage;
    public double Damage
    {
        get => damage;
        set => damage = value;
    }

    protected EquipmentDamageType equipmentDamageType;
    public EquipmentDamageType EquipmentDamageType
    {
        get => equipmentDamageType;
        set => equipmentDamageType = value;
    }

    protected double attackSpeed;
    public double AttackSpeed
    {
        get => attackSpeed;
        set => attackSpeed = value;
    }

    protected double defense;
    public double Defense
    {
        get => defense;
        set => defense = value;
    }

    protected EquipmentDefenseType equipmentDefenseType;
    public EquipmentDefenseType EquipmentDefenseType
    {
        get => equipmentDefenseType;
        set => equipmentDefenseType = value;
    }
}