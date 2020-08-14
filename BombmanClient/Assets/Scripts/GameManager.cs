using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();

    public GameObject playerPrefab;
    public GameObject stagePrefab;
    public GameObject bombPrefab;

    public Sprite[] playerSprites;

    private GameObject currStage;

    private PlayerManager myPlayer;

    private GameObject[,] bombMap = new GameObject[9, 12];
    private float bombXOrigin;
    private float bombYOrigin;
    private const float BOMB_X_OFFSET = 1.1592f;
    private const float BOMB_Y_OFFSET = 1.122f;

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
        bombXOrigin = bombPrefab.transform.position.x;
        bombYOrigin = bombPrefab.transform.position.y;
    }

    public void SpawnGameMap()
    {
        currStage = Instantiate(stagePrefab);
    }

    public void SpawnMyBomb()
    {
        if (myPlayer.PlacedBombs < myPlayer.MaxBombs)
        {
            int rowIndex = (int)(Math.Round(Math.Abs(myPlayer.transform.position.y - bombYOrigin) / BOMB_Y_OFFSET));
            int colIndex = (int)(Math.Round((myPlayer.transform.position.x - bombXOrigin) / BOMB_X_OFFSET)); 
            if (bombMap[rowIndex, colIndex] == null)
            {
                var bomb = Instantiate(bombPrefab);
                bomb.transform.position = new Vector2(bombXOrigin + colIndex * BOMB_X_OFFSET, bombYOrigin - rowIndex * BOMB_Y_OFFSET);
                bomb.GetComponent<Bomb>().OwnerID = myPlayer.ID;
                myPlayer.PlacedBombs++;
                bombMap[rowIndex, colIndex] = bomb;
            }
        }
    }

    public void BombExploded(GameObject explodedBomb)
    {
        for (int row = 0; row < bombMap.GetLength(0); row++)
        {
            for (int col = 0; col < bombMap.GetLength(1); col++)
            {
                if (bombMap[row, col] == explodedBomb)
                {
                    if (explodedBomb.GetComponent<Bomb>().OwnerID == myPlayer.ID)
                    {
                        myPlayer.PlacedBombs--;
                    }

                    bombMap[row, col] = null;
                    return;
                }
            }
        }
    }

    public void SpawnPlayer(int id, string username, int avatar, Vector3 pos)
    {
        playerPrefab.GetComponent<SpriteRenderer>().sprite = playerSprites[avatar];
        GameObject player = Instantiate(playerPrefab, pos, Quaternion.identity);
        player.GetComponent<PlayerManager>().ID = id;
        player.GetComponent<PlayerManager>().UserName = username;
        players.Add(id, player.GetComponent<PlayerManager>());

        if (id == Client.Instance.myId)
        {
            UIManager.Instance.SetPlayerAvatar(avatar);
            myPlayer = players[Client.Instance.myId].GetComponent<PlayerManager>();
        }
        else
        {
            Destroy(player.GetComponent<PlayerController>());
        }
    }

    public void Disconnect()
    {
        ThreadManager.ExecuteOnMainThread(() =>
        {
            foreach (PlayerManager playerManager in players.Values)
            {
                Destroy(playerManager.gameObject);
            }
            DestroyGameMap();
            UIManager.Instance.ShowMainMenu();
        });
    }

    private void DestroyGameMap()
    {
        Destroy(currStage);
    }
}
