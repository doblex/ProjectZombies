using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Patrol : State
{
    StateDefinition chase;
    int currentCheckpoint = -1;

    Vector3 wanderTarget = Vector3.zero;

    public Patrol(GameObject _npc, NavMeshAgent _agent, Animator _anim, StateDefinition _chase) : base(_npc, _agent, _anim)
    {
        chase = _chase;
        stateName = STATE.PATROL;
    }

    public override void Enter()
    {
        base.Enter();

        agent.speed = 1f;
        agent.isStopped = false;

        float lastDist = Mathf.Infinity;

        anim.SetBool("isWalking", true);
        wanderTarget = agent.transform.position;
    }

    public override void Update()
    {
        if (CanSeePlayer())
        {
            nextState = chase.CreateState(npc, agent, anim);
            stage = EVENT.EXIT;
        }

        Wander();
    }

    public override void Exit()
    {
        anim.SetBool("isWalking", false);
        base.Exit();
    }

    void Wander()
    {
        float wanderRadius = 5f;
        float wanderDistance = 4f;
        float wanderJitter = 1f;

        wanderTarget += new Vector3(Random.Range(-1f, 1f) * wanderJitter, 0, Random.Range(-1f, 1f) * wanderJitter);

        wanderTarget.Normalize();

        wanderTarget *= wanderRadius;
        

        Vector3 targetLocal = agent.transform.localPosition + wanderTarget;

        targetLocal +=  new Vector3(0, 0, wanderDistance);

        Seek(targetLocal);
    }
}
