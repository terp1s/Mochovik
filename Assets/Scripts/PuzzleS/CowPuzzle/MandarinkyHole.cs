using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MandarinkyHole : MonoBehaviour, IDropHandler
{

    public void OnDrop(PointerEventData eventData)
    {
        if (!eventData.pointerDrag.TryGetComponent<MasonJarItem>(out MasonJarItem jar)) { return; }

        GameObject spawnedObject = jar.SpawnInWorld(new Vector3(0, 0, -0.01f), this.transform);

        MasonJar masonjar = spawnedObject.GetComponent<MasonJar>();

        GameEventsManager.instance.puzzleEvents.JarPlaced("mandarinky", masonjar);
    }
}
