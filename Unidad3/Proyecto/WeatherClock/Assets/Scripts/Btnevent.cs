using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Btnevent : MonoBehaviour
{
    public InputField port;
    public InputField rate;

    public comm main;
    public comm2 main2;

    // Start is called before the first frame update
    void Start()
    {
        main = GameObject.Find("GameManager").GetComponent<comm>();
        main2 = GameObject.Find("GameManager").GetComponent<comm2>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void Conectar()
    {
        main.ip = port.text;
        main2.ip = rate.text;

        main.cambiar = true;
        main2.cambiar = true;

    }
}
