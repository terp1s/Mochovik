using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public enum PondState { FilledWithWater, Empty, FilledWithRunnyGlue, FilledWithStickyGlue };
public class Pond : MonoBehaviour, IItemReceiver
{
    public PondLiquid pondLiquid;
    public PondState currentState = PondState.FilledWithWater;

    [Header("Settings")]
    public float glueDryingTime = 10f;



    public string requiredItemName = "Chemopren";

    public bool CanReceiveItem(InventoryItem item)
    {
        switch (currentState)
        {
            case PondState.Empty:
                return item is ChemoprenItem;

            default:
                return false;
        }
    }

    public void OnReceiveItem(InventoryItem item)
    {
        if (currentState == PondState.FilledWithWater)
        {
            DrainPond();
        }
        if (currentState == PondState.Empty && item is ChemoprenItem glue)
        {
            FillWithGlue();
        }
    }
    private void DrainPond()
    {
        Debug.Log("Pond drained!");
        currentState = PondState.Empty;
        pondLiquid.Drain();
    }

    private void FillWithGlue()
    {
        Debug.Log("Pond filled with glue");
        currentState = PondState.FilledWithRunnyGlue;
        pondLiquid.ChangeToLiquidGlue();
        StartCoroutine(DryGlueRoutine());
    }

    private IEnumerator DryGlueRoutine()
    {
        yield return new WaitForSeconds(glueDryingTime);
        currentState = PondState.FilledWithStickyGlue;
        pondLiquid.ChangeToStickyGlue();
        Debug.Log("The glue is now sticky");
    }

}
