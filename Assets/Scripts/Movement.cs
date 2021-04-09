using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float playerSpeed;
    [SerializeField, Range(0, 1f)] private float horizontalSmoothInput;
    private readonly Vector3 straightDirection = Vector3.forward;

    private void Update()
    {
        MobileInput(out var moveDirection);

        characterController.Move(moveDirection * (playerSpeed * Time.deltaTime));
    }

    private void MobileInput(out Vector3 moveDirection)
    {
        moveDirection = new Vector3(0, 0, straightDirection.z);
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                var touchDeltaPosition = touch.deltaPosition;
                var deltaVector = new Vector3(touchDeltaPosition.x * horizontalSmoothInput, 0, straightDirection.z);

                moveDirection = deltaVector;
            }
        }
    }
}