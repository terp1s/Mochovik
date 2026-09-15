
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class FlyMovement : MonoBehaviour
{
    public Cow cow;

    [Header("Movement Settings")]
    public float wanderSpeed = 1.0f;
    public float maxFleeSpeed = 6.0f;
    public float acceleration = 30.0f; 
    public float deceleration = 30.0f; 
    public float fleeBurstDuration = 0.25f; 

    [Header("Visuals")]
    public float vibrationIntensity = 0.2f;

    [Header("Enclosure & Pond")]
    public CowRoamingArea roamingArea;
    public Vector3 pondLocation = new Vector3(-5, -1, 0);
    public float drinkDuration = 5.0f;
    [Range(0, 100)] public float chanceToVisitPond = 10f;

    private Vector3 targetPosition;
    private Vector3 basePosition;

    private float currentSpeed;
    private float targetSpeed;
    private float fleeTimer;

    void Awake()
    {
        cow = GetComponent<Cow>();
    }

    private void OnEnable()
    {
        basePosition = transform.position;
        currentSpeed = wanderSpeed;
        targetSpeed = wanderSpeed;

        PickNewTarget();

    }

    void Update()
    {
        HandleSpeedLogic();

        if (cow.currentState != CowState.Drinking)
        {
            basePosition = Vector3.MoveTowards(basePosition, targetPosition, Time.deltaTime * currentSpeed);
        }

        if (Vector3.Distance(basePosition, targetPosition) < 0.1f)
        {
            if (cow.currentState == CowState.GoingToPond) StartCoroutine(DrinkAtPond());
            else if (cow.currentState != CowState.Drinking) PickNewTarget();
        }

        ApplyVisuals();
    }

    void HandleSpeedLogic()
    {
        if (cow.currentState == CowState.Fleeing)
        {
            fleeTimer -= Time.deltaTime;
            if (fleeTimer <= 0)
            {
                targetSpeed = wanderSpeed;
                if (currentSpeed <= wanderSpeed + 0.1f)
                {
                    cow.currentState = CowState.Wandering;
                }
            }
        }

      
        float rate = (currentSpeed < targetSpeed) ? acceleration : deceleration;
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, Time.deltaTime * rate);
    }

    void ApplyVisuals()
    {
        float jitterX = 0, jitterY = 0;
        if (cow.currentState != CowState.Drinking)
        {
            jitterX = (Mathf.PerlinNoise(Time.time * 5, 0) - 0.5f) * vibrationIntensity;
            jitterY = (Mathf.PerlinNoise(0, Time.time * 5) - 0.5f) * vibrationIntensity;
            jitterY += Mathf.Sin(Time.time * 3) * (vibrationIntensity * 0.5f);
        }
        transform.position = basePosition + new Vector3(jitterX, jitterY, 0);
    }

    public void OnClick(PointerEventData eventData)
    {
        if (eventData.dragging) return;
        Flee(eventData.pressPosition);
    }

    public void Flee(Vector2 clickSource)
    {
        StopAllCoroutines();
        cow.currentState = CowState.Fleeing;

        targetSpeed = maxFleeSpeed;
        fleeTimer = fleeBurstDuration;

        Vector3 clickWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(clickSource.x, clickSource.y, Mathf.Abs(Camera.main.transform.position.z)));
        Vector3 fleeDir = (transform.position - clickWorldPos).normalized;

        targetPosition = transform.position + fleeDir * 5.0f;
        targetPosition.x = Mathf.Clamp(targetPosition.x, roamingArea.Bounds.min.x, roamingArea.Bounds.max.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, roamingArea.Bounds.min.y, roamingArea.Bounds.max.y);
    }

    void PickNewTarget()
    {
        targetSpeed = wanderSpeed;
        if (Random.Range(0, 100) < chanceToVisitPond && !cow.IsInJar)
        {
            cow.currentState = CowState.GoingToPond;
            targetPosition = pondLocation;
        }
        else
        {
            cow.currentState = CowState.Wandering;
            targetPosition = new Vector3(Random.Range(roamingArea.Bounds.min.x, roamingArea.Bounds.max.x), Random.Range(roamingArea.Bounds.min.y, roamingArea.Bounds.max.y), -0.1f);
        }
    }

    IEnumerator DrinkAtPond()
    {
        cow.currentState = CowState.Drinking;

        Pond pond = FindAnyObjectByType<Pond>();
        if (pond != null && pond.currentState == PondState.FilledWithStickyGlue)
        {
            Debug.Log("The cow is stuck in the glue");
            transform.SetParent(pond.transform);
            cow.currentState = CowState.Stuck;
            enabled = false;
            yield break;
        }

        yield return new WaitForSeconds(drinkDuration);
        if (cow.currentState == CowState.Drinking) PickNewTarget();
    }
}