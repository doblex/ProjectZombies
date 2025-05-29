using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AI/States/GeneralAttack")]
public class MeleeAttackStateDefinition : AttackStateDefinition
{
    public override State CreateState(GameObject owner, NavMeshAgent agent, Animator animator, BehaviourController behaviour)
    {
        return new MeleeAttack(owner, agent, animator, behaviour);
    }
}


