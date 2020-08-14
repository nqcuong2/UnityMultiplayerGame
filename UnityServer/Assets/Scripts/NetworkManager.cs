using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance { get; private set; }

    public GameObject playerPrefab;

    private int nextPlayerIndex = 0;
    private Vector2[] playerPositions =
    {
        new Vector2(-8.2994f, 4.437201f),
        new Vector2(4.478397f, 4.437201f),
        new Vector2(-8.2994f, -4.4514f),
        new Vector2(4.478397f, -4.4514f)
    };

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
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;

        Server.Start(4, 26950);
    }

    private void OnApplicationQuit()
    {
        Server.Stop();
    }

    public Player InstantiatePlayer()
    {
        Player player = Instantiate(playerPrefab, playerPositions[nextPlayerIndex], Quaternion.identity).GetComponent<Player>();
        player.avatar = nextPlayerIndex++;
        return player;
    }
}
