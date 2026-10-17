using UnityEngine;
using UnityEngine.EventSystems;

public class MandarinkyCap : MonoBehaviour, IPointerClickHandler
{
    enum MandarinkyCapState
    {
        Sealed,
        Cut,
    }

    MandarinkyCapState currentState = MandarinkyCapState.Sealed;

    [SerializeField] private Sprite openCap;
    [SerializeField] SpriteRenderer CanBack;
    [SerializeField] Sprite OpenCan;
    public bool IsSealed => currentState == MandarinkyCapState.Sealed;

    private MandarinkyCapElevator elevator;
    private SpriteRenderer spriteRenderer;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        elevator = GetComponent<MandarinkyCapElevator>();
        
    }

    private void Start()
    {
        currentState = MandarinkyCapState.Sealed;
        elevator.enabled = false;
        spriteRenderer.sprite = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(currentState == MandarinkyCapState.Cut) { return; }

        currentState = MandarinkyCapState.Cut;
        spriteRenderer.sprite = openCap;
        elevator.enabled = true;
        CanBack.sprite = OpenCan;
    }

}
