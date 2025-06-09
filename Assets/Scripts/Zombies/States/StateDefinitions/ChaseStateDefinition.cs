using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AI/States/Chase")]
public class ChaseStateDefinition : StateDefinition
{
    public override State CreateState(GameObject owner, NavMeshAgent agent, Animator animator, BehaviourController behaviour)
    {
        return new Chase(owner, agent, animator, behaviour);
    }
}


