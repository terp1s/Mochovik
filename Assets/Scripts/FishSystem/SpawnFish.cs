using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFish : MonoBehaviour
{
    [SerializeField]
    private GameObject fishObject;

    public bool hasFish = false;
    private void OnMouseDown()
    {
        if (!hasFish)
        {
            GameObject go = Instantiate(
                fishObject,
                transform.position,
                transform.rotation,
                transform
            );
        }

        hasFish = true;
    }
}
