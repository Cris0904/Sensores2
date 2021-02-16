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


    // Initialization
    void Start()
    {
        serialController = GameObject.Find("SerialControllerOne").GetComponent<SerialControllerOne>();
        state = States.init;
    }


    // Executed each frame
    void Update()
    {

        switch (state)
        {
            case States.init:
                byte[] msg = { 0x3E };

                serialController.SendSerialMessage(msg);


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
