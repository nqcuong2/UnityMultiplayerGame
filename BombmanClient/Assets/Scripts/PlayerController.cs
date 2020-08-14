using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !UIManager.Instance.chatField.isFocused)
        {
            GameManager.Instance.SpawnMyBomb();
        }
    }

    private void FixedUpdate()
    {
        SendInputToServer();
    }

    private void SendInputToServer()
    {
#if UNITY_STANDALONE
        bool[] inputs =
        {
            Input.GetKey(KeyCode.UpArrow),
            Input.GetKey(KeyCode.DownArrow),
            Input.GetKey(KeyCode.LeftArrow),
            Input.GetKey(KeyCode.RightArrow),
        };
#else
        bool[] inputs = UIManager.Instance.GetInputs();
#endif
        ClientSend.PlayerMovement(inputs);
    }
}
