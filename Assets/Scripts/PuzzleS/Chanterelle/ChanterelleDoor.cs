using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChanterelleDoor : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] GameObject seedPrefab;
    [SerializeField] private GameObject masonJarPrefab; 

    [SerializeField] Sprite closedSprite;
    [SerializeField] Sprite openedSprite;

    [SerializeField] Tongue tongue;
    [SerializeField] GameObject hand;

    private SpriteRenderer spriteRenderer;
    private bool isOpenOrBusy = false;
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnEnable()
    {
        PuzzleManager.Instance.PuzzleCompleted += PuzzleFinished;
    }

    private void OnDisable()
    {
        PuzzleManager.Instance.PuzzleCompleted -= PuzzleFinished;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.dragging) return;
        Knock();
    }

    public void Knock()
    {
        if (isOpenOrBusy) return;

        Debug.Log("Player knocked on chanterelle door");

        StartCoroutine(OpenDoorRoutine());
    }

    private IEnumerator OpenDoorRoutine()
    {
        boxCollider.enabled = false;

        isOpenOrBusy = true;

        spriteRenderer.sprite = openedSprite;
        yield return new WaitForSeconds(0.5f);

        hand.SetActive(true);
        yield return new WaitForSeconds(0.5f);


        GameObject cargo = SpawnAppropriateCargo();

        tongue.gameObject.SetActive(true);
        tongue.TongueOut(cargo, this);
    }

    private GameObject SpawnAppropriateCargo()
    {
        if(tongue.IsOccupied) { return null; }

        if (IsCapPuzzleFinished() && !MasonJarAlreadyExists())
        {
            return Instantiate(masonJarPrefab);
        }

        if (!PopcornAlreadyExists())
        {
            return Instantiate(seedPrefab);
        }

        return null;
    }

    private void PuzzleFinished(string puzzle)
    {
        if(puzzle is "cap_puzzle")
        {
            tongue.RemoveCargo();
            StartCoroutine(OpenDoorRoutine());
        }
    }

    private bool IsCapPuzzleFinished()
    {
        return PuzzleManager.Instance != null && PuzzleManager.Instance.IsCompleted("cap_puzzle");
    }
    private bool PopcornAlreadyExists()
    {
        if (Popcorn.ExistsInWorld)
            return true;

        if (Inventory.Instance != null && Inventory.Instance.HasItem<PopcornItem>())
            return true;

        return false;
    }

    private bool MasonJarAlreadyExists()
    {
        if (MasonJar.ExistsInWorld)
            return true;

        if (Inventory.Instance != null && Inventory.Instance.HasItem<MasonJarItem>())
            return true;

        return false;
    }

    public void OnCargoTaken()
    {
        StartCoroutine(CloseDoorRoutine());
    }

    public void OnCargoExpired()
    {
        StartCoroutine(CloseDoorWithCargoRoutine());
    }

    private IEnumerator CloseDoorRoutine()
    {
        tongue.TongueIn();
        yield return new WaitForSeconds(1.5f);

        tongue.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);

        hand.SetActive(false);
        yield return new WaitForSeconds(0.5f);

        spriteRenderer.sprite = closedSprite;
        isOpenOrBusy = false;

        boxCollider.enabled = true;
    }

    private IEnumerator CloseDoorWithCargoRoutine()
    {
        tongue.TongueIn();
        yield return new WaitForSeconds(1.5f);

        if (tongue.Cargo != null)
        {
            Destroy(tongue.Cargo);
            tongue.ClearCargoReference();
        }

        tongue.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);

        hand.SetActive(false);
        yield return new WaitForSeconds(0.5f);

        spriteRenderer.sprite = closedSprite;
        isOpenOrBusy = false;

        boxCollider.enabled = true;
    }
}
