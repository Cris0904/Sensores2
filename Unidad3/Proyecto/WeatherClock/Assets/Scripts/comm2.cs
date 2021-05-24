using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using System;
using UnityEngine.UI;

public class comm2 : MonoBehaviour
{

    private static comm2 instance;
    private Thread receiveThread;
    private UdpClient receiveClient;
    private IPEndPoint receiveEndPoint;
    public string ip = "192.168.1.12";
    public int receivePort = 32004;
    public int sendPort = 32003;
    private IPEndPoint sendEndPoint;
    private bool isInitialized;

    private UdpClient sendClient;
    public string ipremote = "192.168.1.18";
    private Queue receiveQueue;


    public bool cambiar;
    bool leer;
    bool enviado;
    byte[] s = { 0x3E };
    public Text Temp;
    public Text Alt;
    public Text hum;
    float secondsCounter = 0;
    float secondsToCount = 5;

    byte[] data = new byte[8];
    /*
     private void Awake()
    {
        Initialize();
    }
    */
    byte decToBcd(int num)
    {
        return (byte)(16 * (num / 10) + (num % 10));
    }

    private void Start()
    {

        cambiar = false;
        leer = false;
        enviado = false;
        /*DateTime date = DateTime.Now;
        data[0] = Convert.ToByte(date.ToString("hh"),16); Debug.Log(data[0].ToString("X2"));
        data[1] = Convert.ToByte(date.ToString("mm"),16); Debug.Log(data[1].ToString("X2"));
        data[2] = Convert.ToByte(date.ToString("ss"),16); Debug.Log(data[2].ToString("X2"));
        data[3] = Convert.ToByte(date.ToString("dd"),16); Debug.Log(data[3].ToString("X2"));
        data[4] = Convert.ToByte(date.ToString("MM"),16);
        data[5] = Convert.ToByte(date.ToString("yy"),16);     
        data[6] = 6;*/
    }
    /*
    private void Initialize()
    {
        instance = this;
        receiveEndPoint = new IPEndPoint(IPAddress.Parse(ip), receivePort);
        receiveClient = new UdpClient(receivePort);
        receiveQueue = Queue.Synchronized(new Queue());
        receiveThread = new Thread(new ThreadStart(ReceiveDataListener));
        sendClient = new UdpClient(sendPort);
        receiveThread.IsBackground = true;
        receiveThread.Start();
        isInitialized = true;
        sendEndPoint = new IPEndPoint(IPAddress.Parse(ipremote), sendPort);
        sendClient.Connect(sendEndPoint);
    }*/

    private void ReceiveDataListener()
    {
        while (true)
        {
            try
            {
                byte[] data = receiveClient.Receive(ref receiveEndPoint);
                for (int i = 0; i < data.Length; i++)
                {
                    receiveQueue.Enqueue(data[i]);
                }
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.ToString());
            }
        }
    }




    private void OnDestroy()
    {
        TryKillThread();
    }

    private void OnApplicationQuit()
    {
        TryKillThread();
    }

    private void TryKillThread()
    {
        if (isInitialized)
        {
            receiveThread.Abort();
            receiveThread = null;
            receiveClient.Close();
            receiveClient = null;
            sendClient.Close();
            sendClient = null;
            Debug.Log("Thread killed");
            isInitialized = false;
        }
    }

    void Update()
    {

        secondsCounter += Time.deltaTime;

        if (cambiar)
        {

            instance = this;
            receiveEndPoint = new IPEndPoint(IPAddress.Parse(ip), receivePort);
            receiveClient = new UdpClient(receivePort);
            receiveQueue = Queue.Synchronized(new Queue());
            receiveThread = new Thread(new ThreadStart(ReceiveDataListener));
            receiveThread.IsBackground = true;
            sendClient = new UdpClient(sendPort);
            receiveThread.Start();
            isInitialized = true;
            sendEndPoint = new IPEndPoint(IPAddress.Parse(ipremote), sendPort);
            sendClient.Connect(sendEndPoint);

            cambiar = false;
            leer = true;
            Debug.Log("Cambia");
        }

        if (leer)
        {
            if (enviado == false)
            {
                sendClient.Send(s, 1);
                enviado = true;
                Debug.Log("Envia 3e");
            }

            if (receiveQueue.Count > 11)
            {
                Debug.Log("Recibe 12 bytes");
                byte[] msg = new byte[12];

                for (int i = 0; i < msg.Length; i++)
                {
                    msg[i] = (byte)receiveQueue.Dequeue();
                }

                
                Temp.text = Math.Round(BitConverter.ToSingle(msg, 0), 1).ToString() + " °C";
                Alt.text = Math.Round(BitConverter.ToSingle(msg, 4)).ToString() + " m";
                hum.text = Math.Round(BitConverter.ToSingle(msg, 8)).ToString();

            }

            if (enviado)
            {
                if (secondsToCount <= secondsCounter)
                {
                    secondsCounter = 0;
                    enviado = false;
                }
            }
        }

    }

}
