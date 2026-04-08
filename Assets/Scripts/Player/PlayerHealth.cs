using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Health")]
    public int maxHealth = 5;
    [SerializeField] private int currentHealth;   // 👈 hiển thị trong Inspector

    [Header("Invincibility")]
    public float invincibilityDuration = 1f;
    private float invincibilityTimer = 0f;

    private Animator anim;

    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (invincibilityTimer > 0)
            invincibilityTimer -= Time.deltaTime;
    }

    public void TakeDamage(int damage)
    {
        if (invincibilityTimer > 0) return;
        if (currentHealth <= 0) return;

        currentHealth -= damage;
        Debug.Log($"❤️ Player nhận {damage} sát thương, còn {currentHealth} HP");
        invincibilityTimer = invincibilityDuration;

        // Animation hurt (nếu có)
        if (anim != null && HasParameter(anim, "Hurt"))
            anim.SetTrigger("Hurt");

        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        Debug.Log("💀 Player chết");
        if (GameManager.instance != null)
            GameManager.instance.GameOver("Player died");
        else
            Time.timeScale = 0f;

        var controller = GetComponent<PlayerController>();
        if (controller != null) controller.enabled = false;

        var combat = GetComponent<PlayerCombat>();
        if (combat != null) combat.enabled = false;

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;
    }

    private bool HasParameter(Animator animator, string paramName)
    {
        if (animator == null) return false;
        foreach (AnimatorControllerParameter param in animator.parameters)
            if (param.name == paramName) return true;
        return false;
    }

    // 👈 Getter để UI lấy máu hiện tại
    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
}