using UnityEngine;

public class PlayerOrderToggleEachPass : MonoBehaviour
{
    [Header("Order Values (set in Inspector)")]
    public int orderA;   // Ví dụ: 9
    public int orderB;   // Ví dụ: 10

    public string playerTag = "Player";

    private bool isOrderA = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        SpriteRenderer sr = other.GetComponent<SpriteRenderer>();
        if (sr == null) return;

        int newOrder = isOrderA ? orderB : orderA;
        sr.sortingOrder = newOrder;
        Debug.Log($"Player order toggled to {sr.sortingOrder}");

        isOrderA = !isOrderA;
    }
}