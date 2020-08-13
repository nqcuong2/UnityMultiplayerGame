using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public GameObject startMenu;
    public GameObject ingameMenu;
    public GameObject mobileController;

    public InputField serverIpField;
    public InputField usernameField;
    public Image avatar;
    public Text ingameName;
    public InputField chatField;
    public Button quitBtn;
    public Button sendBtn;

    public Sprite[] avatarSprites;

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

    private void Start()
    {
        sendBtn.onClick.AddListener(() => SendMessage());
        chatField.onEndEdit.AddListener(delegate { SendMessage(); });
    }

    private void Update()
    {
        if (chatField.isFocused && Input.GetKeyDown(KeyCode.Return))
        {
            SendMessage();
        }
    }

    private void SendMessage()
    {
        if (chatField.text.Length > 0)
        {
            Debug.Log(chatField.text);
            chatField.text = "";
        }
    }

    public void ConnectToServer()
    {
        if (serverIpField.text.Length != 0 && usernameField.text.Length != 0)
        {
            startMenu.SetActive(false);
            ingameMenu.SetActive(true);
#if UNITY_ANDROID
            mobileController.SetActive(true);
#endif
            ingameName.text = usernameField.text;
            Client.Instance.ConnectToServer(serverIpField.text);

            GameManager.Instance.SpawnGameMap();
        }
    }

    public void ShowMainMenu()
    {
        startMenu.SetActive(true);
        ingameMenu.SetActive(false);
#if UNITY_ANDROID
        mobileController.SetActive(false);
#endif
    }

    #region Mobile Devices
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
    #endregion

    public void SetPlayerAvatar(int avatarIndex)
    {
        avatar.sprite = avatarSprites[avatarIndex];
    }
}
