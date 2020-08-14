using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet packet)
    {
        string msg = packet.ReadString();
        int myId = packet.ReadInt();

        Debug.Log($"Message from server: {msg}");
        Client.Instance.myId = myId;
        ClientSend.WelcomeReceived();

        Client.Instance.udp.Connect(((IPEndPoint)Client.Instance.tcp.socket.Client.LocalEndPoint).Port);
    }

    public static void ConnectionDenied(Packet packet)
    {
        string msg = packet.ReadString();
        Client.Instance.Disconnect();
        Debug.Log($"Message from server: {msg}");
        UIManager.Instance.ShowMessageFromServer(msg);
    }

    public static void ReceiveChatMsg(Packet packet)
    {
        int id = packet.ReadInt();
        string message = packet.ReadString();
        UIManager.Instance.AddMessage(GameManager.players[id].UserName, message);
    }

    public static void SpawnPlayer(Packet packet)
    {
        UIManager.Instance.ConnectSucceed();

        int id = packet.ReadInt();
        string username = packet.ReadString();
        int avatar = packet.ReadInt();
        Vector3 pos = packet.ReadVector3();

        GameManager.Instance.SpawnPlayer(id, username, avatar, pos);
    }

    public static void PlayerPosition(Packet packet)
    {
        int id = packet.ReadInt();
        Vector3 position = packet.ReadVector3();

        // Prevent the case where player is already disconnected and removed from the list
        // but due to multithreading, the function still executes
        if (GameManager.players.ContainsKey(id))
        {
            GameManager.players[id].transform.position = position;
        }
    }

    public static void SpawnBomb(Packet packet)
    {
        int owner = packet.ReadInt();
        Vector2 position = packet.ReadVector2();
        GameManager.Instance.SpawnBomb(owner, position);
    }

    public static void PlayerDisconnect(Packet packet)
    {
        int id = packet.ReadInt();
        GameManager.Instance.RemovePlayer(id);
    }
}
