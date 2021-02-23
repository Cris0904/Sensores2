using UnityEngine;
using System;
using System.Collections.Specialized;
using System.Security.Cryptography;
//using System.Diagnostics;

public class Jugador : MonoBehaviour
{
    public Action MeMori;
    public Animator animacion;
    public SerialOne GameController;
    public bool moverse = true;

    private void Start()
    {
        //UDPsend = UDPsend.GetComponent<UDPsends>();
        GameController = GameObject.Find("SerialOne").GetComponent<SerialOne>();
    }

    private void FixedUpdate()
    {
        if (moverse)
        {
            
            float mov = GameController.posicion;
            mov = scale(0, 1023, -40, 40, mov);
            Vector3 posicion = new Vector3(mov, -3, -3);
   
            if(Vector3.Distance(posicion, transform.localPosition) > 1f)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, posicion, Time.deltaTime);
            }
            
            
            //transform.position = new Vector3(mov, 0, 0);
        }

        if (!GameController.funcionando)
        {
            
            MeMori?.Invoke(); //Llamo el evento MeMori
            moverse = false;  //Y me dejo de mover porque me mori
            animacion.SetBool("vivo", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)//Cuando detecta el choque de un meteorito llama el evento MeMori
    {
        Meteorito obj = collision.GetComponent<Meteorito>();

        if(obj != null) //Si el componente no es nulo (significa que golpee un meteorito) 
        {
            GameController.funcionando = false;
            MeMori?.Invoke(); //Llamo el evento MeMori
            moverse = false;  //Y me dejo de mover porque me mori
            animacion.SetBool("vivo", false);
        }
    }

    public float scale(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
    {

        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        return (NewValue);
    }
}