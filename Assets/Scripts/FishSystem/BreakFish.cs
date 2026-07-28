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

    public ItemData OverriddenItem { get; private set; }
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

        BreakResult result = breakDatabase.GetBreakResult(stage, circumstance);

        if (result.brokenSprite != null)
        {
            IsBroken = true;
            spriteRenderer.sprite = result.brokenSprite;
            OverriddenItem = result.brokenItem; 

          
            CapsuleCollider2D col = GetComponent<CapsuleCollider2D>();
            SpriteColliderUtility.UpdateToSprite(col, spriteRenderer.sprite);
        }

      
    }
}
