using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Contador1 : MonoBehaviour
{
    // Start is called before the first frame update

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
        if (funcionar)
        {
            //conteo = conteo + (int)Mathf.Round(Time.deltaTime);
            conteo = conteo + 1;
        }

        if(conteo == 100)
        {
            conteo = 0;
        }

        contador.text= conteo.ToString();
    }

    public void terminar()
    {
        funcionar = false;
    }
}
