using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Contador2 : MonoBehaviour
{
    public int conteo;
    public bool funcionar;

    public Text contador;
    void Start()
    {
        conteo = 0;
        funcionar = true;
    }

    // Update is called once per frame
    void Update()
    { 
        contador.text = conteo.ToString();
    }

    public void sumar()
    {
        if (funcionar)
        {
            conteo = conteo + 1;
        }
    }

    public void terminar()
    {
        funcionar = false;
    }
}
