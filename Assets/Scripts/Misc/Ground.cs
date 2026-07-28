using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour, IWalkable
{
    public Player player;
    public void OnWalkTo(Vector2 target)
    {
        player.MoveToPoint(target);
    }
}
