using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance { get; private set; }

    public enum CursorType { Default, Hover, Grab, Click }

    [System.Serializable]
    public struct CursorData
    {
        public CursorType type;
        public Texture2D texture;
        public Vector2 hotspot;
    }

    [Header("Cursor Configurations")]
    [SerializeField] private CursorData defaultCursor;
    [SerializeField] private CursorData hoverCursor;
    [SerializeField] private CursorData grabCursor;
    [SerializeField] private CursorData clickCursor;

    [Header("Settings")]
    [SerializeField] private float clickFeedbackDuration = 0.1f;
    [SerializeField] private LayerMask interactableLayers = ~0;

    [Header("Scene-specific settings")]
    [SerializeField] private List<string> NoCursorScenes;

    private bool isDragging = false;
    private bool isClicking = false;
    private bool isHovering = false;

    private Camera mainCamera;
    private CursorType currentType = (CursorType)(-1);
    private Coroutine clickFeedbackRoutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        mainCamera = Camera.main;

       
        ApplyCursor(defaultCursor);
        
    }

    private bool IsCursorVisible()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        return !NoCursorScenes.Contains(sceneName);
    }


    public void SetDragging(bool dragging)
    {
        isDragging = dragging;
        if (isDragging && clickFeedbackRoutine != null)
        {
            StopCoroutine(clickFeedbackRoutine);
            isClicking = false;
        }

        UpdateCursorState();
    }

    private void Update()
    {
        if (isDragging) return;

        CheckHoverUnderCursor();

        if (isHovering && Input.GetMouseButtonDown(0))
        {
            TriggerClickPulse();
        }

        UpdateCursorState();
    }

    private void CheckHoverUnderCursor()
    {
        if (mainCamera == null) mainCamera = Camera.main;
        if (mainCamera == null) return;

        Vector2 mouseWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Collider2D[] hits = Physics2D.OverlapPointAll(mouseWorld, interactableLayers);

        isHovering = false;

        foreach (Collider2D hit in hits)
        {
            if (hit.GetComponentInParent<WorldDraggable>() != null ||
                hit.GetComponentInParent<IPointerClickHandler>() != null ||
                hit.GetComponent<InventoryItem>() != null)
            {
                isHovering = true;
                break;
            }


        }
    }

    private void TriggerClickPulse()
    {
        if (clickFeedbackRoutine != null)
            StopCoroutine(clickFeedbackRoutine);

        clickFeedbackRoutine = StartCoroutine(ClickPulseRoutine());
    }

    private IEnumerator ClickPulseRoutine()
    {
        isClicking = true;
        UpdateCursorState();

        yield return new WaitForSeconds(clickFeedbackDuration);

        isClicking = false;
        UpdateCursorState();
    }

    private void UpdateCursorState()
    {
        if (isDragging)
        {
            ApplyCursor(grabCursor);
        }
        else if (isClicking && clickCursor.texture != null)
        {
            ApplyCursor(clickCursor);
        }
        else if (isHovering)
        {
            ApplyCursor(hoverCursor);
        }
        else
        {
            ApplyCursor(defaultCursor);
        }
    }

    private void ApplyCursor(CursorData data)
    {
        if (!IsCursorVisible()) 
        { 
            Cursor.visible = false;
            return;
        }

        if (currentType == data.type) return;


        if (Cursor.visible == false) Cursor.visible = true;
       
        currentType = data.type;
        Cursor.SetCursor(data.texture, data.hotspot, CursorMode.Auto);
    }
}