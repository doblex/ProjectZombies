﻿using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AI/States/Death")]
public class DeathStateDefinition : StateDefinition
{
    public override State CreateState(GameObject owner, NavMeshAgent agent, Animator animator, BehaviourController behaviour)
    {
        return new Death(owner, agent, animator, behaviour);
    }
}


