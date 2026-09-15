using UnityEngine;

public class PoppyAnimationHandlerer : MonoBehaviour
{
    public void LoadGame()
    {
        HomeManager.Instance.LoadGame();
    }

    public void ExitGame()
    {
        HomeManager.Instance.ExitGame();
    }
}
