using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Mandarinky : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] MandarinkyHole hole;
    [SerializeField] Animator smokeAnimator;
    [SerializeField] float burnToEffectDelay = 0.5f;
    
    MandarinkyCapElevator capElevator;
    MandarinkyCap cap;
    Fire fire;

    private void Awake()
    {
        capElevator = GetComponentInChildren<MandarinkyCapElevator>();
        cap = GetComponentInChildren<MandarinkyCap>();
        fire = GetComponentInChildren<Fire>(true);
    }

    private void OnEnable()
    {
        GameEventsManager.instance.puzzleEvents.OnCowsFedInMandarinky += CowsFed;
    }

    private void OnDisable()
    {
        if (GameEventsManager.instance != null)
        {
            GameEventsManager.instance.puzzleEvents.OnCowsFedInMandarinky -= CowsFed;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        hole.gameObject.SetActive(true);
        GetComponent<BoxCollider2D>().enabled = false;
    }

    public void CowsFed()
    {
        if (cap.IsSealed)
        {
            StartCoroutine(Smoke());
        }
        else
        {
            StartCoroutine(Fire());
        }
    }

    private IEnumerator Smoke()
    {
        yield return new WaitForSeconds(burnToEffectDelay);

        Debug.Log("Smoke effect triggered!");
        smokeAnimator.SetTrigger("Smoke");
    }

  

    private IEnumerator Fire()
    {
        yield return new WaitForSeconds(burnToEffectDelay);
        fire.StartFire();
        yield return new WaitForSeconds(burnToEffectDelay);
        capElevator.MoveUp();
    }

}
