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
        agent.speed = parent.ChaseSpeed;
        agent.isStopped = false;
        anim.SetBool("isRunning", true);
    }

    public override void Update()
    {
        Vector3 direzione = (playerInfo.currentPosition - agent.transform.position).normalized;

        // Punto a distanza desiderata dal giocatore verso il nemico
        Vector3 targetPos = playerInfo.currentPosition - direzione * parent.AttackDistance;

        // Verifica che il punto sia sulla NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPos, out hit, 1.0f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }


        if (!agent.hasPath) return;


        switch (CanAttack())
        {
            case COMBATRANGE.CLOSE:
                if (parent.HasStateDefinition(STATE.REPOSITION))
                {
                    nextState = STATE.REPOSITION;
                    stage = EVENT.EXIT;
                }
                break;

            case COMBATRANGE.RANGE:
                nextState = STATE.ATTACK;
                stage = EVENT.EXIT;
                break;
        }
    }

    public override void Exit()
    {
        anim.SetBool("isRunning", false);
        base.Exit();
    }
}
