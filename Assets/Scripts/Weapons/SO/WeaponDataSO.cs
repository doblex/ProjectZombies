using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/WeaponData", order = 1)]
public class WeaponDataSO : ScriptableObject
{
    //Amount of bullets to shoot before needing to reload
    public int bulletStartingCapacity;

    //Total amount of bullets that can be carried
    public int bulletCapacity;

    //Bullet damage
    public int bulletDamage;

    //Amount of enemies the bullet can hit in its trajectory
    public int piercePower;

    //Weapon reload timer
    public float reloadTime;

    //Weapon reload timer
    public float bulletMaxDistance;

    //Weapon firing sound
    public AudioClip fireSFX;

    //Different shoot types for weapons that have various modes (Automatic, semi-automatic, burst)
    public ShootDataSO[] shootTypes;
}
