using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FishCollect : MonoBehaviour
{
    public void Collect(FishItem inventoryFish)
    {
        Inventory.Instance.AddItem(inventoryFish);
        Destroy(gameObject);
    }

}
