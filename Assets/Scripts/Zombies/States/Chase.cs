using System;
using UnityEngine;
using UnityEngine.AI;

public class Chase : State
{

    public Chase(GameObject _npc, NavMeshAgent _agent, Animator _anim, BehaviourController _behaviour) : base(_npc, _agent, _anim, _behaviour)
    {
        stateName = STATE.CHASE;
    }

    public override void Enter()
    {
        base.Enter();
        agent.speed = 5;
        agent.isStopped = false;
        anim.SetBool("isRunning", true);
    }

    public override void Update()
    {
        agent.SetDestination(playerInfo.currentPosition);

        if (!agent.hasPath) return;

        if(CanAttack())
        {
            nextState = STATE.ATTACK;
            stage = EVENT.EXIT;
        }
        else if(!CanSeePlayer())
        {
            nextState = STATE.PATROL;
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        anim.SetBool("isRunning", false);
        base.Exit();
    }
}
