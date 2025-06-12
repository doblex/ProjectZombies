using UnityEngine;
using UnityEngine.Animations;

public abstract class  OnAttack : StateMachineBehaviour
{
    [SerializeField] float timing;
    protected BehaviourController parent;


    float timer;
    bool hasAttacked = false;

    protected abstract void PerformAttack();


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        timer = 0f;
        hasAttacked = false;
        parent = animator.GetComponentInParent<BehaviourController>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        timer += Time.deltaTime;
        if (timer >= timing && !hasAttacked)
        {
            PerformAttack();
            timer = 0f;
            hasAttacked = true;
        }
    }

    protected Vector3 CalculateVelocity(Vector3 target, Vector3 origin, float speed)
    {
        Vector3 toTarget = target - origin;
        float yOffset = toTarget.y;
        toTarget.y = 0;

        float distance = toTarget.magnitude;
        float angle = 45f * Mathf.Deg2Rad;

        float yVel = Mathf.Sin(angle) * speed;
        float xzVel = Mathf.Cos(angle) * speed;

        Vector3 direction = toTarget.normalized * xzVel;
        direction.y = yVel;

        return direction;
    }
}
