using UnityEngine;

public class BarrelMovement : MonoBehaviour
{
    public Transform player;
    public Transform castle;
    public float speed = 3f;
    public float detectionRange = 5f;

    private Transform currentTarget;
    private Animator animator;
    private BarrelHealth health;

    void Start()
    {
        animator = GetComponent<Animator>();
        health = GetComponent<BarrelHealth>();

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
            else Debug.LogError("❌ Không tìm thấy Player");
        }
        if (castle == null)
        {
            GameObject c = GameObject.FindGameObjectWithTag("Castle");
            if (c != null) castle = c.transform;
            else Debug.LogError("❌ Không tìm thấy Castle");
        }
    }

    void Update()
    {
        if (health != null && health.IsDead()) return;

        ChooseTarget();

        if (currentTarget != null)
        {
            float distance = Vector2.Distance(transform.position, currentTarget.position);
            if (distance > 0.1f)
            {
                transform.position = Vector2.MoveTowards(
                    transform.position,
                    currentTarget.position,
                    speed * Time.deltaTime
                );
                if (animator != null) animator.SetBool("isRunning", true);
            }
            else
            {
                if (animator != null) animator.SetBool("isRunning", false);
            }
        }
        else
        {
            if (animator != null) animator.SetBool("isRunning", false);
        }
    }

    void ChooseTarget()
    {
        if (player == null || castle == null) return;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        currentTarget = (distanceToPlayer <= detectionRange) ? player : castle;
    }
}