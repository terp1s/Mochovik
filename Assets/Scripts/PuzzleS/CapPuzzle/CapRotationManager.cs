using UnityEngine;

public struct RotationState
{
    public CapRotate cap;
    public bool hasBeenRotated;
    public RotationState(CapRotate cap, bool isRotated)
    {
        this.cap = cap;
        this.hasBeenRotated = isRotated;
    }
}
public class CapRotationManager : MonoBehaviour
{
    [SerializeField] private RotationState[] capStates = new RotationState[13];
    [SerializeField] private GameObject valvePrefab;
    private bool valveHasSpawned = false;

    private void Awake()
    {
        CapRotate[] caps = GetComponentsInChildren<CapRotate>();

        for (int i = 0; i < caps.Length; i++)
        {
            capStates[i] = new RotationState(caps[i], false);
            caps[i].ID = i; // Assign a unique ID to each cap
        }
    }

    public void UpdateCapState(CapRotate cap, bool isRotated)
    {
        capStates[cap.ID] = new RotationState(cap, true);

        int untouchedCount = CheckUntouchedCaps();

        if (untouchedCount == 0 && !valveHasSpawned)
        {
            Debug.Log("All caps have been rotated at least once!");

            cap.SpawnValve(valvePrefab);
            valveHasSpawned = true;
        }
    }

    private int CheckUntouchedCaps()
    {
        int untouchedCount = 0;
        foreach (var state in capStates)
        {
            if (!state.hasBeenRotated)
            {
                untouchedCount++;
            }
        }
        return untouchedCount;
    }
}
