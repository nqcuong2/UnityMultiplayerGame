using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public enum ServerPackets
{
    Welcome = 1,
    ConnectionDenied,
    SendChatMsg,
    SpawnPlayer,
    PlayerPosition,
    SpawnBomb,
}

public enum ClientPackets
{
    WelcomeReceived = 1,
    SendChatMsg,
    PlayerMovement,
    SpawnBomb,
}

public class Packet : IDisposable
{
    private List<byte> buffer;
    private byte[] readableBuffer;
    private int readPos;

    #region Constructors
    public Packet()
    {
        InitializeMembers();
    }

    public Packet(int id)
    {
        InitializeMembers();

        Write(id);
    }

    public Packet(byte[] data)
    {
        InitializeMembers();

        SetBytes(data);
    }
    #endregion

    #region Functions
    private void InitializeMembers()
    {
        buffer = new List<byte>();
        readPos = 0;
    }

    public void SetBytes(byte[] data)
    {
        Write(data);
        readableBuffer = buffer.ToArray();
    }

    public void WriteLength()
    {
        buffer.InsertRange(0, BitConverter.GetBytes(buffer.Count));
    }

    public void InsertInt(int value)
    {
        buffer.InsertRange(0, BitConverter.GetBytes(value));
    }

    public byte[] ToArray()
    {
        readableBuffer = buffer.ToArray();
        return readableBuffer;
    }

    public int Length()
    {
        return buffer.Count;
    }

    public int UnreadLength()
    {
        return Length() - readPos;
    }

    public void Reset(bool shouldReset = true)
    {
        if (shouldReset) // Reset the whole packet
        {
            buffer.Clear();
            readableBuffer = null;
            readPos = 0;
        }
        else
        {
            readPos -= 4; // "Unread" the last read int
        }
    }
    #endregion

    #region Write Data
    public void Write(byte value)
    {
        buffer.Add(value);
    }

    public void Write(byte[] value)
    {
        buffer.AddRange(value);
    }

    public void Write(short value)
    {
        buffer.AddRange(BitConverter.GetBytes(value));
    }

    public void Write(int value)
    {
        buffer.AddRange(BitConverter.GetBytes(value));
    }

    public void Write(long value)
    {
        buffer.AddRange(BitConverter.GetBytes(value));
    }

    public void Write(float value)
    {
        buffer.AddRange(BitConverter.GetBytes(value));
    }

    public void Write(bool value)
    {
        buffer.AddRange(BitConverter.GetBytes(value));
    }

    public void Write(string value)
    {
        Write(value.Length);
        buffer.AddRange(Encoding.ASCII.GetBytes(value));
    }

    public void Write(Vector2 value)
    {
        Write(value.x);
        Write(value.y);
    }

    public void Write(Vector3 value)
    {
        Write(value.x);
        Write(value.y);
        Write(value.z);
    }

    public void Write(Quaternion value)
    {
        Write(value.x);
        Write(value.y);
        Write(value.z);
        Write(value.w);
    }
    #endregion

    #region Read Data
    public byte ReadByte(bool moveReadPos = true)
    {
        if (buffer.Count > readPos)
        {
            byte value = readableBuffer[readPos];
            if (moveReadPos)
            {
                readPos += 1;
            }
            return value;
        }
        else
        {
            throw new Exception("Could not read value of type 'byte'!");
        }
    }

    public byte[] ReadBytes(int length, bool moveReadPos = true)
    {
        if (buffer.Count > readPos)
        {
            byte[] value = buffer.GetRange(readPos, length).ToArray();
            if (moveReadPos)
            {
                readPos += length;
            }
            return value;
        }
        else
        {
            throw new Exception("Could not read value of type 'byte[]'!");
        }
    }

    public short ReadShort(bool moveReadPos = true)
    {
        if (buffer.Count > readPos)
        {
            short value = BitConverter.ToInt16(readableBuffer, readPos);
            if (moveReadPos)
            {
                readPos += 2;
            }
            return value;
        }
        else
        {
            throw new Exception("Could not read value of type 'short'!");
        }
    }

    public int ReadInt(bool moveReadPos = true)
    {
        if (buffer.Count > readPos)
        {
            int value = BitConverter.ToInt32(readableBuffer, readPos);
            if (moveReadPos)
            {
                readPos += 4;
            }
            return value;
        }
        else
        {
            throw new Exception("Could not read value of type 'int'!");
        }
    }

    public long ReadLong(bool moveReadPos = true)
    {
        if (buffer.Count > readPos)
        {
            long value = BitConverter.ToInt64(readableBuffer, readPos);
            if (moveReadPos)
            {
                readPos += 8;
            }
            return value;
        }
        else
        {
            throw new Exception("Could not read value of type 'long'!");
        }
    }

    public float ReadFloat(bool moveReadPos = true)
    {
        if (buffer.Count > readPos)
        {
            float value = BitConverter.ToSingle(readableBuffer, readPos);
            if (moveReadPos)
            {
                readPos += 4;
            }
            return value;
        }
        else
        {
            throw new Exception("Could not read value of type 'float'!");
        }
    }

    public bool ReadBool(bool moveReadPos = true)
    {
        if (buffer.Count > readPos)
        {
            bool value = BitConverter.ToBoolean(readableBuffer, readPos);
            if (moveReadPos)
            {
                readPos += 1;
            }
            return value;
        }
        else
        {
            throw new Exception("Could not read value of type 'bool'!");
        }
    }

    public string ReadString(bool moveReadPos = true)
    {
        try
        {
            int length = ReadInt();
            string value = Encoding.ASCII.GetString(readableBuffer, readPos, length);
            if (moveReadPos && value.Length > 0)
            {
                readPos += length;
            }
            return value;
        }
        catch
        {
            throw new Exception("Could not read value of type 'string'!");
        }
    }

    public Vector2 ReadVector2(bool moveReadPos = true)
    {
        return new Vector2(ReadFloat(moveReadPos), ReadFloat(moveReadPos));
    }

    public Vector3 ReadVector3(bool moveReadPos = true)
    {
        return new Vector3(ReadFloat(moveReadPos), ReadFloat(moveReadPos), ReadFloat(moveReadPos));
    }
    #endregion

    private bool disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                buffer = null;
                readableBuffer = null;
                readPos = 0;
            }

            disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
