using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace GameServer
{
    class Player
    {
        public int id;
        public string username;

        public Vector3 position;
        public Quaternion rotation;

        private float moveSpeed = 5f / Constants.TICKS_PER_SEC;
        private bool[] inputs;

        public Player(int id, string username, Vector3 spawnPos)
        {
            this.id = id;
            this.username = username;
            position = spawnPos;
            rotation = Quaternion.Identity;

            inputs = new bool[4];
        }

        public void SetInput(bool[] inputs, Quaternion rotation)
        {
            this.inputs = inputs;
            this.rotation = rotation;
        }

        public void Update()
        {
            Vector2 inputDir = Vector2.Zero;
            if (inputs[0])
            {
                inputDir.Y += 1;
            }
            if (inputs[1])
            {
                inputDir.Y -= 1;
            }
            if (inputs[2])
            {
                inputDir.X += 1;
            }
            if (inputs[3])
            {
                inputDir.X -= 1;
            }

            Move(inputDir);
        }

        private void Move(Vector2 inputDir)
        {
            Vector3 forward = Vector3.Transform(new Vector3(0, 0, 1), rotation);
            Vector3 right = Vector3.Normalize(Vector3.Cross(forward, new Vector3(0, 1, 0)));

            Vector3 moveDir = right * inputDir.X + forward * inputDir.Y;
            position += moveDir * moveSpeed;

            ServerSend.PlayerPosition(this);
            ServerSend.PlayerRotation(this);
        }
    }
}
