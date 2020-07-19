using UnityEngine;

public class Player : MonoBehaviour
{
    public int id;
    public string username;
    public CharacterController controller;
    public float gravity = -9.81f;
    public float moveSpeed = 5f;
    public float jumpSpeed = 5f;

    private bool[] inputs;
    private float yVelocity = 0;

    private void Start()
    {
        gravity *= Time.fixedDeltaTime * Time.fixedDeltaTime;
        moveSpeed *= Time.fixedDeltaTime;
        jumpSpeed *= Time.fixedDeltaTime;
    }

    public void Initialize(int id, string username)
    {
        this.id = id;
        this.username = username;

        inputs = new bool[5];
    }

    public void SetInput(bool[] inputs, Quaternion rotation)
    {
        this.inputs = inputs;
        transform.rotation = rotation;
    }

    public void FixedUpdate()
    {
        Vector2 inputDir = Vector2.zero;
        if (inputs[0])
        {
            inputDir.y += 1;
        }
        if (inputs[1])
        {
            inputDir.y -= 1;
        }
        if (inputs[2])
        {
            inputDir.x -= 1;
        }
        if (inputs[3])
        {
            inputDir.x += 1;
        }

        Move(inputDir);
    }

    private void Move(Vector2 inputDir)
    {
        Vector3 moveDirection = transform.right * inputDir.x + transform.forward * inputDir.y;
        moveDirection *= moveSpeed;

        if (controller.isGrounded)
        {
            yVelocity = 0f;
            if (inputs[4])
            {
                yVelocity += jumpSpeed;
            }
        }
        yVelocity += gravity;

        moveDirection.y = yVelocity;
        controller.Move(moveDirection);

        ServerSend.PlayerPosition(this);
        ServerSend.PlayerRotation(this);
    }
}
