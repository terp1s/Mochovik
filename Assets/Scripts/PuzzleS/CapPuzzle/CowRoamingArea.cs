using UnityEngine;

public class CowRoamingArea : MonoBehaviour
{
    public Bounds Bounds => GetComponent<BoxCollider2D>().bounds;

}
