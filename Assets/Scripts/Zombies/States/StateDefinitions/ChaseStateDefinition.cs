using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AI/States/Chase")]
public class ChaseStateDefinition : StateDefinition
{
    [SerializeField] StateDefinition patrol;
    [SerializeField] StateDefinition attack;
    public override State CreateState(GameObject owner, NavMeshAgent agent, Animator animator)
    {
        return new Chase(owner, agent, animator, patrol, attack);
    }
}


