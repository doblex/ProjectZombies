using UnityEngine;


[CreateAssetMenu(fileName = "ShootDataCluster", menuName = "ScriptableObjects/ShootData/ShootDataCluster", order = 1)]
public class ShootDataClusterSO : ShootDataSO
{
    //Amount of simultaneously shot bullets
    public int bulletsNum;

    public override void Shoot(Vector3 lookDir)
    {
        for (int i = 0; i < bulletsNum; i++)
        {
            Debug.Log("Shot!");
            onShoot.Invoke(lookDir);
        }
    }
}
