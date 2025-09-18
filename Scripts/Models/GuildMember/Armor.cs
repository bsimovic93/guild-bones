using Godot;

[Tool]
public partial class Armor : Equipment
{
    protected ArmorType armorType;
    public ArmorType ArmorType
    {
        get => armorType;
        set => armorType = value;
    }
}