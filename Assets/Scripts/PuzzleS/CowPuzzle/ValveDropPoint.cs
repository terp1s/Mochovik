using UnityEngine;

public class ValveDropPoint : MonoBehaviour, IItemReceiver
{
    public string requiredItemName = "ValveHandle";
    private bool hasReceivedItem = false;

    public bool CanReceiveItem(InventoryItem item)
    {
        return !hasReceivedItem && item is ValveHandleItem ;
    }

    public void OnReceiveItem(InventoryItem item)
    {
        Debug.Log("Valve handle placed!");
        hasReceivedItem = true;
        GameObject valve = item.SpawnInWorld(new Vector3(0, 0, -0.01f), this.transform);
        valve.GetComponent<ValveHandle>().Place();
    }
}
