using UnityEngine;

public class ValveDropPoint : MonoBehaviour, IItemReceiver, IObjectReceiver
{
    public string requiredItemName = "ValveHandle";
    private bool hasReceivedItem = false;

    public bool CanReceiveItem(InventoryItem item)
    {
        return !hasReceivedItem && item is ValveHandleItem ;
    }

    public bool CanReceiveObject(WorldDraggable obj)
    {
        return !hasReceivedItem  && obj is ValveHandle;
    }

    public void OnReceiveItem(InventoryItem item)
    {
        Debug.Log("Valve handle placed!");
        hasReceivedItem = true;
        GameObject valve = item.SpawnInWorld(new Vector3(0, 0, -0.01f), this.transform);
        valve.GetComponent<ValveHandle>().Place();
    }

    public void OnReceiveObject(WorldDraggable obj)
    {
        Debug.Log("Valve handle placed!");
        hasReceivedItem = true;
        obj.transform.SetParent(this.transform);
        obj.transform.localPosition = new Vector3(0, 0, -0.01f);
        obj.GetComponent<ValveHandle>().Place();
    }
}
