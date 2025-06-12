using UnityEngine;

public class OnRangedAttack : OnAttack
{
    protected override void PerformAttack() 
    {
        PlayerInfo playerInfo = parent.CurrentState.PlayerInfo;

        if (!parent.IsRanged) { Debug.LogError("The Behaviour is melee but the state implies it is ranged"); return; }

        GameObject clone = ObjectPooling.Instance.GetOrAdd(parent.ProjectilePrefab, parent.ShootPos.position, parent.ShootPos.rotation);

        Rigidbody body = clone.GetComponent<Rigidbody>();

        body.linearVelocity = CalculateVelocity(playerInfo.currentPosition, parent.ShootPos.position, parent.ProjectileSpeed);
    }
}
