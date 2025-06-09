using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AI/States/RangedAttack")]
public class RangedAttackStateDefinition : AttackStateDefinition
{
    public override State CreateState(GameObject owner, NavMeshAgent agent, Animator animator, BehaviourController behaviour)
    {
        return new RangedAttack(owner, agent, animator, behaviour);
    }
}


