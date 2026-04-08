using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private PlayerCombat combat;

    private Vector2 move;
    public float speed = 5f;

    private int comboStep = 0;
    private float comboTimer = 0f;
    public float comboDelay = 1f;

    private bool isAttacking = false;
    private bool canCombo = false;
    private bool comboInput = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        combat = GetComponent<PlayerCombat>();
    }

    void Update()
    {
        move.x = Input.GetAxisRaw("Horizontal");
        move.y = Input.GetAxisRaw("Vertical");

        if (!isAttacking && anim != null)
            anim.SetBool("isRunning", move != Vector2.zero);

        if (!isAttacking)
        {
            comboTimer -= Time.deltaTime;
            if (comboTimer <= 0f)
            {
                comboStep = 0;
                comboInput = false;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (isAttacking)
            {
                if (canCombo) comboInput = true;
            }
            else
            {
                StartAttack();
            }
        }

        if (move.x != 0)
            transform.localScale = new Vector3(move.x > 0 ? 1 : -1, 1, 1);
    }

    void FixedUpdate()
    {
        if (!isAttacking)
            rb.linearVelocity = move.normalized * speed;
        else
            rb.linearVelocity = Vector2.zero;
    }

    void StartAttack()
    {
        isAttacking = true;
        canCombo = false;
        comboTimer = comboDelay;

        if (comboStep == 0)
        {
            if (anim != null) anim.SetTrigger("Attack1");
            comboStep = 1;
        }
        else
        {
            if (anim != null) anim.SetTrigger("Attack2");
            comboStep = 0;
        }
    }

    public void OnAttackHit()
    {
        if (combat != null)
            combat.PerformAttack();
    }

    public void EnableCombo() { canCombo = true; }

    public void EndAttack()
    {
        isAttacking = false;
        if (comboInput && canCombo)
        {
            comboInput = false;
            StartAttack();
        }
        else
        {
            comboInput = false;
            canCombo = false;
        }
    }
}