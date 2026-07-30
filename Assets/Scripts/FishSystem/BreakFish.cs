/*
 * using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BreakFish : MonoBehaviour
{
    public List<FishVariation> BrokenFish;
    private SpriteRenderer spriteRenderer;
    public Collider2D normalCollider; 
    public BoxCollider2D brokenCollider; 

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Break()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = BrokenFish[0].sprite;

            normalCollider.enabled = false;
            brokenCollider.enabled = true;

            SpriteColliderUtility.UpdateToSprite(brokenCollider, spriteRenderer.sprite);
        }
    }
}
*/



using UnityEngine;

public enum BreakCircumstance
{
    HitGround
}


public class BreakFish : MonoBehaviour
{
    public FishBreakDatabaseSO breakDatabase;
    private SpriteRenderer spriteRenderer;
    private FishGrow grower;

    public FishItem OverriddenItem { get; private set; }
    public bool IsBroken { get; private set; }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        grower = GetComponent<FishGrow>();
    }

    public void Break(BreakCircumstance circumstance)
    {
        if (IsBroken) return;

        int stage = grower.GetGrowthStage;

        FishItem result = breakDatabase.GetBreakResult(stage, circumstance);

        if (result.Sprite != null)
        {
            IsBroken = true;
            spriteRenderer.sprite = result.Sprite;
            OverriddenItem = result; 
          
            CapsuleCollider2D col = GetComponent<CapsuleCollider2D>();
            SpriteColliderUtility.UpdateToSprite(col, spriteRenderer.sprite);
        }

      
    }
}
