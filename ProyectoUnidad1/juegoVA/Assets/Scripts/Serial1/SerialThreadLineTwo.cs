﻿/**
 * Ardity (Serial Communication for Arduino + Unity)
 * Author: Daniel Wilches <dwilches@gmail.com>
 *
 * This work is released under the Creative Commons Attributions license.
 * https://creativecommons.org/licenses/by/2.0/
 */

using UnityEngine;

using System.IO.Ports;

/**
 * This class contains methods that must be run from inside a thread and others
 * that must be invoked from Unity. Both types of methods are clearly marked in
 * the code, although you, the final user of this library, don't need to even
 * open this file unless you are introducing incompatibilities for upcoming
 * versions.
 * 
 * For method comments, refer to the base class.
 */
public class SerialThreadLineTwo : AbstractSerialThreadTwo
{
    private byte[] buffer = new byte[1024];

    public SerialThreadLineTwo(string portName,
                             int baudRate,
                             int delayBeforeReconnecting,
                             int maxUnreadMessages)
        : base(portName, baudRate, delayBeforeReconnecting, maxUnreadMessages, false)
    {
    }

    protected override void SendToWire(object message, SerialPort serialPort)
    {
        serialPort.Write((string)message + '\n');
    }

    protected override object ReadFromWire(SerialPort serialPort)
    {
        return serialPort.ReadLine();
    }
}
