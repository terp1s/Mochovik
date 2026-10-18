using UnityEngine;
using UnityEngine.EventSystems;

public interface IItemReceiver
{
    bool CanReceiveItem(InventoryItem item);
    void OnReceiveItem(InventoryItem item);
}

public interface IObjectReceiver 
{
    bool CanReceiveObject(WorldDraggable obj);
    void OnReceiveObject(WorldDraggable obj);
}

public interface INiche : IObjectReceiver
{
    bool CanRemoveObject();
    void OnObjectRemoved();
    void OnObjectHeld();
    void OnObjectReturned();
}