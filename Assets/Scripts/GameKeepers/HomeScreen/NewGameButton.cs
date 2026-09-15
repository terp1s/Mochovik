using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class NewGameButton : MonoBehaviour
{
    public void NewGame()
    {
       
        HomeManager.Instance.NewGame();
    }
}
