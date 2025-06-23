using System;
using UnityEngine;

public enum ShootMode { Single, Burst, Cluster }

public abstract class ShootDataSO : ScriptableObject
{
    //Firing variants
    public ShootMode shootMode;

    //Angle added to the bullets initial trajectory when shot
    public float inaccuracyAngle;

    //The vertical kickback given to the aim after shooting
    public float recoilPower;

    //Time between the bullets/series of bullets
    public float firerate;

    //If the weapon keeps firing when the player holds the shooting button
    public bool automatic;

    public Action<Vector3> onShoot;

    public abstract void Shoot(Vector3 lookDir);
}
