using UnityEngine;

public class BarrelCombat : MonoBehaviour
{
    [Header("Explosion")]
    public float explosionRadius = 1.5f;
    public int damage = 1;
    public float explosionDelay = 1.2f;        // Delay hủy object sau khi nổ
    public float triggerExplosionDelay = 0.5f; // Delay từ lúc chạm đến lúc nổ

    private Animator animator;
    private bool exploded = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
            Debug.LogError("❌ Thiếu Animator trên Barrel");
    }

    public void Explode()
    {
        if (exploded) return;
        exploded = true;

        Debug.Log("💥 EXPLODE");

        if (animator != null)
        {
            animator.SetTrigger("explode");
            animator.SetBool("isRunning", false);
        }

        BarrelHealth.isExploding = true;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (var hit in hits)
        {
            IDamageable damageable = hit.GetComponent<IDamageable>();
            if (damageable != null)
                damageable.TakeDamage(damage);

            if (hit.CompareTag("Castle"))
            {
                Debug.Log("🏰 Castle trúng nổ → Game Over");
                if (GameManager.instance != null)
                    GameManager.instance.GameOver("Castle exploded");
                else
                    Time.timeScale = 0f;
            }
        }

        BarrelHealth.isExploding = false;

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        Destroy(gameObject, explosionDelay);
    }

    public void TriggerExplodeWithDelay()
    {
        if (exploded) return;
        Invoke(nameof(Explode), triggerExplosionDelay);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (exploded) return;
        if (other.CompareTag("Player") || other.CompareTag("Castle"))
        {
            TriggerExplodeWithDelay();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}