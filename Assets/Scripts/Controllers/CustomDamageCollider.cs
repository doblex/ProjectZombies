using UnityEngine;
using utilities.Controllers;

public class CustomDamageCollider : DamageColliderController
{
    [SerializeField] BehaviourController behaviourController;
    private void Awake()
    {
        damage = behaviourController.AttackDamage;
    }
}
