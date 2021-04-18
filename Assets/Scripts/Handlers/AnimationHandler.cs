using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private static readonly int Run = Animator.StringToHash("Run");

    private void Awake()
    {
        animator ??= GetComponent<Animator>();
    }

    private void ChangeState()
    {
        animator.SetTrigger(Run);
    }

    public void GetStartingInput()
    {
        ChangeState();
    }
}