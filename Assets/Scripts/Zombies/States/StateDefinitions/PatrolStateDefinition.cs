using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AI/States/Patrol")]
public class PatrolStateDefinition : StateDefinition
{
    public override State CreateState(GameObject owner, NavMeshAgent agent, Animator animator, BehaviourController behaviour)
    {
        return new Patrol(owner, agent, animator, behaviour);
    }
}


