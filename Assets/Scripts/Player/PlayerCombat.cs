using UnityEngine;
using System.Collections.Generic;

public class PlayerCombat : MonoBehaviour
{
    public int damage = 1;
    public Transform attackPoint;
    public float attackRange = 1f;
    public LayerMask targetLayers;

    void Start()
    {
        if (attackPoint == null)
        {
            GameObject go = new GameObject("AttackPoint");
            go.transform.SetParent(transform);
            go.transform.localPosition = new Vector3(0.8f, 0, 0);
            attackPoint = go.transform;
        }

        if (targetLayers == 0)
        {
            int enemyLayer = LayerMask.NameToLayer("Enemy");
            int barrelLayer = LayerMask.NameToLayer("Barrel");
            if (enemyLayer != -1) targetLayers |= (1 << enemyLayer);
            if (barrelLayer != -1) targetLayers |= (1 << barrelLayer);
        }
    }

    public void PerformAttack()
    {
        if (attackPoint == null) return;

        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, targetLayers);
        HashSet<GameObject> damaged = new HashSet<GameObject>();

        foreach (var hit in hits)
        {
            if (hit.gameObject == gameObject) continue;
            if (damaged.Contains(hit.gameObject)) continue;

            IDamageable d = hit.GetComponent<IDamageable>();
            if (d != null)
            {
                d.TakeDamage(damage);
                damaged.Add(hit.gameObject);
                Debug.Log($"⚔️ Đánh trúng {hit.name}");
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}