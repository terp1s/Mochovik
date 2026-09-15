using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    private void LeftMouseClicked()
    {
        Debug.Log("Mouse Clicked");
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

        if (hit.collider != null)
        {
            Debug.Log(hit.collider.gameObject.name);

            if (hit.collider.TryGetComponent(out IInteractable interactable))
            {
                interactable.Interact();
            }

            else if (hit.collider.TryGetComponent(out IWalkable walkable))
            {
                walkable.OnWalkTo(hit.point); // Pass the exact hit point!
            }
        }
    }
    private void OnEnable()
    {
        GameEventsManager.instance.inputEvents.onLeftMouseClicked += LeftMouseClicked;

    }
}
