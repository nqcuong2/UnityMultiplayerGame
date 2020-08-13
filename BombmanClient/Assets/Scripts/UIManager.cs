using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public GameObject startMenu;
    public GameObject chatPanel;
    public GameObject mobileController;

    public InputField serverIpField;
    public InputField usernameField;

    public Button sendBtn;

    private bool leftPressed;
    private bool rightPressed;
    private bool upPressed;
    private bool downPressed;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    public void ConnectToServer()
    {
        if (serverIpField.text.Length != 0 && usernameField.text.Length != 0)
        {
            startMenu.SetActive(false);
            chatPanel.SetActive(true);
#if UNITY_ANDROID
            mobileController.SetActive(true);
#endif
            Client.Instance.ConnectToServer(serverIpField.text);
        }
    }

    public void ShowMainMenu()
    {
        startMenu.SetActive(true);
        chatPanel.SetActive(false);
#if UNITY_ANDROID
        mobileController.SetActive(false);
#endif
    }

    public void LeftEnter()
    {
        ResetInputs();
        leftPressed = true;
    }

    public void ResetInputs()
    {
        leftPressed = false;
        rightPressed = false;
        upPressed = false;
        downPressed = false;
    }

    public void RightEnter()
    {
        ResetInputs();
        rightPressed = true;
    }

    public void UpEnter()
    {
        ResetInputs();
        upPressed = true;
    }

    public void DownEnter()
    {
        ResetInputs();
        downPressed = true;
    }

    public bool[] GetInputs()
    {
        return new bool[]{ upPressed, downPressed, leftPressed, rightPressed};
    }
}
