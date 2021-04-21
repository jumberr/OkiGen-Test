using UnityEngine;

namespace Handlers
{
    public class InputHandler : MonoBehaviour
    {
        [SerializeField] private Movement movement;
        [SerializeField] private AnimationHandler animationHandler;
        [SerializeField] private CanvasHandler canvasHandler;
        [SerializeField, Range(0, 1f)] private float horizontalSmoothInput;
        private delegate void TappedNotify();
        private event TappedNotify OnTap;
        private bool tapped;
        private void Awake()
        {
            movement ??= GetComponent<Movement>();
            OnTap += SendTapInfo;
        }

        private void Update()
        {
            switch (tapped)
            {
                case false when Input.touchCount > 0:
                    tapped = !tapped;
                    OnTap?.Invoke();
                    break;
                case true:
                    MobileInput(out var moveDirection);
                    movement.MoveDirection = moveDirection;
                    break;
            }
        }

        private void MobileInput(out Vector3 moveDirection)
        {
            moveDirection = movement.StraightDirection;
            if (Input.touchCount <= 0) return;
            var touch = Input.GetTouch(0);

            if (touch.phase != TouchPhase.Moved) return;
            var touchDeltaPosition = touch.deltaPosition;

            Vector3 deltaVector;
            if (moveDirection == Vector3.forward) // z
            {
                deltaVector = new Vector3(touchDeltaPosition.x * horizontalSmoothInput, 0, moveDirection.z);
            }
            else if (moveDirection == Vector3.right) // x
            {
                deltaVector =
                    new Vector3(moveDirection.x, 0, -touchDeltaPosition.x * horizontalSmoothInput);
            }
            else if (moveDirection == Vector3.back) // -z
            {
                deltaVector =
                    new Vector3(-touchDeltaPosition.x * horizontalSmoothInput, 0, moveDirection.z);
            }
            else // -x
            {
                deltaVector =
                    new Vector3(moveDirection.x, 0, touchDeltaPosition.x * horizontalSmoothInput);
            }

            moveDirection = deltaVector;
        }
    
        private void SendTapInfo()
        {
            animationHandler.GetStartingInput();
            canvasHandler.GetStartingInput();
        }
    }
}