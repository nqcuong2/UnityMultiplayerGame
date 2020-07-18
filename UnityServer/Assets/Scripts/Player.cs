using UnityEngine;

public class Player : MonoBehaviour
{
    public int id;
    public string username;

    private float moveSpeed = 5f / Constants.TICKS_PER_SEC;
    private bool[] inputs;

    public void Initialize(int id, string username)
    {
        this.id = id;
        this.username = username;

        inputs = new bool[4];
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
        Vector3 moveDir = transform.right * inputDir.x + transform.forward * inputDir.y;
        transform.position += moveDir * moveSpeed;

        ServerSend.PlayerPosition(this);
        ServerSend.PlayerRotation(this);
    }
}
