﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    #region Packets
    public static void WelcomeReceived()
    {
        using (Packet packet = new Packet((int)ClientPackets.WelcomeReceived))
        {
            packet.Write(Client.Instance.myId);
            packet.Write(UIManager.Instance.usernameField.text);

            SendTCPData(packet);
        }
    }

    public static void SendChatMsg(string message)
    {
        using (Packet packet = new Packet((int)ClientPackets.SendChatMsg))
        {
            packet.Write(Client.Instance.myId);
            packet.Write(message);

            SendTCPData(packet);
        }
    }

    public static void PlayerMovement(bool[] inputs)
    {
        using (Packet packet = new Packet((int)ClientPackets.PlayerMovement))
        {
            packet.Write(inputs.Length);
            foreach (bool input in inputs)
            {
                packet.Write(input);
            }

            SendUDPData(packet);
        }
    }

    public static void SpawnBomb(Vector2 pos)
    {
        using (Packet packet = new Packet((int)ClientPackets.SpawnBomb))
        {
            packet.Write(Client.Instance.myId);
            packet.Write(pos);

            SendTCPData(packet);
        }
    }
    #endregion

    private static void SendTCPData(Packet packet)
    {
        packet.WriteLength();
        Client.Instance.tcp.SendData(packet);
    }

    private static void SendUDPData(Packet packet)
    {
        packet.WriteLength();
        Client.Instance.udp.SendData(packet);
    }
}
