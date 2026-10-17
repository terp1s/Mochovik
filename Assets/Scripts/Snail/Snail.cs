using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IEdible
{

}
public class Snail : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Sprite eating;
    [SerializeField] Sprite idle;
    private MandarinkyCapElevator elevator;

    public float eatingTime = 0.5f;
    public float waitBeforeEating = 2f;
    public float waitBeforEmote = 0.5f;

    private void Awake()
    {
        elevator = FindAnyObjectByType<MandarinkyCapElevator>();
    }
    private void OnEnable()
    {
        GameEventsManager.instance.puzzleEvents.OnCargoDelivered += Eat;
    }
    private void OnDisable()
    {
        GameEventsManager.instance.puzzleEvents.OnCargoDelivered -= Eat;
    }

    private void Eat(GameObject food)
    {
        if (food is null || !food.TryGetComponent<IEdible>(out IEdible edible))
            return;
        
        Debug.Log("Snail is eating!");

        StartCoroutine(EatCoroutine(food));
    }

    private IEnumerator EatCoroutine(GameObject food)
    {
        yield return new WaitForSeconds(waitBeforeEating);

        GetComponent<SpriteRenderer>().sprite = eating;
        yield return new WaitForSeconds(eatingTime);

        elevator.OnObjectRemoved();
        Destroy(food);
        yield return new WaitForSeconds(waitBeforEmote);

        GetComponent<SpriteRenderer>().sprite = idle;
        yield return new WaitForSeconds(waitBeforEmote);

        GetComponent<SnailEmotes>().EmoteHunger();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GetComponent<SnailEmotes>().EmoteHunger();
    }

   
}
