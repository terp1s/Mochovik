using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MasonJar : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    [SerializeField] MasonJarItem inventoryJar;
    public float burnTime = 5f;
    public enum MasonJarState
    {
        Empty,
        Full
    }

    CowRoamingArea cowRoamingArea;
    int cowsInJar = 0;
    public MasonJarState currentState = MasonJarState.Empty;
    public bool IsFull => currentState == MasonJarState.Full;

    private void OnEnable()
    {
        GameEventsManager.instance.puzzleEvents.OnJarPlaced += PlacedJar;
    }

    private void OnDisable()
    {
        if (GameEventsManager.instance != null)
        {
            GameEventsManager.instance.puzzleEvents.OnJarPlaced -= PlacedJar;
        }
    }

    private void Awake()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        cowRoamingArea = GetComponentInChildren<CowRoamingArea>();

    }
    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag.TryGetComponent<Cow>(out Cow cow))
        {
            cow.Capture(transform, cowRoamingArea);
            cowsInJar++;

            if (cowsInJar >= 3)
            {
                currentState = MasonJarState.Full;
            }
        }
    }

    public void PlacedJar(string drop, MasonJar jar)
    {
        if (drop is not "mandarinky" || jar is null) { return; }
        if (jar.IsFull)
        {
            StartCoroutine(BurnCows());
        }
    }

    public IEnumerator BurnCows() 
    {
        yield return new WaitForSeconds(burnTime);
        //currentState = MasonJarState.Empty;
        cowsInJar = 0;
        Debug.Log("Cows burned!");
        GameEventsManager.instance.puzzleEvents.CowsBurned();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.dragging) return;
        if(currentState == MasonJarState.Full) { inventoryJar.isFilled = true; }
        else { inventoryJar.isFilled = false; }

        Inventory.Instance.AddItem(inventoryJar);
        Destroy(gameObject);
    }
}
