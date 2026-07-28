using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour, IInteractable
{
    public FishFall faller;
    public FishGrow grower;
    public BreakFish breaker;
    public SpawnFish spawner;

    private bool isOnTree = true;

    private void Awake()
    {
        faller = GetComponent<FishFall>();
        grower = GetComponent<FishGrow>();
        breaker = GetComponent<BreakFish>();
        spawner = GetComponentInParent<SpawnFish>();
    }

    private void Start()
    {
        grower.StartGrowing();
    }

    public void Interact()
    {
        if (isOnTree)
        {
            isOnTree = false;
            grower.StopGrowing();
            faller.Fall();
        }
        else
        {
            if (TryGetComponent(out ICollectible collectible))
            {
                ItemData dataToCollect;

                if (breaker.IsBroken)
                {
                    dataToCollect = breaker.OverriddenItem;
                }
                else
                {
                    dataToCollect = grower.GetCurrentItemData();
                }

                collectible.Collect(dataToCollect);
                spawner.hasFish = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && grower.IsFullyGrown)
        {
            breaker.Break(BreakCircumstance.HitGround);
        }
    }
}