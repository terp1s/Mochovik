using Unity.VisualScripting;
using UnityEngine;

public class CapPuzzleAnchor : MonoBehaviour
{
    public int CapCount;
    public int RopeIndex;
    [SerializeField] private CapHolder Holder;

    private void Awake()
    {
        Holder = GetComponent<CapHolder>();
        //RopeIndex = (int)gameObject.name[4];

    }
    public bool IsCompleted()
    {
        CapPuzzlePiece cap = Holder.cap;
        int count = 0;

        while (cap is not null || count > CapCount)
        {
            if (cap.capIndex.x != RopeIndex || cap.capIndex.y != count)
            {
                return false;
            }
            count++;

            if (cap.ChildHolder is null)
            {
                break;
            }


            cap = cap.ChildHolder.cap;
        }

        return count == CapCount;
    }
}
