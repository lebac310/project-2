using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    public int damage = 1;
    public Transform attackPoint;
    public float attackRange = 1.5f;
    public LayerMask playerLayer;

    void Start()
    {
        if (attackPoint == null)
            Debug.LogError("❌ EnemyCombat: attackPoint chưa được gán");
    }

    // Hàm này sẽ được gọi từ Animation Event
    public void Attack()
    {
        if (attackPoint == null) return;

        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);
        foreach (var hit in hits)
        {
            IDamageable damageable = hit.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage);
                Debug.Log($"Enemy tấn công {hit.name}, gây {damage} sát thương");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}