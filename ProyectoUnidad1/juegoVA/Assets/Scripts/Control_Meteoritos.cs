using System.Collections;
using UnityEngine;

public class Control_Meteoritos : MonoBehaviour
{
    [SerializeField]
    Meteorito meteorito; //Prefab del meteorito que va a instanciar 

    [SerializeField]
    Transform[] puntosDeOrigen = new Transform[0]; //Puntos desde donde salen los meteoritos

    [SerializeField]
    Transform puntoFinal; //El punto hacia donde van a ir los meteoritos

    [SerializeField]
    float intervalo, varianzaDeIntervalo; //Intervalo cada cuanto salen y que no salgan al mismo tiempo

    [SerializeField]
    float velocidad, varianzaDeVelocidad; //Esto para que salgan con distintas velocidades

    public bool Generar { get; set; } //Esto es para activar y desactivar la salida de los meteoritos 

    private void Start()
    {
        StartCoroutine(GenerarMeteoro()); //Inicia la coroutina que instancia a los meteoritos
    }

    IEnumerator GenerarMeteoro()
    {
        if (Generar) //Reviso si genera o no
        {
            if(puntosDeOrigen.Length > 0 && meteorito != null) //Este if es para evitar errores
            {
                Meteorito meteoroActual = Instantiate(meteorito, puntosDeOrigen[Random.Range(0, puntosDeOrigen.Length)].position, Quaternion.identity); //Instancio un nuevo meteorito y lo meto en una variable
                meteoroActual.Target = puntoFinal.position; //Aqui le digo hacia donde debe ir  
                meteoroActual.Speed = velocidad + Random.Range(-varianzaDeVelocidad, varianzaDeVelocidad); //Aqui le digo con que velocidad debe ir 
            }
            yield return new WaitForSeconds(intervalo + Random.Range(-varianzaDeIntervalo, varianzaDeIntervalo));//Espero un tiempo para instanciar un nuevo meteorito
        }
        else
        {
            yield return new WaitForEndOfFrame();//Sino esta generando para que se llame en el siguiente frame
        }
        StartCoroutine(GenerarMeteoro());//Se vuelve a llamar la coroutina
    }
}
