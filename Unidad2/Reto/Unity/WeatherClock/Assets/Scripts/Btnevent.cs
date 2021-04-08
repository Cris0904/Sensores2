using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Btnevent : MonoBehaviour
{
    public InputField port;
    public InputField rate;

    public SerialMain main;

    // Start is called before the first frame update
    void Start()
    {
        main = GameObject.Find("GameManager").GetComponent<SerialMain>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void Conectar()
    {
        main.port = port.text;
        main.baudrate = Convert.ToInt32(rate.text);

        main.cambiar = true;
    }
}
