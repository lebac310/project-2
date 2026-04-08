using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 3f;
    public float detectionRange = 5f;
    public float attackRange = 2f;

    [Header("Attack Cooldown")]
    public float attackCooldown = 1.5f;
    private float attackCooldownTimer = 0f;

    [Header("References")]
    public Transform detectionPoint;
    public LayerMask playerLayer;

    private Rigidbody2D rb;
    private Transform player;
    private Animator anim;
    private EnemyCombat combat;
    private EnemyHealth health;

    private int facingDirection = 1;
    private EnemyState enemyState = EnemyState.Idle;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        combat = GetComponent<EnemyCombat>();
        health = GetComponent<EnemyHealth>();

        if (rb == null) Debug.LogError("❌ Enemy cần Rigidbody2D");
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
        else Debug.LogError("❌ Không tìm thấy Player");

        if (detectionPoint == null) detectionPoint = transform;

        facingDirection = (transform.localScale.x > 0) ? 1 : -1;
        ChangeState(EnemyState.Idle);
    }

    void Update()
    {
        if (health != null && health.IsDead()) return;

        if (attackCooldownTimer > 0)
            attackCooldownTimer -= Time.deltaTime;

        CheckForPlayer();

        switch (enemyState)
        {
            case EnemyState.Chasing:
                Chase();
                break;
            case EnemyState.Attacking:
                rb.linearVelocity = Vector2.zero;
                break;
            case EnemyState.Idle:
                rb.linearVelocity = Vector2.zero;
                break;
        }
    }

    void CheckForPlayer()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        Collider2D[] hits = Physics2D.OverlapCircleAll(detectionPoint.position, detectionRange, playerLayer);
        bool playerDetected = (hits.Length > 0);

        if (!playerDetected)
        {
            if (enemyState != EnemyState.Idle) ChangeState(EnemyState.Idle);
            return;
        }

        player = hits[0].transform;

        if (distance <= attackRange)
        {
            if (attackCooldownTimer <= 0f && enemyState != EnemyState.Attacking)
            {
                ChangeState(EnemyState.Attacking);
                PerformAttack(); // chỉ trigger animation, không gây damage ngay
            }
            else if (enemyState != EnemyState.Idle && enemyState != EnemyState.Attacking)
            {
                ChangeState(EnemyState.Idle);
            }
        }
        else
        {
            if (enemyState != EnemyState.Chasing) ChangeState(EnemyState.Chasing);
        }
    }

    void Chase()
    {
        if (player == null) return;

        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * speed;

        if ((player.position.x > transform.position.x && facingDirection != 1) ||
            (player.position.x < transform.position.x && facingDirection != -1))
        {
            Flip();
        }
    }

    void PerformAttack()
    {
        attackCooldownTimer = attackCooldown;

        // Chỉ kích hoạt trigger animation, damage sẽ được gọi từ Animation Event
        if (anim != null && HasParameter(anim, "isAttack"))
        {
            anim.SetTrigger("isAttack");
            Debug.Log("🎬 Enemy attack animation triggered");
        }

        // Không gọi combat.Attack() ở đây nữa
    }

    // Hàm này sẽ được gọi từ Animation Event (gắn trong clip Attack_Enemy)
    public void OnAttackHit()
    {
        if (combat != null && enemyState == EnemyState.Attacking)
        {
            combat.Attack();
            Debug.Log("💥 Enemy gây sát thương từ Animation Event");
        }
    }

    // Kết thúc animation attack (có thể gọi từ Animation Event ở cuối clip)
    public void EndAttackFromAnimation()
    {
        if (enemyState == EnemyState.Attacking)
            ChangeState(EnemyState.Chasing);
    }

    void Flip()
    {
        facingDirection *= -1;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void ChangeState(EnemyState newState)
    {
        if (enemyState == newState) return;

        if (anim != null)
        {
            if (HasParameter(anim, "isIdle"))
                anim.SetBool("isIdle", (newState == EnemyState.Idle));

            if (HasParameter(anim, "isMoving"))
                anim.SetBool("isMoving", (newState == EnemyState.Chasing));
        }

        enemyState = newState;
        Debug.Log($"Enemy state changed to: {newState}");
    }

    private bool HasParameter(Animator animator, string paramName)
    {
        if (animator == null) return false;
        foreach (AnimatorControllerParameter param in animator.parameters)
            if (param.name == paramName) return true;
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        if (detectionPoint == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(detectionPoint.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}

public enum EnemyState
{
    Idle,
    Chasing,
    Attacking
}