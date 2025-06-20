﻿using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AI/States/Idle")]
public class IdleStateDefinition : StateDefinition
{
    public override State CreateState(GameObject owner, NavMeshAgent agent, Animator animator, BehaviourController behaviour)
    {
        return new Idle(owner, agent, animator, behaviour);
    }
}
