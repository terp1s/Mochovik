using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MandarinkyHole : MonoBehaviour, IObjectReceiver, IItemReceiver, INiche
{
    [SerializeField] private MandarinkyCapElevator elevator;
    [SerializeField] private GameObject shadow;
    private GameObject obj;
    public bool IsOccupied  => obj != null;

    private void Start()
    {
        if(transform.childCount > 0)
        {
            obj = transform.GetChild(0).gameObject;
            SetNicheForObject();
        }
       
    }
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
        shadow.SetActive(false);
    }

    public void OnObjectRemoved()
    {
        obj = null;
        shadow.SetActive(false);
    }

    public void OnObjectReturned()
    {
        obj.transform.SetParent(transform);
        obj.transform.localPosition = new Vector3(0, 0, -0.01f);
        shadow.SetActive(true);
        FitObject(obj);
    }

    public void OnReceiveItem(InventoryItem item)
    {
        obj = item.SpawnInWorld(new Vector3(0, 0, -0.01f), this.transform);
        SetNicheForObject();
        FitObject(obj);
        shadow.SetActive(true);
    }

    public void OnReceiveObject(WorldDraggable drop)
    {
        obj =  drop.gameObject;
        SetNicheForObject();
        obj.transform.SetParent(transform);
        obj.transform.localPosition = new Vector3(0, 0, -0.01f);
        FitObject(obj);
        shadow.SetActive(true);

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
