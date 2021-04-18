using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float playerSpeed;

    public Vector3 StraightDirection { get; private set; }
    public Vector3 MoveDirection { get; set; }

    private void Awake()
    {
        StraightDirection = Vector3.forward;
    }

    private void Update()
    {
        characterController.Move(MoveDirection * (playerSpeed * Time.deltaTime));
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

        if (rotation > 0) // turn right
        {
            if (StraightDirection == Vector3.forward) // was z
            {
                StraightDirection = Vector3.right; // do x
            }
            else if (StraightDirection == Vector3.right) // was x
            {
                StraightDirection = Vector3.back; // do -z
            }
            else if (StraightDirection == Vector3.back) //was -z
            {
                StraightDirection = Vector3.left; // do -x
            }
            else
            {
                StraightDirection = Vector3.forward;
            }
        }
        else //left
        {
            if (StraightDirection == Vector3.forward) // was z
            {
                StraightDirection = Vector3.left; // do -x
            }
            else if (StraightDirection == Vector3.left) // was -x
            {
                StraightDirection = Vector3.back; // do -z
            }
            else if (StraightDirection == Vector3.back) //was -z
            {
                StraightDirection = Vector3.right; // do x
            }
            else
            {
                StraightDirection = Vector3.forward;
            }
        }
    }
}