using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AI/States/Attack")]
public class AttackStateDefinition : StateDefinition
{
    [SerializeField] StateDefinition chase;
    public override State CreateState(GameObject owner, NavMeshAgent agent, Animator animator)
    {
        return new Attack(owner, agent, animator, chase);
    }
}


