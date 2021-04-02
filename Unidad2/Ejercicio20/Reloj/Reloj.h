#ifndef Reloj_h
#define Reloj_h

#include "Arduino.h"


class Reloj
{
    public:
        Reloj(int _DS1307, int hora, int min, int sec, int dia, int mes, int ano, int sdia);
        void OnOff();
        void ReadTime();
    private:    
        int DS1307;
        int hour;
        int minute;
        int second;
        int year;
        int month;
        int monthday;
        int weekday;
        byte decToBcd(byte val);
        byte bcdToDec(byte val);
        void setTime();


};

#endif