using Unity.VisualScripting;
using UnityEngine;

public class CapPuzzleAnchor : MonoBehaviour
{
    public int CapCount;
    public int RopeIndex;
    [SerializeField] private CapHolder Holder;

    [SerializeField] private float sizeMultiplier = 0.85f;
    [SerializeField] private float brightnessMultiplier = 0.85f;

    private void Awake()
    {
        Holder = GetComponent<CapHolder>();

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
