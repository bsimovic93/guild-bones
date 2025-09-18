using Godot;

[Tool]
public partial class Equipment : Resource
{
    protected string equipmentName { get; set; }
    public string EquipmentName
    {
        get => equipmentName;
        set => equipmentName = value;
    }

    protected EquipmentStats equipmentStats { get; set; }
    public EquipmentStats EquipmentStats
    {
        get => equipmentStats;
        set => equipmentStats = value;
    }
}