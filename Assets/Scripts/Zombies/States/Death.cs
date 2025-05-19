using UnityEngine;
using UnityEngine.AI;

public class Death : State
{
    public Death(GameObject _npc, NavMeshAgent _agent, Animator _anim) : base(_npc, _agent, _anim)
    {
        stateName = STATE.DEATH;
    }

    public override void Enter()
    {
        base.Enter();
        agent.isStopped = true;
        anim.SetBool("isDeath", true);
        anim.SetBool("isRunning", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isWalking", false);

        npc.SetActive(false);
    }
}