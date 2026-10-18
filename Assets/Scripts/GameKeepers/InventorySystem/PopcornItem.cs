using UnityEngine;

public class PopcornItem : InventoryItem
{
    public bool IsGrown;
    public override void Setup(GameObject obj)
    {
        Sprite = obj.GetComponent<SpriteRenderer>().sprite;
        IsGrown = obj.GetComponent<Popcorn>().IsGrown;
    }

    public override GameObject SpawnInWorld(Vector3 position, Transform parent)
    {
        GameObject spawn = base.SpawnInWorld(position, parent);

        spawn.GetComponent<SpriteRenderer>().sprite = Sprite;

        if (IsGrown)
        {
            spawn.GetComponent<Popcorn>().SpawnAsGrown();
        }

        return spawn;
    }
}
