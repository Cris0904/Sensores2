/**
 * Ardity (Serial Communication for Arduino + Unity)
 * Author: Daniel Wilches <dwilches@gmail.com>
 *
 * This work is released under the Creative Commons Attributions license.
 * https://creativecommons.org/licenses/by/2.0/
 */

using UnityEngine;
using System.Collections;

/**
 * Sample for reading using polling by yourself, and writing too.
 */
public class SerialOne : MonoBehaviour
{
    public SerialControllerOne serialController;
    enum States { init, active, inactive };
    States state;
    bool enviar;

    // Initialization
    void Start()
    {
        serialController = GameObject.Find("SerialControllerOne").GetComponent<SerialControllerOne>();
        state = States.init;
        enviar = true;
    }


    // Executed each frame
    void Update()
    {

        switch (state)
        {
            case States.init:

                if (enviar)
                {
                    byte[] msg = { 0x3E };
                    serialController.SendSerialMessage(msg);
                    enviar = false;
                }
                byte[] rcv = (byte[])serialController.ReadSerialMessage();

                if(rcv[0] == 0x4A)
                {
                    state = States.active;
                }
                else
                {
                    Debug.Log("Inicio Fallido, Reintentando");
                    enviar = true;
                }
                break;
            case States.active:
                break;
            case States.inactive:
                break;
            default:
                break;

        }

        //---------------------------------------------------------------------
        // Send data
        //---------------------------------------------------------------------

        // If you press one of these keys send it to the serial device. A
        // sample serial device that accepts this input is given in the README.


        //---------------------------------------------------------------------
        // Receive data
        //---------------------------------------------------------------------

        string message = serialController.ReadSerialMessage();

        if (message == null)
            return;

        
    }
}
