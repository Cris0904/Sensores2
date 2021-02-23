﻿/**
 * Ardity (Serial Communication for Arduino + Unity)
 * Author: Daniel Wilches <dwilches@gmail.com>
 *
 * This work is released under the Creative Commons Attributions license.
 * https://creativecommons.org/licenses/by/2.0/
 */

using UnityEngine;
using System.Collections;
using System;

/**
 * Sample for reading using polling by yourself, and writing too.
 */
public class SerialOne : MonoBehaviour
{

    
    public Game_Manager gameManager;

    public GameObject panelDeMuerte;

    public SerialControllerOne serialController;
    public SerialControllerTwo serialControllerTwo;
    enum States { init, active, inactive };
    enum StatesActive { init, active, inactive };
    States state;
    StatesActive statesActive;
    bool enviar=true;
    bool enviar2=true;
    public bool funcionando;
    public int posicion;

    // Initialization
    void Start()
    {
        serialController = GameObject.Find("SerialControllerOne").GetComponent<SerialControllerOne>();
        serialControllerTwo = GameObject.Find("SerialControllerTwo").GetComponent<SerialControllerTwo>();
        gameManager = GameObject.Find("Manager").GetComponent<Game_Manager>();
        state = States.init;
        statesActive = StatesActive.init;
        //enviar = true;
        //enviar2 = true;
        posicion = 0;
        funcionando = false;
    }


    // Executed each frame
    void Update()
    {

        switch (state)
        {
            case States.init:

                if (enviar)
                {
                    byte[] s = { 0x3E };
                    serialController.SendSerialMessage(s);
                    Debug.Log("Envia 3e");
                    enviar = false;

                }
                object rc = serialController.ReadSerialMessage();
                Debug.Log(rc);

                if (rc == null)
                {
                    Debug.Log("Inicio Fallido, Reintentando");
                    
                }
                else
                {
                    byte[] rcv = (byte[])rc;
                    
                    if (rcv[0] == 0x4A)
                    {
                        state = States.active;
                        Debug.Log("Inicio Exitoso");
                        funcionando = true;
                    }
                    else
                    {
                        Debug.Log("Inicio incorrecto, Reintentando");
                        enviar = true;
                        Debug.Log(rcv[0]);
                    }
                }
                break;
            case States.active:
                Debug.Log("Activado");
                byte[] msg = (byte[])serialController.ReadSerialMessage();
                funcionando = true;
                //SceneManager.LoadScene(0);
                gameManager.contar = true;
                gameManager.conteo = 0;
                panelDeMuerte.SetActive(false);
                gameManager.meteoritosManager.Generar = true;
                gameManager.puntaje = 0;
                gameManager.jugador.moverse = true;
                gameManager.AudioNave.Play();
                gameManager.MusicaJuego.Play();

                if (msg == null)
                {
                    
                }
                else
                {
                    if (msg[0] == 0x3D)
                    {
                        state = States.inactive;
                        Debug.Log("Desactivar");
                    }
                }
                    break;
            case States.inactive:
                Debug.Log("Desactivado");
                funcionando = false;
                byte[] msg2 = (byte[])serialController.ReadSerialMessage();

                if (msg2 == null)
                {

                }
                else
                {
                    if (msg2[0] == 0x3D)
                    {
                        state = States.active;
                        Debug.Log("Activar");
                    }
                }
                break;
            default:
                break;

        }
        

        switch (statesActive)
        {
            case StatesActive.init:
                

                if (enviar2)
                {
                    string s = "on";
                    serialControllerTwo.SendSerialMessage(s);
                    enviar2 = false;
                }
                string rcv = (string)serialControllerTwo.ReadSerialMessage();

                if (rcv == null)
                {
                    Debug.Log("Inicio Fallido2, Reintentando");
                    enviar = true;
                }
                else
                {
                    if (Equals(rcv, "on\n"))
                    {
                        statesActive = StatesActive.active;
                        Debug.Log("Inicio Exitoso2");
                        funcionando = true;
                    }
                    else
                    {
                        Debug.Log("Inicio Fallido2, Reintentando");
                        enviar2 = true;
                    }
                }

                break;
            case StatesActive.active:
                Debug.Log("Detección de movimiento activado");
                string msg = (string)serialControllerTwo.ReadSerialMessage();

                if(msg != null)
                {
                    if(msg[0] == 'n')
                    {
                        string value = msg.Trim('n');
                        posicion = Convert.ToInt32(value);
                    }
                }

                break;
            case StatesActive.inactive:
                Debug.Log("Desactivado");
                string end = "off";
                serialControllerTwo.SendSerialMessage(end);

                break;
        }
        

    }
}
