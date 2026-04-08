using UnityEngine;

public class Elevation_Exit : MonoBehaviour
{
    public Collider2D[] mountainColliders;
    public Collider2D[] boundaryColliders;

    [Header("Cho phép các tag này xuống cầu thang")]
    public string[] allowedTags = { "Player" };

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsTagAllowed(collision.tag))
        {
            foreach (Collider2D mountain in mountainColliders)
                mountain.enabled = true;

            foreach (Collider2D boundary in boundaryColliders)
                boundary.enabled = false;
        }
    }

    private bool IsTagAllowed(string tag)
    {
        foreach (string allowed in allowedTags)
            if (allowed == tag) return true;
        return false;
    }
}