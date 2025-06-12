using System;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class State
{
    public STATE stateName;
    public EVENT stage;

    protected GameObject npc;
    protected Animator anim;
    protected PlayerInfo playerInfo;
    protected STATE nextState;
    protected NavMeshAgent agent;
    protected BehaviourController parent;

    public PlayerInfo PlayerInfo { get => playerInfo; }

    public State(GameObject _npc, NavMeshAgent _agent, Animator _anim, BehaviourController _behaviuor)
    {
        npc = _npc;
        agent = _agent;
        anim = _anim;
        parent = _behaviuor;
        stage = EVENT.ENTER;
    }

    public virtual void Enter() 
    {
        stage = EVENT.UPDATE;

        AIManager.OnPlayerInfo += GetPlayerInfo;
    }
    public virtual void Update() { stage = EVENT.UPDATE; }
    public virtual void Exit()
    {
        stage = EVENT.EXIT;

        AIManager.OnPlayerInfo -= GetPlayerInfo;
    }

    public STATE Process()
    {
        if (stage == EVENT.ENTER) Enter();
        else if(stage == EVENT.UPDATE) Update();
        else if(stage == EVENT.EXIT)
        {
            Exit();
            return nextState;
        }

        return stateName;
    }

    void GetPlayerInfo(PlayerInfo _playerInfo)
    {
        playerInfo = _playerInfo;
    }

    public bool CanSeePlayer()
    {
        Vector3 direction = playerInfo.currentPosition - npc.transform.position;
        float angle = Vector3.Angle(direction, npc.transform.forward);

        if(direction.magnitude < parent.VisionDistance && angle < parent.VisionAngle)
        {
            return true;
        }

        return false;
    }


    public COMBATRANGE CanAttack()
    {
        Vector3 direction = playerInfo.currentPosition - npc.transform.position;

        if (direction.magnitude > parent.AttackDistance + 1)
        {
            return COMBATRANGE.FAR;
        }
        else if (direction.magnitude < parent.MinimumRange)
        {
            return COMBATRANGE.CLOSE;
        }
        else
        {
            return COMBATRANGE.RANGE;
        }
    }

    public bool IsPlayerBehind()
    {
        Vector3 direction = npc.transform.position - playerInfo.currentPosition;
        float angle = Vector3.Angle(direction, npc.transform.forward);

        float dotProduct = Vector3.Dot(npc.transform.forward, playerInfo.forwardVector);
        
        
        if (direction.magnitude < parent.AttackDistance && angle < 30)
        {
            return true;
        }

        return false;
    }

    protected void Seek(Vector3 position)
    {
        agent.SetDestination(position);

        Debug.DrawLine(agent.transform.position + Vector3.up, position + Vector3.up, Color.red);
    }

    protected void Flee(Vector3 position)
    {
        Vector3 fleeVector = position - agent.transform.position;
        agent.SetDestination(agent.transform.position - fleeVector);
    }
}

