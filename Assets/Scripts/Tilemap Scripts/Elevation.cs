using UnityEngine;

public class Elevation : MonoBehaviour
{
    public Collider2D[] mountainColliders;
    public Collider2D[] boundaryColliders;

    [Header("Cho phép các tag này lên cầu thang")]
    public string[] allowedTags = { "Player" };  // Mặc định có "Player", bạn thêm tag khác ở Inspector

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsTagAllowed(collision.tag))
        {
            foreach (Collider2D mountain in mountainColliders)
                mountain.enabled = false;

            foreach (Collider2D boundary in boundaryColliders)
                boundary.enabled = true;
        }
    }

    private bool IsTagAllowed(string tag)
    {
        foreach (string allowed in allowedTags)
            if (allowed == tag) return true;
        return false;
    }
}