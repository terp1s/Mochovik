using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sardine Can", menuName = "Game/Sardine Can")]
public class SardineCanItemData : ItemData
{
    public Sprite open;
    public Sprite closed;
    public GameObject puzzle;

    public override void Use()
    {
        if(puzzle is null)
        {
            puzzle = GameObject.Find("SardineCan");

        }
        if (puzzle != null)
        {
            puzzle.gameObject.SetActive(true);
        }
    }

    public override void EndDrag()
    {
        base.EndDrag();
    }
}