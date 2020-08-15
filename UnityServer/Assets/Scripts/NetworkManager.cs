using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance { get; private set; }

    public GameObject playerPrefab;

    private List<int> avatarIndices = new List<int> { 0, 1, 2, 3 };
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

    public void AddAvatarBackToPool(int avatarIndex)
    {
        for (int i = 0; i < avatarIndices.Count; i++)
        {
            if (avatarIndices[i] > avatarIndex)
            {
                avatarIndices.Insert(i == 0 ? 0 : i, avatarIndex);
                return;
            }
        }

        avatarIndices.Add(avatarIndex);
    }

    private void OnApplicationQuit()
    {
        Server.Stop();
    }

    public Player InstantiatePlayer()
    {
        int avatarIndex = avatarIndices[0];
        avatarIndices.RemoveAt(0);
        Player player = Instantiate(playerPrefab, playerPositions[avatarIndex], Quaternion.identity).GetComponent<Player>();
        player.avatar = avatarIndex;
        return player;
    }
}
