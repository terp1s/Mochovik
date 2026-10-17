using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MasonJar : WorldDraggable, IObjectReceiver, IPointerClickHandler
{
    [SerializeField] MasonJarItem inventoryJar;
    [SerializeField] GameObject CowPrefab;
    public float burnTime = 10f;


    CowRoamingArea cowRoamingArea;
    public int cowsInJar = 0;
    public bool IsFull => cowsInJar == 3;
    private bool isDraggable = true;

    private new void Awake()
    {
        base.Awake();
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        cowRoamingArea = GetComponentInChildren<CowRoamingArea>();

    }

    private void Start()
    {
        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
        for (int i = 0; i < cowsInJar; i++)
        {
            GameObject cow = Instantiate(CowPrefab, transform);
            cow.GetComponent<Cow>().Capture(this.transform, cowRoamingArea);
            cow.GetComponent<Cow>().IsInJar = true;
            cow.GetComponent<Transform>().localPosition = Vector3.zero;
            cow.GetComponent<FlyMovement>().pondLocation = this.transform;
        }
    }

    public void PlacedJar(string drop, MasonJar jar)
    {
        if (drop is not "mandarinky" || jar is null) { return; }
    }

    public IEnumerator BurnCows() 
    {
        isDraggable = false;  // Disable dragging while burning

        yield return new WaitForSeconds(burnTime);

        if (CurrentNiche is MandarinkyHole niche)
        {
            GameEventsManager.instance.puzzleEvents.CowsBurned();
        }

        Debug.Log("Cows burned!");

        isDraggable = true; // Re-enable dragging after burning
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.dragging) return;

        inventoryJar.numberOfCows = cowsInJar;

        Inventory.Instance.AddItem(inventoryJar);
        Destroy(gameObject);
    }

    public bool CanReceiveObject(WorldDraggable obj)
    {
        if (obj.gameObject.TryGetComponent<Cow>(out Cow cow))
        {
            return true;
        }

        if(obj.gameObject.TryGetComponent<Popcorn>(out Popcorn pop) && pop.IsGrown)
        {
            return true;
        }

        return false;
    }

    public void OnReceiveObject(WorldDraggable obj)
    {
        if(obj.gameObject.TryGetComponent<Cow>(out Cow cow))
        {
            cow.Capture(transform, cowRoamingArea);
            cowsInJar++;
        }

        if(obj.gameObject.TryGetComponent<Popcorn>(out Popcorn pop))
        {
            RecievePopcorn(pop);
        }

    }

    private void RecievePopcorn(Popcorn pop)
    {
        Destroy(pop.gameObject);

        if (IsFull)
        {

            // other objects need to be notified only when the jar is placed in mandarinky niche
            if(CurrentNiche is MandarinkyHole niche)
            {
                GameEventsManager.instance.puzzleEvents.CowsFedInMandarinky();
            }

            StartCoroutine(BurnCows());
        }
    }

    protected override bool CanStartDrag(PointerEventData eventData)
    {
        return isDraggable;
    }

    protected override void OnDragEnd()
    {
        Debug.Log("Drag ended for MasonJar");
    }

    protected override void OnDragStart()
    {
        Debug.Log("Drag started for MasonJar");
    }


}
