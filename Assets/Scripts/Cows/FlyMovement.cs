
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class FlyMovement : MonoBehaviour, IPointerClickHandler
{
    public enum CowState { Wandering, GoingToPond, Drinking, Fleeing }
    public CowState currentState = CowState.Wandering;

    [Header("Movement Settings")]
    public float wanderSpeed = 1.0f;
    public float maxFleeSpeed = 6.0f;
    public float acceleration = 30.0f; 
    public float deceleration = 30.0f; 
    public float fleeBurstDuration = 0.25f; 

    [Header("Visuals")]
    public float vibrationIntensity = 0.2f;

    [Header("Enclosure & Pond")]
    public Vector2 minBounds = new Vector2(-7, 1);
    public Vector2 maxBounds = new Vector2(-1, 4);
    public Vector3 pondLocation = new Vector3(-5, -1, 0);
    public float drinkDuration = 5.0f;
    [Range(0, 100)] public float chanceToVisitPond = 10f;

    private Vector3 targetPosition;
    private Vector3 basePosition;

    private float currentSpeed;
    private float targetSpeed;
    private float fleeTimer;

    void Start()
    {
        basePosition = transform.position;
        currentSpeed = wanderSpeed;
        targetSpeed = wanderSpeed;
        PickNewTarget();
    }

    void Update()
    {
        HandleSpeedLogic();

        if (currentState != CowState.Drinking)
        {
            basePosition = Vector3.MoveTowards(basePosition, targetPosition, Time.deltaTime * currentSpeed);
        }

        if (Vector3.Distance(basePosition, targetPosition) < 0.1f)
        {
            if (currentState == CowState.GoingToPond) StartCoroutine(DrinkAtPond());
            else if (currentState != CowState.Drinking) PickNewTarget();
        }

        ApplyVisuals();
    }

    void HandleSpeedLogic()
    {
        if (currentState == CowState.Fleeing)
        {
            fleeTimer -= Time.deltaTime;
            if (fleeTimer <= 0)
            {
                targetSpeed = wanderSpeed;
                if (currentSpeed <= wanderSpeed + 0.1f)
                {
                    currentState = CowState.Wandering;
                }
            }
        }

      
        float rate = (currentSpeed < targetSpeed) ? acceleration : deceleration;
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, Time.deltaTime * rate);
    }

    void ApplyVisuals()
    {
        float jitterX = 0, jitterY = 0;
        if (currentState != CowState.Drinking)
        {
            jitterX = (Mathf.PerlinNoise(Time.time * 5, 0) - 0.5f) * vibrationIntensity;
            jitterY = (Mathf.PerlinNoise(0, Time.time * 5) - 0.5f) * vibrationIntensity;
            jitterY += Mathf.Sin(Time.time * 3) * (vibrationIntensity * 0.5f);
        }
        transform.position = basePosition + new Vector3(jitterX, jitterY, 0);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.dragging) return;
        Flee(eventData.pressPosition);
    }

    public void Flee(Vector2 clickSource)
    {
        StopAllCoroutines();
        currentState = CowState.Fleeing;

        targetSpeed = maxFleeSpeed;
        fleeTimer = fleeBurstDuration;

        Vector3 clickWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(clickSource.x, clickSource.y, Mathf.Abs(Camera.main.transform.position.z)));
        Vector3 fleeDir = (transform.position - clickWorldPos).normalized;

        targetPosition = transform.position + fleeDir * 5.0f;
        targetPosition.x = Mathf.Clamp(targetPosition.x, minBounds.x, maxBounds.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minBounds.y, maxBounds.y);
    }

    void PickNewTarget()
    {
        targetSpeed = wanderSpeed;
        if (Random.Range(0, 100) < chanceToVisitPond)
        {
            currentState = CowState.GoingToPond;
            targetPosition = pondLocation;
        }
        else
        {
            currentState = CowState.Wandering;
            targetPosition = new Vector2(Random.Range(minBounds.x, maxBounds.x), Random.Range(minBounds.y, maxBounds.y));
        }
    }

    IEnumerator DrinkAtPond()
    {
        currentState = CowState.Drinking;
        basePosition = targetPosition;
        yield return new WaitForSeconds(drinkDuration);
        if (currentState == CowState.Drinking) PickNewTarget();
    }
}