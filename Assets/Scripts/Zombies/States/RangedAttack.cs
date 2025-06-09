using UnityEngine;
using UnityEngine.AI;

public class RangedAttack : Attack
{
    public RangedAttack(GameObject _npc, NavMeshAgent _agent, Animator _anim, BehaviourController _behaviour) : base(_npc, _agent, _anim, _behaviour)
    {
        stateName = STATE.ATTACK;
    }

    protected override void PerformAttack()
    {
        base.PerformAttack();
        if (!parent.IsRanged) { Debug.LogError("The Behaviour is melee but the state implies it is ranged"); return; }

        GameObject clone = GameObject.Instantiate(parent.ProjectilePrefab, parent.ShootPos.position, parent.ShootPos.rotation);
        Rigidbody body = clone.GetComponent<Rigidbody>();

        body.linearVelocity = CalculateVelocity(playerInfo.currentPosition, parent.ShootPos.position, parent.ProjectileSpeed);
    }
    Vector3 CalculateVelocity(Vector3 target, Vector3 origin, float speed)
    {
        Vector3 toTarget = target - origin;
        float yOffset = toTarget.y;
        toTarget.y = 0;

        float distance = toTarget.magnitude;
        float angle = 45f * Mathf.Deg2Rad;

        float yVel = Mathf.Sin(angle) * speed;
        float xzVel = Mathf.Cos(angle) * speed;

        Vector3 direction = toTarget.normalized * xzVel;
        direction.y = yVel;

        return direction;
    }
}
