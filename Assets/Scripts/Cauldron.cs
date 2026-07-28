using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cauldron : MonoBehaviour, IInteractable
{
    
    private List<ItemData> ingredients = new();

    public void AddIngredient(ItemData item)
    {
        ingredients.Add(item);
        Debug.Log($"Added {item.name} to potion");
    }
    public IReadOnlyList<ItemData> GetIngredients()
    {
        return ingredients;
    }
    public void Interact()
    {

    }

}
