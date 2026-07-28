using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemInteraction { Drag, Click }

[CreateAssetMenu(fileName = "New Item", menuName = "Game/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
}

public interface ISlotItemInteractionHandlerer
{
    void HandleSlotInteraction(ItemInteraction interacrtion);
}