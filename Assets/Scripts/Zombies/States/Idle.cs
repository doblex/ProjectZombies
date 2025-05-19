using System;
using UnityEngine;
using UnityEngine.AI;

public class Idle : State
{
    StateDefinition patrol;
    StateDefinition chase;

    public Idle(GameObject _npc, NavMeshAgent _agent, Animator _anim, StateDefinition _patrol, StateDefinition _chase) : base(_npc, _agent, _anim)
    {
        patrol = _patrol;
        chase = _chase;
        stateName = STATE.IDLE;
    }

    public override void Enter()
    {
        base.Enter();
        agent.speed = 0f;
        agent.isStopped = true;
        anim.SetBool("isIdle", true);
    }

    public override void Update()
    {
        if(CanSeePlayer())
        {
            nextState = chase.CreateState(npc, agent, anim);
            stage = EVENT.EXIT;
        }

        else if (UnityEngine.Random.Range(0, 100) > 60)
        {
            nextState = patrol.CreateState(npc, agent, anim);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        anim.SetBool("isIdle", false);
        base.Exit();
    }
}
