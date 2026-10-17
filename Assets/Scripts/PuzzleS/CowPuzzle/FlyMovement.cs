
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;

public class FlyMovement : MonoBehaviour
{
    public Cow cow;
    private SpriteRenderer spriteRenderer;


    [Header("Movement Settings")]
    public float wanderSpeed = 1.0f;
    public float maxFleeSpeed = 6.0f;
    public float acceleration = 30.0f;
    public float deceleration = 30.0f;
    public float fleeBurstDuration = 0.25f;
    public float landingDuration = 1.3f;

    [Header("Visuals")]
    public float vibrationIntensity = 0.1f;

    [Header("Enclosure & Pond")]
    public CowRoamingArea roamingArea;
    public Transform pondLocation;

    public float drinkDuration = 5.0f;
    [Range(0, 100)] public float chanceToVisitPond = 10f;

    private Vector3 targetPosition;
    private Vector3 basePosition;

    private float currentSpeed;
    private float targetSpeed;
    private float fleeTimer;
    private bool isCheckingPond = false;


    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        cow = GetComponent<Cow>();
    }

    private void OnEnable()
    {
        basePosition = (cow != null && cow.IsInJar) ? transform.localPosition : transform.position;

        currentSpeed = wanderSpeed;
        targetSpeed = wanderSpeed;

        GameEventsManager.instance.puzzleEvents.OnWaterDrained += WaterDrained;
        GameEventsManager.instance.puzzleEvents.OnGlueDried += GlueDried;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.puzzleEvents.OnWaterDrained -= WaterDrained;
        GameEventsManager.instance.puzzleEvents.OnGlueDried -= GlueDried;
    }

    private void Start()
    {
        PickNewTarget();

    }

    void Update()
    {
        // If the cow is stuck or drinking, it should not move or change state.
        if (cow.CurrentState == CowState.Stuck || cow.CurrentState == CowState.Drinking || isCheckingPond)
            return;

        HandleSpeedLogic();

        basePosition = Vector3.MoveTowards(basePosition, targetPosition, Time.deltaTime * currentSpeed);
        float triggerDistance = CalculateTriggerDistancce();

        //cows in jar only wander
        if (!cow.IsInJar)
        {
            //start landing animation in time
            if (Vector3.Distance(basePosition, pondLocation.position) < triggerDistance)
            {
                if (cow.CurrentState == CowState.GoingToPond)
                    cow.SetState(CowState.Landing);
            }
        }


        if (Vector3.Distance(basePosition, targetPosition) < 0.1f)
        {
            if (cow.CurrentState == CowState.Landing)
                StartCoroutine(DrinkAtPond());
            else
                PickNewTarget();
        }

        ApplyVisuals();
    }

    private float CalculateTriggerDistancce()
    {
        AnimatorStateInfo state = cow.GetAnimatorStateInfo();
        float normalizedTime = state.normalizedTime % 1f;
        float wanderingRemainingTime = wanderSpeed * (1 - normalizedTime);
        float totalTime = wanderingRemainingTime + landingDuration;
        float triggerDistance = wanderSpeed * totalTime;

        return triggerDistance;
    }

    void HandleSpeedLogic()
    {
        if (cow.CurrentState == CowState.Fleeing)
        {
            fleeTimer -= Time.deltaTime;
            if (fleeTimer <= 0)
            {
                targetSpeed = wanderSpeed;
                if (currentSpeed <= wanderSpeed + 0.1f)
                {
                    cow.SetState(CowState.Wandering);
                }
            }
        }


        float rate = (currentSpeed < targetSpeed) ? acceleration : deceleration;
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, Time.deltaTime * rate);
    }

    void ApplyVisuals()
    {
        float jitterX = 0, jitterY = 0;
        if (cow.CurrentState != CowState.Drinking)
        {
            jitterX = (Mathf.PerlinNoise(Time.time * 5, 0) - 0.5f) * vibrationIntensity;
            jitterY = (Mathf.PerlinNoise(0, Time.time * 5) - 0.5f) * vibrationIntensity;
            jitterY += Mathf.Sin(Time.time * 3) * (vibrationIntensity * 0.5f);
        }

        Vector3 jitterOffset = new Vector3(jitterX, jitterY, 0);

        if (cow != null && cow.IsInJar)
        {
            transform.localPosition = basePosition + jitterOffset;
        }
        else
        {
            transform.position = basePosition + jitterOffset;
        }
    }

    public void OnClick(PointerEventData eventData)
    {
        if (eventData.dragging) return;
        Flee(eventData.pressPosition);
    }

    public void Flee(Vector2 clickSource)
    {
        StopAllCoroutines();
        cow.SetState(CowState.Fleeing);

        targetSpeed = maxFleeSpeed;
        fleeTimer = fleeBurstDuration;

        Vector3 clickWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(clickSource.x, clickSource.y, Mathf.Abs(Camera.main.transform.position.z)));
        Vector3 fleeDir = (transform.position - clickWorldPos).normalized;

        targetPosition = transform.position + fleeDir * 5.0f;
        targetPosition.x = Mathf.Clamp(targetPosition.x, roamingArea.Bounds.min.x, roamingArea.Bounds.max.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, roamingArea.Bounds.min.y, roamingArea.Bounds.max.y);
    }

    public void PickNewTarget()
    {
        targetSpeed = wanderSpeed;

        if (Random.Range(0, 100) < chanceToVisitPond && !cow.IsInJar)
        {
            cow.SetState(CowState.GoingToPond);
            targetPosition = pondLocation.position;
        }
        else if (cow.IsInJar)
        {
            cow.SetState(CowState.Wandering);

            // Pick a world point within the jar's roaming bounds
            Vector3 worldTarget = new Vector3(
                Random.Range(roamingArea.Bounds.min.x, roamingArea.Bounds.max.x),
                Random.Range(roamingArea.Bounds.min.y, roamingArea.Bounds.max.y),
                roamingArea.transform.position.z
            );

            // Convert it to local space inside the jar
            targetPosition = transform.parent != null
                ? transform.parent.InverseTransformPoint(worldTarget)
                : worldTarget;

            targetPosition.z = -0.1f;
        }
        else
        {
            cow.SetState(CowState.Wandering);
            targetPosition = new Vector3(
                Random.Range(roamingArea.Bounds.min.x, roamingArea.Bounds.max.x),
                Random.Range(roamingArea.Bounds.min.y, roamingArea.Bounds.max.y),
                -0.1f
            );
        }


        if (targetPosition.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

    IEnumerator DrinkAtPond()
    {
        isCheckingPond = true;

        Pond pond = FindAnyObjectByType<Pond>();
        if (pond != null)
        {

            if (pond.currentState == PondState.FilledWithStickyGlue)
            {
                Debug.Log("The cow is stuck in the glue");
                transform.SetParent(pond.transform);
                cow.SetState(CowState.Stuck);
                enabled = false;
                isCheckingPond = false;
                yield break;
            }

            if (pond.currentState != PondState.FilledWithWater)
            {
                yield return new WaitForSeconds(0.5f);
                isCheckingPond = false;
                PickNewTarget();
                yield break;
            }

        }

        isCheckingPond = false;
        cow.SetState(CowState.Drinking);


        yield return new WaitForSeconds(drinkDuration);
        if (cow.CurrentState == CowState.Drinking) PickNewTarget();
    }

    public void WaterDrained()
    {
        if (cow.CurrentState == CowState.Drinking)
        {
            PickNewTarget();
        }
    }

    public void GlueDried()
    {
        if (targetPosition == pondLocation.position && Vector3.Distance(targetPosition, transform.position) < 0.1f)
        {
            cow.SetState(CowState.Stuck);
        }
    }
}