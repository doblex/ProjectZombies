using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AI/States/Idle")]
public class IdleStateDefinition : StateDefinition
{
    [SerializeField] StateDefinition patrol;
    [SerializeField] StateDefinition chase;
    public override State CreateState(GameObject owner, NavMeshAgent agent, Animator animator)
    {
        return new Idle(owner, agent, animator, patrol, chase);
    }
}
