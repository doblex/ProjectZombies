using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AI/States/Attack")]
public class AttackStateDefinition : StateDefinition
{
    public override State CreateState(GameObject owner, NavMeshAgent agent, Animator animator, BehaviourController behaviour)
    {
        return new Attack(owner, agent, animator, behaviour);
    }
}


