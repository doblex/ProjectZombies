using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AI/States/Reposition")]
public class RepositionStateDefinition : StateDefinition
{
    public override State CreateState(GameObject owner, NavMeshAgent agent, Animator animator, BehaviourController behaviour)
    {
        return new Reposition(owner, agent, animator, behaviour);
    }
}
