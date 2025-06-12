using UnityEngine;
using UnityEngine.AI;

internal class Reposition : State
{
    public Reposition(GameObject _npc, NavMeshAgent _agent, Animator _anim, BehaviourController _behaviuor) : base(_npc, _agent, _anim, _behaviuor)
    {
        stateName = STATE.REPOSITION;
    }

    public override void Enter()
    {
        base.Enter();
        agent.speed = parent.RepositionSpeed;
        agent.isStopped = false;
        anim.SetBool("isRunning", true);
    }

    public override void Update()
    {
        base.Update();

        if(Vector3.Distance(agent.transform.position , playerInfo.currentPosition) > parent.MinimumRange)
        {
            nextState = STATE.CHASE;
            stage = EVENT.EXIT;
            return;
        }

        Vector3 direzione = (agent.transform.position - playerInfo.currentPosition).normalized;

        // Punto a distanza desiderata dal giocatore verso il nemico
        Vector3 targetPos = (direzione * parent.MinimumRange) + agent.transform.position;

        // Verifica che il punto sia sulla NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPos, out hit, 1.0f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }

        if (!agent.hasPath) return;

        if (CanAttack() == COMBATRANGE.RANGE)
        {
            nextState = STATE.ATTACK;
            stage = EVENT.EXIT;
        }
    }
}