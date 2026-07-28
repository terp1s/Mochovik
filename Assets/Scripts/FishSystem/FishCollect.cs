using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FishCollect : MonoBehaviour, ICollectible
{

    public void Collect(ItemData inventoryFish)
    {
        Inventory.Instance.AddItem(inventoryFish);
        Destroy(gameObject);
    }

}
