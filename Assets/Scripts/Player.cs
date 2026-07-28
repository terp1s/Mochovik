using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private MoveToPoint move;

    private void Awake()
    {
        move = GetComponent<MoveToPoint>();
        //move.enabled = false;
    }
    public void MoveToPoint(Vector2 target)
    {
        Debug.Log("player is walking");
        move.SetTarget(target);
    }

}
