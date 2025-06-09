using System;
using UnityEngine;
using UnityEngine.AI;

public class Idle : State
{

    public Idle(GameObject _npc, NavMeshAgent _agent, Animator _anim, BehaviourController _behaviour) : base(_npc, _agent, _anim, _behaviour)
    {
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
            nextState = STATE.CHASE;
            stage = EVENT.EXIT;
        }

        else if (UnityEngine.Random.Range(0, 100) > 60)
        {
            nextState = STATE.PATROL;
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        anim.SetBool("isIdle", false);
        base.Exit();
    }
}
