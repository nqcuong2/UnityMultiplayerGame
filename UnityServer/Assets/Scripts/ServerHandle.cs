using System;
using UnityEngine;

public class ServerHandle
{
    public static void WelcomeReceived(int fromClient, Packet packet)
    {
        int clientIdCheck = packet.ReadInt();
        string username = packet.ReadString();

        Debug.Log($"{Server.clients[fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {fromClient}.");

        // Should not occur otherwise sth goes wrong
        if (fromClient != clientIdCheck)
        {
            Debug.Log($"Plaer \"{username}\" (ID: {fromClient}) has assumed the wrong client ID ({clientIdCheck})!");
        }

        Server.clients[fromClient].SendIntoGame(username);
    }

    public static void PlayerMovement(int fromClient, Packet packet)
    {
        bool[] inputs = new bool[packet.ReadInt()];
        for (int i = 0; i < inputs.Length; i++)
        {
            inputs[i] = packet.ReadBool();
        }

        Server.clients[fromClient].player.SetInput(inputs);
    }
}
