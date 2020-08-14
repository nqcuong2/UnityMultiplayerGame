using UnityEngine;

public class ServerSend
{
    #region Packets
    public static void ConnectionDenied(int toClient, string msg)
    {
        using (Packet packet = new Packet((int)ServerPackets.ConnectionDenied))
        {
            packet.Write(msg);
            packet.Write(toClient);

            SendTCPData(toClient, packet);
        }
    }

    public static void Welcome(int toClient, string msg)
    {
        using (Packet packet = new Packet((int)ServerPackets.Welcome))
        {
            packet.Write(msg);
            packet.Write(toClient);

            SendTCPData(toClient, packet);
        }
    }

    public static void SendChatMsg(int exceptClient, string message)
    {
        using (Packet packet = new Packet((int)ServerPackets.SendChatMsg))
        {
            packet.Write(exceptClient);
            packet.Write(message);
            SendTCPDataToAllExcept(exceptClient, packet);
        }
    }

    public static void SpawnPlayer(int toClient, Player player)
    {
        using (Packet packet = new Packet((int)ServerPackets.SpawnPlayer))
        {
            packet.Write(player.id);
            packet.Write(player.username);
            packet.Write(player.avatar);
            packet.Write(player.transform.position);

            SendTCPData(toClient, packet);
        }
    }

    public static void PlayerPosition(Player player)
    {
        using (Packet packet = new Packet((int)ServerPackets.PlayerPosition))
        {
            packet.Write(player.id);
            packet.Write(player.transform.position);

            SendUDPDataToAll(packet);
        }
    }

    public static void SpawnBomb(int exceptClient, Vector2 pos)
    {
        using (Packet packet = new Packet((int)ServerPackets.SpawnBomb))
        {
            packet.Write(exceptClient);
            packet.Write(pos);
            SendTCPDataToAllExcept(exceptClient, packet);
        }
    }

    public static void PlayerDisconnect(int disconnectedClient)
    {
        using (Packet packet = new Packet((int)ServerPackets.PlayerDisconnect))
        {
            packet.Write(disconnectedClient);
            SendTCPDataToAll(packet);
        }
    }
    #endregion

    #region TCP
    private static void SendTCPData(int toClient, Packet packet)
    {
        packet.WriteLength();
        Server.clients[toClient].tcp.SendData(packet);
    }

    private static void SendTCPDataToAll(Packet packet)
    {
        packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.clients[i].tcp.SendData(packet);
        }
    }

    private static void SendTCPDataToAllExcept(int exceptClient, Packet packet)
    {
        packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != exceptClient)
            {
                Server.clients[i].tcp.SendData(packet);
            }
        }
    }
    #endregion

    #region UDP
    private static void SendUDPData(int toClient, Packet packet)
    {
        packet.WriteLength();
        Server.clients[toClient].udp.SendData(packet);
    }

    private static void SendUDPDataToAll(Packet packet)
    {
        packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.clients[i].udp.SendData(packet);
        }
    }

    private static void SendUDPDataToAllExcept(int exceptClient, Packet packet)
    {
        packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != exceptClient)
            {
                Server.clients[i].udp.SendData(packet);
            }
        }
    }
    #endregion
}
