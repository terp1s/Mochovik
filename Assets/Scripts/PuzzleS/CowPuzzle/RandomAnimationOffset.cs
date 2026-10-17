using UnityEngine;

public class RandomAnimationOffset : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        animator = GetComponent<Animator>();

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

        float offset = Random.value;

        animator.Play(state.fullPathHash, 0, offset);
        animator.Update(0f);
    }
}