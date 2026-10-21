using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MandarinkyHole : MonoBehaviour, IObjectReceiver, IItemReceiver, INiche
{
    [SerializeField] private MandarinkyCapElevator elevator;
    [SerializeField] private GameObject shadow;
    private GameObject obj;
    [SerializeField] private Transform placementPoint;
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
        SpriteRenderer holeSr = GetComponent<SpriteRenderer>();
        SpriteRenderer objSr = obj.GetComponentInChildren<SpriteRenderer>();

        if (holeSr == null || objSr == null) return;

        Vector2 targetSize = holeSr.bounds.size * 0.7f;

        Vector2 unscaledSpriteSize = objSr.sprite.rect.size / objSr.sprite.pixelsPerUnit;

        float targetWorldScale = Mathf.Min(targetSize.x / unscaledSpriteSize.x, targetSize.y / unscaledSpriteSize.y);

        Vector3 parentScale = obj.transform.parent != null ? obj.transform.parent.lossyScale : Vector3.one;
        obj.transform.localScale = new Vector3(
            targetWorldScale / parentScale.x,
            targetWorldScale / parentScale.y,
            1f
        );

        AlignCargo(obj, objSr);
    }

    private void AlignCargo(GameObject obj, SpriteRenderer sr)
    {
        Vector3 bottomCenter = new Vector3(
            sr.bounds.center.x,
            sr.bounds.min.y,
            sr.bounds.center.z
        );

        Vector3 offset = placementPoint.position - bottomCenter;
        obj.transform.position += offset;

        obj.transform.position = new Vector3(
            obj.transform.position.x,
            obj.transform.position.y,
            placementPoint.position.z - 0.01f
        );
    }
}
