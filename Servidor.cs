using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;
public class Servidor : MonoBehaviour
{
    private Socket sock;
    private Socket[] con_socket;
    public static readonly int port = 1232;
    private IPAddress addr = IPAddress.Parse("192.168.0.111");
    private byte[][] dataRcvBuf = new byte[2][] { new byte[8000], new byte[8000] };
    public void Start()
    {
        sock = new Socket(
            addr.AddressFamily,
            SocketType.Stream,
            ProtocolType.Tcp);
        sock.Bind(new IPEndPoint(addr, Servidor.port));
        sock.Listen(4096);
        sock.BeginAccept(OnConnectRequest, sock);
        con_socket = new Socket[2];
    }
    
    private void OnConnectRequest(IAsyncResult result)
    {
        if (con_socket[0] == null)
        {
            con_socket[0] = sock.EndAccept(result);
            con_socket[0].BeginReceive(
                    dataRcvBuf[0], 0,
                    dataRcvBuf[0].Length,
                    SocketFlags.None,
                    new AsyncCallback(OnBytesReceived0),
                    con_socket[0]);
        }
        else
        {
            con_socket[1] = sock.EndAccept(result);
            con_socket[1].BeginReceive(
                    dataRcvBuf[1], 0,
                    dataRcvBuf[1].Length,
                    SocketFlags.None,
                    new AsyncCallback(OnBytesReceived1),
                    this);
        }
        sock.BeginAccept(OnConnectRequest, sock);
    }
    protected void OnBytesReceived1(IAsyncResult result)
    {
        con_socket[0].Send(dataRcvBuf[1]);
        Parallel.For(0, con_socket[1].EndReceive(result) / 5, (i, loopState) =>
        {
            switch ((InformationType)dataRcvBuf[1][i * 5])
            {
                case InformationType.TransformPositionX:
                    //ejemplo: float ntransformpositionx = BitConverter.ToSingle(dataRcvBuf[1], i * 5 + 1);
                    break;
                case InformationType.TransformPositionZ:
                    //ejemplo: float ntransformpositionz = BitConverter.ToSingle(dataRcvBuf[1], i * 5 + 1);
                    break;
                case InformationType.SpeedX:
                    //ejemplo: float speedx = BitConverter.ToSingle(dataRcvBuf[1], i * 5 + 1);
                    break;
                case InformationType.SpeedZ:
                    //ejemplo: float speeedz = BitConverter.ToSingle(dataRcvBuf[1], i * 5 + 1);
                    break;
                case InformationType.TransformRotationY:
                    //ejemplo: float ntransformrotationy = BitConverter.ToSingle(dataRcvBuf[1], i * 5 + 1);
                    break;
                case InformationType.SpecialMovements:
                    //ejemplo: int specialmovements = BitConverter.ToInt32(dataRcvBuf[1], i * 5 + 1);
                    break;
                case InformationType.PlayerType:
                    //ejemplo: int playertype = BitConverter.ToInt32(dataRcvBuf[1], i * 5 + 1);
                    break;
            }
        });
        con_socket[1].BeginReceive(
        dataRcvBuf[1], 0,
        dataRcvBuf[1].Length,
        SocketFlags.None,
        new AsyncCallback(OnBytesReceived1),
        this);

    }
    protected void OnBytesReceived0(IAsyncResult result)
    {
        con_socket[1].Send(dataRcvBuf[0]);
        Parallel.For(0, con_socket[0].EndReceive(result) / 5, (i, loopState) =>
        {
            switch ((InformationType)dataRcvBuf[0][i * 5])
            {
                case InformationType.TransformPositionX:
                    //ejemplo: float ntransformpositionx = BitConverter.ToSingle(dataRcvBuf[0], i * 5 + 1);
                    break;
                case InformationType.TransformPositionZ:
                    //ejemplo: float ntransformpositionz = BitConverter.ToSingle(dataRcvBuf[0], i * 5 + 1);
                    break;
                case InformationType.SpeedX:
                    //ejemplo: float speedx = BitConverter.ToSingle(dataRcvBuf[0], i * 5 + 1);
                    break;
                case InformationType.SpeedZ:
                    //ejemplo: float speeedz = BitConverter.ToSingle(dataRcvBuf[0], i * 5 + 1);
                    break;
                case InformationType.TransformRotationY:
                    //ejemplo: float ntransformrotationy = BitConverter.ToSingle(dataRcvBuf[0], i * 5 + 1);
                    break;
                case InformationType.SpecialMovements:
                    //ejemplo: int specialmovements = BitConverter.ToInt32(dataRcvBuf[0], i * 5 + 1);
                    break;
                case InformationType.PlayerType:
                    //ejemplo: int playertype = BitConverter.ToInt32(dataRcvBuf[0], i * 5 + 1);
                    break;
            }
        });
        con_socket[0].BeginReceive(
        dataRcvBuf[0], 0,
        dataRcvBuf[0].Length,
        SocketFlags.None,
        new AsyncCallback(OnBytesReceived0),
        this);

    }
    
    private void Update()
    {

    }
}
