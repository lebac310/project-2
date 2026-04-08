using UnityEngine;

public class BarrelHealth : MonoBehaviour, IDamageable
{
    public int maxHP = 3;
    [SerializeField] private int currentHP;

    private bool isDead = false;
    private BarrelCombat combat;

    public static bool isExploding = false;

    void Start()
    {
        currentHP = maxHP;
        combat = GetComponent<BarrelCombat>();
        if (combat == null)
            Debug.LogError("❌ BarrelHealth cần BarrelCombat trên cùng object");
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;
        currentHP -= damage;
        Debug.Log($"Barrel HP: {currentHP} / {maxHP}");

        if (currentHP <= 0)
        {
            Die(isExploding);
        }
    }

    void Die(bool causedByExplosion)
    {
        if (isDead) return;
        isDead = true;

        if (causedByExplosion)
        {
            Debug.Log("💥 Barrel nổ dây chuyền");
            if (combat != null) combat.Explode();
        }
        else
        {
            Debug.Log("🔨 Barrel bị đập vỡ, không nổ");
            // Hủy mọi lệnh nổ đã được lên lịch (từ BarrelCombat)
            CancelInvoke();                      // Hủy trên chính BarrelHealth
            if (combat != null) combat.CancelInvoke(); // Hủy trên BarrelCombat nếu có
            Collider2D col = GetComponent<Collider2D>();
            if (col != null) col.enabled = false;
            Destroy(gameObject, 0.2f);
        }
    }

    public bool IsDead() => isDead;
    public int GetCurrentHealth() => currentHP;
    public int GetMaxHealth() => maxHP;
}