using UnityEngine;


[CreateAssetMenu(fileName = "ShootDataSingle", menuName = "ScriptableObjects/ShootData/ShootDataSingle", order = 1)]
public class ShootDataSingleSO : ShootDataSO
{
    public override void Shoot(Vector3 lookDir)
    {
        Debug.Log("Shot!");
        onShoot.Invoke(lookDir); 
    
    }
}
