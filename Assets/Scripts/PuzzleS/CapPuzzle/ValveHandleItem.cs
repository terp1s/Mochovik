using UnityEngine;

public class ValveHandleItem : InventoryItem
{
    public override GameObject SpawnInWorld(Vector3 position, Transform parent)
    {
        GameObject valve = base.SpawnInWorld(position, parent);

        valve.GetComponent<CollectOnClick>().enabled = false;
        valve.GetComponent<ValveHandle>().enabled = true;
        valve.GetComponent<Collider2D>().enabled = true;
        valve.GetComponent<ValveHandle>().UpdateColliderSize();
        return valve;
    }
}
