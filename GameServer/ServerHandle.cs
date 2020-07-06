using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{
    class ServerHandle
    {
        public static void WelcomeReceived(int fromClient, Packet packet)
        {
            int clientIdCheck = packet.ReadInt();
            string username = packet.ReadString();

            Console.WriteLine($"{Server.clients[fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {fromClient}.");
            
            // Should not occur otherwise sth goes wrong
            if (fromClient != clientIdCheck)
            {
                Console.WriteLine($"Plaer \"{username}\" (ID: {fromClient}) has assumed the wrong client ID ({clientIdCheck})!");
            }

            // TODO: send player into game
        }
    }
}
