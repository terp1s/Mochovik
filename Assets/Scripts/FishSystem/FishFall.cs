using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FishFall : MonoBehaviour
{
    public Rigidbody2D rb;

    public void Fall()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

}
