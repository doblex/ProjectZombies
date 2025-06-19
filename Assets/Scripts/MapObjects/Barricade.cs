using UnityEngine;
using UnityEngine.UI;

public class Barricade : MonoBehaviour, IDamageable
{
    [Header("Barricade Settings")]
    [SerializeField] private float maxHealth = 100;
    [SerializeField][ReadOnly] private float currentHealth = 100;
    public float CurrentHealth
    {
        set
        {
            currentHealth = Mathf.Clamp(value, 0, maxHealth);
            UpdateVisuals();
        }
        get => currentHealth;
    }

    [System.Serializable]
    private struct BarricadeState
    {
        public Sprite sprite;

        [Tooltip("Value in percentage: from 0 to 1")]
        [Range(0, 1)] public float fromValue;
    }

    [Header("Barricade Visuals")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private BarricadeState HealedSprite;
    [SerializeField] private BarricadeState DamagedSprite;
    [SerializeField] private BarricadeState BrokenSprite;

    [Header("UI Elements")]
    [SerializeField] private SpriteRenderer interactRenderer;
    [SerializeField] private Image InteractButtonMK;
    [SerializeField] private Image InteractButtonGamepad;

    private void Start()
    {
        CurrentHealth = maxHealth;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void Heal(float amount)
    {
        CurrentHealth += amount;
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
    }

    private void UpdateVisuals()
    {
        float healthPercentage = currentHealth / maxHealth;
        if (healthPercentage <= 0)
        {
            spriteRenderer.sprite = BrokenSprite.sprite;
        }
        else if (healthPercentage < DamagedSprite.fromValue)
        {
            spriteRenderer.sprite = DamagedSprite.sprite;
        }
        else if (healthPercentage < HealedSprite.fromValue)
        {
            spriteRenderer.sprite = HealedSprite.sprite;
        }
        else
        {
            spriteRenderer.sprite = HealedSprite.sprite;
        }
    }
}
