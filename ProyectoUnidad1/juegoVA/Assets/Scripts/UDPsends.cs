using System.Collections;
using System;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

public class UDPsends : MonoBehaviour
{
    Thread recieveThread;
    UdpClient client;
    int port;

    public float x = 0;


    // Start is called before the first frame update
    void Start()
    {
        port = 5065;
        Initialization();

    }

    void Initialization()
    {
        recieveThread = new Thread(new ThreadStart(Recieve));
        recieveThread.IsBackground = true;
        recieveThread.Start();

    }

    void Recieve()
    {
        client = new UdpClient(port);
        while (true)
        {
            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Parse("0.0.0.0"), port);
                byte[] data = client.Receive(ref anyIP);

                string text = Encoding.UTF8.GetString(data);
                x = float.Parse(text);
                Debug.Log(x + " ------");

            }
            catch (Exception e)
            {
                print(e.ToString());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnApplicationQuit()
    {
        if (recieveThread.IsAlive)
         {
             recieveThread.Abort();
             Debug.Log(recieveThread.IsAlive); //true (must be false)
         }
         else
         {
             Debug.Log("Dead duck on Thanksgiving table");
         }
    }
}
