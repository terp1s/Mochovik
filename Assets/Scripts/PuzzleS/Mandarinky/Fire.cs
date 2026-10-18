using UnityEngine;

public class Fire : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        GameEventsManager.instance.puzzleEvents.OnCowsBurned += EndFire;
    }
    private void OnDisable()
    {
        GameEventsManager.instance.puzzleEvents.OnCowsBurned -= EndFire;
    }

    public void StartFire()
    {
        animator.SetTrigger("Start");
    }

    public void EndFire()
    {
        animator.SetTrigger("GoOut");
    }
}

