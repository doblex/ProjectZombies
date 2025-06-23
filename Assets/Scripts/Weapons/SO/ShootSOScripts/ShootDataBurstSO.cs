using UnityEngine;


[CreateAssetMenu(fileName = "ShootDataBurst", menuName = "ScriptableObjects/ShootData/ShootDataBurst", order = 1)]
public class ShootDataBurstSO : ShootDataSO
{
    //Amount of bullets fired in rapid succession
    public int burstNum;

    //Time between a bullet and the other during the burst
    public float burstFirerate;

    public override void Shoot(Vector3 lookDir)
    {
        Debug.Log("Shot!");
        onShoot.Invoke(lookDir);
    }

}
