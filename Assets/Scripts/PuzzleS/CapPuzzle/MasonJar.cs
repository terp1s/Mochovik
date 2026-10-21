using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MasonJar : WorldDraggable, IObjectReceiver, IItemReceiver
{
    public static MasonJar Instance;
    public static bool ExistsInWorld => Instance != null;

    [SerializeField] GameObject CowPrefab;
    public float burnTime = 7f;


    CowRoamingArea cowRoamingArea;
    public int cowsInJar = 0;
    public bool IsFull => cowsInJar == 3;
    private bool isDraggable = true;
    private Animator glowAnimator;

    private new void Awake()
    {
        base.Awake();

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        cowRoamingArea = GetComponentInChildren<CowRoamingArea>();
        glowAnimator = GetComponentInChildren<Animator>();

    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
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
            cow.GetComponent<Cow>().Capture(transform, cowRoamingArea);
            cow.GetComponent<Cow>().IsInJar = true;
            cow.GetComponent<Transform>().localPosition = Vector3.zero;
            cow.GetComponent<FlyMovement>().pondLocation = transform;
        }
    }
    public IEnumerator BurnCows()
    {
        isDraggable = false;  // Disable dragging while burning


        glowAnimator.SetTrigger("FadeIn");

        yield return new WaitForSeconds(burnTime);

        glowAnimator.SetTrigger("FadeOut");

        if (CurrentNiche is MandarinkyHole niche && IsFull)
        {
            GameEventsManager.instance.puzzleEvents.CowsBurned();
        }

        Debug.Log("Cows burned!");

        isDraggable = true; // Re-enable dragging after burning
    }
    public bool CanReceiveObject(WorldDraggable obj)
    {
        if (obj.gameObject.TryGetComponent<Cow>(out Cow cow))
        {
            return true;
        }

        if (obj.gameObject.TryGetComponent<Popcorn>(out Popcorn pop) && pop.IsGrown)
        {
            return true;
        }

        return false;
    }

    public void OnReceiveObject(WorldDraggable obj)
    {
        if (obj.gameObject.TryGetComponent<Cow>(out Cow cow))
        {
            cow.Capture(transform, cowRoamingArea);
            cowsInJar++;
        }

        if (obj.gameObject.TryGetComponent<Popcorn>(out Popcorn pop))
        {
            RecievePopcorn(pop);
        }

    }

    private void RecievePopcorn(Popcorn pop)
    {
        Destroy(pop.gameObject);

        if (cowsInJar > 0)
        {
            // other objects need to be notified only when the jar is placed in mandarinky niche
            if (CurrentNiche is MandarinkyHole niche)
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

    public bool CanReceiveItem(InventoryItem item)
    {
        if (item.gameObject.TryGetComponent<PopcornItem>(out PopcornItem pop) && pop.IsGrown)
        {
            return true;
        }

        return false;
    }

    public void OnReceiveItem(InventoryItem item)
    {
        GameObject pop = item.SpawnInWorld(Vector3.zero, transform);

        RecievePopcorn(pop.GetComponent<Popcorn>());
    }
}
