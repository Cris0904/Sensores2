/**
 * Ardity (Serial Communication for Arduino + Unity)
 * Author: Daniel Wilches <dwilches@gmail.com>
 *
 * This work is released under the Creative Commons Attributions license.
 * https://creativecommons.org/licenses/by/2.0/
 */

using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

/**
 * Sample for reading using polling by yourself, and writing too.
 */
public class SerialOne : MonoBehaviour
{


    public InputField puerto1;
    public InputField rate1; 
    
    public InputField puerto2;
    public InputField rate2;

    public GameObject serialObject1;
    public GameObject serialObject2;

    public SerialControllerOne serialController;
    public SerialControllerTwo serialControllerTwo;
    enum States { init, active, inactive };
    enum StatesActive { init, active, inactive };
    States state;
    StatesActive statesActive;
    bool enviar=true;
    bool enviar2=true;
    bool onoff = true;
    public bool funcionando;
    public int posicion;

    // Initialization
    void Start()
    {
        serialController = GameObject.Find("SerialControllerOne").GetComponent<SerialControllerOne>();
        serialControllerTwo = GameObject.Find("SerialControllerTwo").GetComponent<SerialControllerTwo>();

        serialObject1 = GameObject.Find("SerialControllerOne");
        serialObject2 = GameObject.Find("SerialControllerTwo");
        
        state = States.init;
        statesActive = StatesActive.init;
        posicion = 0;
        funcionando = false;
    }


    // Executed each frame
    void Update()
    {
        if (serialController.portName != puerto1.text || serialController.baudRate != Convert.ToInt32(rate1.text) || serialControllerTwo.portName != puerto2.text || serialControllerTwo.baudRate != Convert.ToInt32(rate2.text)  ){
            serialController.portName = puerto1.text;
            serialController.baudRate = Convert.ToInt32(rate1.text);

            serialControllerTwo.portName = puerto2.text;
            serialControllerTwo.baudRate = Convert.ToInt32(rate2.text);
            
            serialController.SendSerialMessage(new byte[] { 0x4a });
            serialControllerTwo.SendSerialMessage("off");

            //serialObject1.SetActive(false);
            serialObject2.SetActive(false);
            //serialObject1.SetActive(true);
            serialObject2.SetActive(true);

            state = States.init;
            statesActive = StatesActive.init;   
        }

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
                    enviar = true;
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
                        Debug.Log(rcv[0].ToString("X2"));
                    }
                }
                break;
            case States.active:
                Debug.Log("Activado");
                byte[] msg = (byte[])serialController.ReadSerialMessage();
                funcionando = true;
                //SceneManager.LoadScene(0);
                

                if (msg == null)
                {
                    
                }
                else
                {
                    if (msg[0] == 0x3D)
                    {
                        state = States.inactive;
                        Debug.Log("Desactivar");
                        onoff = true;
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
                    Debug.Log("Envío on");
                    enviar2 = false;
                }
                string rcvs = (string)serialControllerTwo.ReadSerialMessage();

                if (rcvs == null)
                {
                    Debug.Log("Inicio Fallido2 Null, Reintentando");
                    enviar2 = true;
                }
                else
                {
                    if (rcvs.Equals("on"))
                    {   
                        statesActive = StatesActive.active;
                        Debug.Log("Inicio Exitoso2");
                        funcionando = true;
                    }
                    else
                    {
                        Debug.Log("Inicio Incorrecto2, Reintentando" + rcvs);
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
