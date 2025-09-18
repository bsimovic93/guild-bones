using Godot;

[Tool]
public partial class Weapon : Equipment
{
    protected WeaponType weaponType;
    public WeaponType WeaponType
    {
        get => weaponType;
        set => weaponType = value;
    }

    protected WeaponDamageType weaponDamageType;
    public WeaponDamageType WeaponDamageType
    {
        get => weaponDamageType;
        set => weaponDamageType = value;
    }
}