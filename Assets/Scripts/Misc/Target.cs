using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private Vector2 followSpot; //misto, kam panacek jde
    public float speed; 
    // Start is called before the first frame update
    void Start()
    {
        followSpot = transform.position; //na zacatku panacek zustane na miste
    }

    // Update is called once per frame
    void Update()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        

        if (Input.GetMouseButtonDown(0))
        {
            followSpot = new Vector2(mousePos.x, mousePos.y);
        }

        transform.position = Vector2.MoveTowards(transform.position, followSpot, Time.deltaTime * speed);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        followSpot = transform.position;
    }
}
