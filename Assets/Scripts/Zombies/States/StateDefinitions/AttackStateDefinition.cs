using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AI/States/MeleeAttack")]
public class AttackStateDefinition : StateDefinition
{
    public override State CreateState(GameObject owner, NavMeshAgent agent, Animator animator, BehaviourController behaviour)
    {
        return new Attack(owner, agent, animator, behaviour);
    }
}


