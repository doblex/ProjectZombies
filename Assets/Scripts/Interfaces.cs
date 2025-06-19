

// DAMAGEABLE INTERFACE
public interface IDamageable
{
    void TakeDamage(float damage);
    void Heal(float amount);
}

// INTERACTABLE INTERFACE
public interface IInteractable
{
    void ShowInteract();
}