using UnityEngine;
using UnityEngine.AI;

public class MeleeAttack : Attack
{
    public MeleeAttack(GameObject _npc, NavMeshAgent _agent, Animator _anim, BehaviourController _behaviour) : base(_npc, _agent, _anim, _behaviour)
    {
        stateName = STATE.ATTACK;
    }

    protected override void PerformAttack()
    {
        base.PerformAttack();
        if (parent.IsRanged) { Debug.LogError("The Behaviour is ranged, but the state implies it is melee"); return; }
    }
}
