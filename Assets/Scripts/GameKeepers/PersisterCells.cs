using UnityEngine;

public class PersisterCells : MonoBehaviour
{
    public static PersisterCells Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
}
