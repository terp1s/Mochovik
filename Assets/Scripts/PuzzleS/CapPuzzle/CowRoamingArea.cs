using UnityEngine;

public class CowRoamingArea : MonoBehaviour
{
    BoxCollider2D boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }
    public Bounds Bounds
    {
        get
        {
            if (boxCollider != null)
            {
                // World center taking into account collider offset
                Vector3 center = transform.TransformPoint(boxCollider.offset);
                // World size taking into account scale
                Vector3 size = Vector3.Scale(boxCollider.size, transform.lossyScale);
                return new Bounds(center, size);
            }

            return new Bounds(transform.position, Vector3.one);
        }
    }
}
