using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MasonJarItem : InventoryItem
{
    public int numberOfCows = 0;

    [SerializeField] private GameObject uiCowPrefab; 

    private void Start()
    {
        UpdateVisuals();
    }

    public void UpdateVisuals()
    {

        RectTransform jarRect = GetComponent<RectTransform>();

        for (int i = 0; i < numberOfCows; i++)
        {
            GameObject cow = Instantiate(uiCowPrefab, transform);
            cow.GetComponent<UiFlyCow>().Setup(jarRect);
        }

    }

    public override GameObject SpawnInWorld(Vector3 position, Transform parent)
    {
        wasPlaced = true;
        GameObject spawnedObject = Instantiate(WorldPrefab, parent);
        spawnedObject.transform.localPosition = position;
        MasonJar masonJar = spawnedObject.GetComponent<MasonJar>();
        masonJar.cowsInJar = numberOfCows;
        

        return spawnedObject;
    }

    public override void Setup(GameObject obj)
    {
        numberOfCows = obj.GetComponent<MasonJar>().cowsInJar;
    }
}
