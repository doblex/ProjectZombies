using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AI/States/Patrol")]
public class PatrolStateDefinition : StateDefinition
{
    [SerializeField] StateDefinition chase;
    public override State CreateState(GameObject owner, NavMeshAgent agent, Animator animator)
    {
        return new Patrol(owner, agent, animator, chase);
    }
}


