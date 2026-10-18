using UnityEngine;
using UnityEngine.EventSystems;

public class Puddle : MonoBehaviour, IObjectReceiver, IItemReceiver
{
    public Transform spawnPoint;
    public bool IsSpilled { get; private set; } = false;
    public Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        GameEventsManager.instance.puzzleEvents.OnValveHandleTurned += Spill;
    }

    private void OnDisable()
    {

        GameEventsManager.instance.puzzleEvents.OnValveHandleTurned -= Spill;

    }

    public bool CanReceiveObject(WorldDraggable obj)
    {
        return obj.gameObject.TryGetComponent<Popcorn>(out Popcorn pop) && IsSpilled;
    }

    public void OnReceiveObject(WorldDraggable obj)
    {
        Popcorn pop = obj.GetComponent<Popcorn>();

        if (pop != null)
        {
            pop.transform.SetParent(transform);
            pop.Grow();
            pop.transform.localPosition = spawnPoint.localPosition;
        }
    }

    public void Spill()
    {
        animator.SetTrigger("Spill");
    }

    public void HasSpilled()
    {
        IsSpilled = true;
    }

    public bool CanReceiveItem(InventoryItem item)
    {
        return item.gameObject.TryGetComponent<PopcornItem>(out PopcornItem pop) && IsSpilled;
    }

    public void OnReceiveItem(InventoryItem item)
    {
        Popcorn pop = item.SpawnInWorld(spawnPoint.localPosition, transform).GetComponent<Popcorn>();

        if (pop != null)
        {
            pop.Grow();
        }
    }
}
