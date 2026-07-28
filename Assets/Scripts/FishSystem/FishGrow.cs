using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FishGrow : MonoBehaviour
{
    [SerializeField] private GrowingSO fishData;
    private SpriteRenderer spriteRenderer;
    private CapsuleCollider2D capsuleCollider;
    private TimeEvent growEvent;
    private int currentStage = 0;

    public bool IsFullyGrown => currentStage >= fishData.MaxPhase;
    public int GetGrowthStage => currentStage;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider2D>(); 
    }
    public void StartGrowing() => Grow();

    private void Grow()
    {
        var stageInfo = fishData.growthStages[currentStage];
        spriteRenderer.sprite = stageInfo.sprite;
        
        currentStage++;

        SpriteColliderUtility.UpdateToSprite(capsuleCollider, spriteRenderer.sprite);

        if (currentStage < fishData.MaxPhase)
        {
            growEvent = TimeManager.Instance.Schedule(fishData.growthInterval, Grow);
        }
    }

    public void StopGrowing()
    {
        if (growEvent != null) growEvent.cancelled = true;
    }

    public ItemData GetCurrentItemData()
    {
        int index = Mathf.Clamp(currentStage - 1, 0, fishData.MaxPhase - 1);
        return fishData.growthStages[index].itemData;
    }

    public GrowingSO GetFishData()
    {
        return fishData;
    }
}
