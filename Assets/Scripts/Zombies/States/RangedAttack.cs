using UnityEngine;
using UnityEngine.AI;

public class RangedAttack : Attack
{
    public RangedAttack(GameObject _npc, NavMeshAgent _agent, Animator _anim, BehaviourController _behaviour) : base(_npc, _agent, _anim, _behaviour)
    {
        stateName = STATE.ATTACK;
    }
}
