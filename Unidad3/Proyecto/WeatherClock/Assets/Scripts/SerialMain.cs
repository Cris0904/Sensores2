using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class SerialMain : MonoBehaviour
{
    public SerialController serialController;
    public GameObject serialObject;
    public string port = "COM4";
    public int baudrate = 9600;
    public bool cambiar;
    bool leer;
    bool enviado;
    byte[] s = { 0x3E };
    public Text Hora;
    public Text Temp;
    public Text Alt;
    public Text hum;
    float secondsCounter = 0;
    float secondsToCount = 1;

        

    byte[] data = new byte[8];

    // Start is called before the first frame update
    void Start()
    {
        serialController = GameObject.Find("SerialController").GetComponent<SerialController>();
        serialObject = GameObject.Find("SerialController");
        cambiar = false;
        leer = false;
        enviado = false;
        DateTime date = DateTime.Now;
        /*data[0] = Convert.ToByte(date.ToString("hh"),16); Debug.Log(data[0].ToString("X2"));
        data[1] = Convert.ToByte(date.ToString("mm"),16); Debug.Log(data[1].ToString("X2"));
        data[2] = Convert.ToByte(date.ToString("ss"),16); Debug.Log(data[2].ToString("X2"));
        data[3] = Convert.ToByte(date.ToString("dd"),16); Debug.Log(data[3].ToString("X2"));
        data[4] = Convert.ToByte(date.ToString("MM"),16);
        data[5] = Convert.ToByte(date.ToString("yy"),16);     
        data[6] = 6;*/
        data[0] = 0x4A;
        data[1] = decToBcd(Convert.ToInt32(date.ToString("hh")));
        data[2] = decToBcd(Convert.ToInt32(date.ToString("mm")));
        data[3] = decToBcd(Convert.ToInt32(date.ToString("ss")));
        data[4] = decToBcd(Convert.ToInt32(date.ToString("dd")));
        data[5] = decToBcd(Convert.ToInt32(date.ToString("MM")));
        data[6] = decToBcd(Convert.ToInt32(date.ToString("yy")));
        data[7] = decToBcd(1);


    }

    byte decToBcd(int num) { 
    
        return (byte)(16 * (num / 10) + (num % 10));
    }

    // Update is called once per frame
    void Update()
    {
        secondsCounter += Time.deltaTime;
        if (cambiar)
        {
            serialController.SendSerialMessage(data); Debug.Log("Envia SetUp");

            serialController.portName = port;
            serialController.baudRate = baudrate;

            cambiar = false;
            leer = true;

            serialObject.SetActive(false);
            serialObject.SetActive(true);

        }

        if (leer)
        {
            if (enviado == false)
            {
                serialController.SendSerialMessage(s);//Debug.Log("Envia 3E");
                enviado = true; 
            }
            
            byte[] msg = (byte[])serialController.ReadSerialMessage();
            if(msg != null)
            {
                /*
                for (int i = 0; i < 6; i++) {
                    Debug.Log(msg[i].ToString("X2"));
                }*/

                Hora.text = msg[1] + ":" + msg[2] + ":" + msg[3];

                Temp.text = Math.Round(BitConverter.ToSingle(msg, 4),1).ToString() + " °C";
                Alt.text = Math.Round(BitConverter.ToSingle(msg, 8)).ToString() + " m";
                hum.text = Math.Round(BitConverter.ToSingle(msg, 12)).ToString();
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
