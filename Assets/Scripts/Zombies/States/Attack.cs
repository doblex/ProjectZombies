using UnityEngine;
using UnityEngine.AI;

public class Attack : State
{
    float rotationSpeed = 2.0f;
    AudioSource shoot;

    public Attack(GameObject _npc, NavMeshAgent _agent, Animator _anim, BehaviourController _behaviour) : base(_npc, _agent, _anim, _behaviour)
    {
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
            nextState = STATE.CHASE;
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
