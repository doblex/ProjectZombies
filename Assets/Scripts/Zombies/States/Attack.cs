using System;
using UnityEngine;
using UnityEngine.AI;

public class Attack : State
{
    StateDefinition chase;

    float rotationSpeed = 2.0f;
    AudioSource shoot;

    public Attack(GameObject _npc, NavMeshAgent _agent, Animator _anim, StateDefinition _chase) : base(_npc, _agent, _anim)
    {
        chase = _chase;

        stateName = STATE.ATTACK;
        shoot = _npc.GetComponent<AudioSource>();
    }

    public override void Enter()
    {
        base.Enter();
        anim.SetBool("isAttacking", true);
        agent.isStopped = true;
        //shoot.Play();
    }

    public override void Update()
    {
        Vector3 direction = playerInfo.currentPosition - npc.transform.position;
        float angle = Vector3.Angle(direction, npc.transform.forward);
        direction.y = 0;

        npc.transform.rotation = Quaternion.Slerp
            (npc.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotationSpeed);

        if (!CanAttack())
        {
            nextState = chase.CreateState(npc, agent, anim);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        anim.SetBool("isAttacking", false);
        //shoot.Stop();
        base.Exit();
    }
}
