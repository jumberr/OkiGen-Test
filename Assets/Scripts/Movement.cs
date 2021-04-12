using System;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float playerSpeed;
    [SerializeField, Range(0, 1f)] private float horizontalSmoothInput;
    private Vector3 straightDirection = Vector3.forward;

    private void Update()
    {
        MobileInput(out var moveDirection);

        characterController.Move(moveDirection * (playerSpeed * Time.deltaTime));
    }

    private void MobileInput(out Vector3 moveDirection)
    {
        moveDirection = straightDirection;
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                var touchDeltaPosition = touch.deltaPosition;

                Vector3 deltaVector;
                if (straightDirection == Vector3.forward) // z
                {
                    deltaVector = new Vector3(touchDeltaPosition.x * horizontalSmoothInput, 0, straightDirection.z);
                }
                else if (straightDirection == Vector3.right) // x
                {
                    deltaVector =
                        new Vector3(straightDirection.x, 0, -touchDeltaPosition.x * horizontalSmoothInput);
                }
                else if (straightDirection == Vector3.back) // -z
                {
                    deltaVector =
                        new Vector3(-touchDeltaPosition.x * horizontalSmoothInput, 0, straightDirection.z);
                }
                else // -x
                {
                    deltaVector =
                        new Vector3(straightDirection.x, 0, touchDeltaPosition.x * horizontalSmoothInput);
                }

                moveDirection = deltaVector;
            }
        }
    }

    public void OnRotate(float rotation)
    {
        //              Turn (right or left 1-4 times streak)
        //           ------------------------------------------
        //                        |    Right       |      Left
        //           ------------------------------------------
        //           1st time     |      x         |       -x
        //           2nd time     |     -z         |       -z
        //           3d time      |     -x         |        x
        //           4th time     |      z         |        z
        
        if (rotation > 0)  // turn right
        {
            if (straightDirection == Vector3.forward) // was z
            {
                straightDirection = Vector3.right; // do x
            }
            else if (straightDirection == Vector3.right) // was x
            {
                straightDirection = Vector3.back; // do -z
            }
            else if (straightDirection == Vector3.back) //was -z
            {
                straightDirection = Vector3.left; // do -x
            }
            else
            {
                straightDirection = Vector3.forward;
            }
        }
        else  //left
        {
            if (straightDirection == Vector3.forward) // was z
            {
                straightDirection = Vector3.left; // do -x
            }
            else if (straightDirection == Vector3.left) // was -x
            {
                straightDirection = Vector3.back; // do -z
            }
            else if (straightDirection == Vector3.back) //was -z
            {
                straightDirection = Vector3.right; // do x
            }
            else
            {
                straightDirection = Vector3.forward;
            }
        }
    }
}