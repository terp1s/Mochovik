using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MandarinkyHole : MonoBehaviour, IObjectReceiver, IItemReceiver, INiche
{
    [SerializeField] private MandarinkyCapElevator elevator;
    private GameObject obj;
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

    public void OnObjectHeld()
    {
    }

    public void OnObjectRemoved()
    {
        IsOccupied = false;
    }

    public void OnObjectReturned()
    {
        obj.transform.SetParent(transform);
        obj.transform.localPosition = new Vector3(0, 0, -0.01f);
        FitObject(obj);
        IsOccupied = true;
    }

    public void OnReceiveItem(InventoryItem item)
    {
        obj = item.SpawnInWorld(new Vector3(0, 0, -0.01f), this.transform);
        SetNicheForObject();
        IsOccupied = true;
        FitObject(obj);
    }

    public void OnReceiveObject(WorldDraggable drop)
    {
        obj =  drop.gameObject;
        SetNicheForObject();
        obj.transform.SetParent(transform);
        obj.transform.localPosition = new Vector3(0, 0, -0.01f);
        FitObject(obj);
        IsOccupied = true;
    }

    private void SetNicheForObject()
    {
        if (obj.TryGetComponent(out WorldDraggable draggable))
        {
            draggable.SetNiche(this);
        }
        if (obj.TryGetComponent(out CollectOnClick collectible))
        {
            collectible.CurrentNiche = this;
        }
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
