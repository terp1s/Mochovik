using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MasonJarItem : InventoryItem
{
    public bool isFilled = false;
    
    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);

        if (!wasPlaced)
        {
            Inventory.Instance.AddItem(this);
        }

        Destroy(gameObject);

    }

    public override GameObject SpawnInWorld(Vector3 position, Transform parent)
    {
        wasPlaced = true;
        GameObject spawnedObject = Instantiate(WorldPrefab, parent);
        spawnedObject.transform.localPosition = position;
        MasonJar masonJar = spawnedObject.GetComponent<MasonJar>();
        masonJar.currentState = isFilled ? MasonJar.MasonJarState.Full : MasonJar.MasonJarState.Empty;

        return spawnedObject;
    }
}
