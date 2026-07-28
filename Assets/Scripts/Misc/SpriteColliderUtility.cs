using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpriteColliderUtility
{
    public static void UpdateToSprite(this Collider2D collider, Sprite sprite)
    {
        if (sprite == null) return;

        if (collider is BoxCollider2D box)
        {
            box.size = sprite.bounds.size;
            box.offset = sprite.bounds.center;
        }
        else if (collider is CapsuleCollider2D capsule)
        {
            capsule.size = sprite.bounds.size;
            capsule.offset = sprite.bounds.center;
        }
        else if (collider is CircleCollider2D circle)
        {
            circle.radius = Mathf.Max(sprite.bounds.extents.x, sprite.bounds.extents.y);
            circle.offset = sprite.bounds.center;
        }
      
    }
}

