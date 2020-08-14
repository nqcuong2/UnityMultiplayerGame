using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public GameObject startMenu;
    public GameObject ingameMenu;
    public GameObject mobileController;

    public GameObject messageContainer;
    public GameObject localMsgPref;
    public GameObject msgPref;

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

    private void SendMessage()
    {
        if (chatField.text.Length > 0)
        {
            string msg = $"Me: {chatField.text}";
            var msgObj = Instantiate(localMsgPref, messageContainer.transform);
            msgObj.GetComponent<Text>().text = msg;
            ClientSend.SendChatMsg(chatField.text);
            chatField.text = "";
            chatField.ActivateInputField();
        }
    }

    public void AddMessage(string username, string message)
    {
        var msg = Instantiate(msgPref, messageContainer.transform);
        msg.GetComponent<Text>().text = $"{username}: {message}";
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
