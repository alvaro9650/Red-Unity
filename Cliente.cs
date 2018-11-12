using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;
public class Cliente : MonoBehaviour
{
    private Socket sock=new Socket(
            IPAddress.Any.AddressFamily,
            SocketType.Stream,
            ProtocolType.Tcp);
    Byte[] dataRcvBuf = new Byte[8000];
    public void Start()
    {
        Parallel.For(0, 255, (i) =>
        {
            ConnectTo(new IPAddress(43200 + i * 16777216L));
        });
    }
    private void ConnectTo(IPAddress ip)
    {
        Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        SocketAsyncEventArgs e = new SocketAsyncEventArgs();
        e.RemoteEndPoint = new IPEndPoint(ip, Servidor.port);
        e.UserToken = s;
        e.Completed += new EventHandler<SocketAsyncEventArgs>(e_Completed);
        s.ConnectAsync(e);
    }

    private void e_Completed(object sender, SocketAsyncEventArgs e)
    {
        if (e.ConnectSocket != null && e.ConnectSocket.Connected)
        {
            sock = e.ConnectSocket;
            sock.BeginReceive(
                dataRcvBuf, 0,
                dataRcvBuf.Length,
                SocketFlags.None,
                new AsyncCallback(OnBytesReceived),
                this);
        }
    }
    protected void OnBytesReceived(IAsyncResult result)
    {
        Parallel.For(0, sock.EndReceive(result) / 5, (i, loopState) =>
          {
              switch ((InformationType)dataRcvBuf[i * 5])
              {
                  case InformationType.TransformPositionX:
                      //ejemplo: float ntransformpositionx = BitConverter.ToSingle(dataRcvBuf, i * 5 + 1);
                      break;
                  case InformationType.TransformPositionZ:
                      //ejemplo: float ntransformpositionz = BitConverter.ToSingle(dataRcvBuf, i * 5 + 1);
                      break;
                  case InformationType.SpeedX:
                      //ejemplo: float speedx = BitConverter.ToSingle(dataRcvBuf, i * 5 + 1);
                      break;
                  case InformationType.SpeedZ:
                      //ejemplo: float speeedz = BitConverter.ToSingle(dataRcvBuf, i * 5 + 1);
                      break;
                  case InformationType.TransformRotationY:
                      //ejemplo: float ntransformrotationy = BitConverter.ToSingle(dataRcvBuf, i * 5 + 1);
                      break;
                  case InformationType.SpecialMovements:
                      //ejemplo: int specialmovements = BitConverter.ToInt32(dataRcvBuf, i * 5 + 1);
                      break;
                  case InformationType.PlayerType:
                      //ejemplo: int playertype = BitConverter.ToInt32(dataRcvBuf, i * 5 + 1);
                      break;
              }
          });
        sock.BeginReceive(
                dataRcvBuf, 0,
                dataRcvBuf.Length,
                SocketFlags.None,
                new AsyncCallback(OnBytesReceived),
                this);

    }
    void Update()
    {
        /*ejemplo:
        byte[] outb = new byte[5];
        int i = 1;
        foreach (byte e in BitConverter.GetBytes(transform.position.x))
        {
            outb[i++] = e;
        }
        outb[0] = 0x01;
        sock.Send(outb);*/
    }
}
