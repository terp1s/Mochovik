using UnityEngine;

public static class GameBootstrap
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        if (AudioManager.instance != null)
            return;

        GameObject prefab = Resources.Load<GameObject>("PersisterCells");

        if (prefab == null)
        {
            Debug.LogError("PersisterCells prefab not found in Resources!");
            return;
        }

        Object.Instantiate(prefab);
    }
}