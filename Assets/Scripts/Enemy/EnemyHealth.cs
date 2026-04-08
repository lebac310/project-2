using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [Header("Health")]
    public int maxHealth = 3;
    [SerializeField] private int currentHealth;

    private EnemyMovement movement;
    private EnemyCombat combat;
    private Animator anim;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        movement = GetComponent<EnemyMovement>();
        combat = GetComponent<EnemyCombat>();
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;
        currentHealth -= damage;
        Debug.Log($"Enemy HP: {currentHealth} / {maxHealth}");
        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;
        Debug.Log("💀 Enemy chết (không animation)");

        // Tắt AI và combat
        if (movement != null) movement.enabled = false;
        if (combat != null) combat.enabled = false;

        // Tắt collider để không va chạm
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        // Hủy enemy sau 0.5 giây (có thể đổi thành 0 để biến mất ngay)
        Destroy(gameObject, 0.5f);
    }

    public bool IsDead() { return isDead; }
    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
}