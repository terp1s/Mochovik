using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MandarinkyHole : MonoBehaviour, IObjectReceiver, IItemReceiver, INiche
{
    [SerializeField] private MandarinkyCapElevator elevator;
    public bool IsOccupied { get; private set; } = false;

   
    public bool CanReceiveItem(InventoryItem item)
    {
        return !IsOccupied;
    }

    public bool CanReceiveObject(WorldDraggable obj)
    {
        return !IsOccupied;
    }

    public bool CanRemoveObject()
    {
        return elevator.CanRemoveObject();
    }
    public void OnObjectRemoved()
    {
        IsOccupied = false;
    }

    public void OnReceiveItem(InventoryItem item)
    {
        GameObject spawnedObject = item.SpawnInWorld(new Vector3(0, 0, -0.01f), this.transform);
        IsOccupied = true;
        FitObject(spawnedObject);
    }

    public void OnReceiveObject(WorldDraggable obj)
    {
        obj.transform.SetParent(transform);
        obj.transform.localPosition = new Vector3(0, 0, -0.01f);
        FitObject(obj.gameObject);
        IsOccupied = true;
    }

    private void FitObject(GameObject obj)
    {
        Vector2 holeSize = GetComponent<SpriteRenderer>().bounds.size * 0.7f;

        obj.transform.localScale = Vector3.one; // Reset scale to 1 before fitting
        Vector2 objSize = obj.GetComponent<SpriteRenderer>().bounds.size;

        float scaleX = holeSize.x / objSize.x;
        float scaleY = holeSize.y / objSize.y;

        float scale = System.Math.Min(scaleX, scaleY);

        obj.transform.localScale = new Vector3(scale, scale, 1f);
    }
}
