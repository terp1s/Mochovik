public interface IItemReceiver
{
    bool CanReceiveItem(InventoryItem item);
    void OnReceiveItem(InventoryItem item);
}