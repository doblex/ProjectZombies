using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Patrol : State
{

    Vector3 wanderTarget = Vector3.zero;

    public Patrol(GameObject _npc, NavMeshAgent _agent, Animator _anim, BehaviourController _behaviour) : base(_npc, _agent, _anim, _behaviour)
    {
        stateName = STATE.PATROL;
    }

    public override void Enter()
    {
        base.Enter();

        agent.speed = parent.WanderSpeed;
        agent.isStopped = false;

        anim.SetBool("isWalking", true);
        wanderTarget = agent.transform.position;
    }

    public override void Update()
    {
        if (CanSeePlayer())
        {
            nextState = STATE.CHASE;
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
