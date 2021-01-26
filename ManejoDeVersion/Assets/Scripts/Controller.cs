using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    // Start is called before the first frame update

    public Contador1 Contado1;
    public Contador2 Contado2;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void terminar()
    {
        Contado1.terminar();
        Contado2.terminar();
    }
}
