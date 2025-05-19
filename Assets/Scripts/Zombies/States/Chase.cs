using System;
using UnityEngine;
using UnityEngine.AI;

public class Chase : State
{
    StateDefinition patrol;
    StateDefinition attack;

    public Chase(GameObject _npc, NavMeshAgent _agent, Animator _anim, StateDefinition _patrol, StateDefinition _attack) : base(_npc, _agent, _anim)
    {
        patrol = _patrol;
        attack = _attack;
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
            nextState = attack.CreateState(npc, agent, anim);
            stage = EVENT.EXIT;
        }
        else if(!CanSeePlayer())
        {
            nextState = patrol.CreateState(npc, agent, anim);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        anim.SetBool("isRunning", false);
        base.Exit();
    }
}
