using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPoint : MonoBehaviour
{
    [SerializeField] Vector2 target;
    [SerializeField] int speed;

    void Update()
    {
        if(target != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, Time.deltaTime * speed);
        }
    }

    public void SetTarget(Vector2 targ)
    {
        target = targ;
    }
}
