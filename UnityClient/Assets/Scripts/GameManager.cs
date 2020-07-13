using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();

    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;

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

    public void SpawnPlayer(int id, string username, Vector3 pos, Quaternion rotation)
    {
        GameObject player;
        if (id == Client.Instance.myId)
        {
            player = Instantiate(localPlayerPrefab, pos, rotation);
        }
        else
        {
            player = Instantiate(playerPrefab, pos, rotation);
        }

        player.GetComponent<PlayerManager>().ID = id;
        player.GetComponent<PlayerManager>().UserName = username;
        players.Add(id, player.GetComponent<PlayerManager>());
    }
}
