using UnityEngine;

public class Player : MonoBehaviour
{
    public int id;
    public string username;
    public int avatar;
    public float moveSpeed = 2f;
    private Rigidbody2D rb2D;

    private bool[] inputs;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
    }

    public void Initialize(int id, string username)
    {
        this.id = id;
        this.username = username;

        inputs = new bool[5];
    }

    public void SetInput(bool[] inputs)
    {
        this.inputs = inputs;
    }

    public void FixedUpdate()
    {
        Vector2 inputDir = Vector2.zero;
        if (inputs[0])
        {
            inputDir.y += 1;
        }
        else if (inputs[1])
        {
            inputDir.y -= 1;
        }
        else if (inputs[2])
        {
            inputDir.x -= 1;
        }
        else if (inputs[3])
        {
            inputDir.x += 1;
        }

        Move(inputDir);
    }

    private void Move(Vector2 inputDir)
    {
        rb2D.velocity = new Vector3(inputDir.x * moveSpeed, inputDir.y * moveSpeed);
        ServerSend.PlayerPosition(this);
    }
}
