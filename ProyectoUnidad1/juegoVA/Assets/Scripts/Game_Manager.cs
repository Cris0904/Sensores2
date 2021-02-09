using System.Collections;
using UnityEngine;
using System;

public class Game_Manager : MonoBehaviour
{
    [SerializeField]
    public Control_Meteoritos meteoritosManager; //Es una referencia al generador de meteoritos

    [SerializeField]
    public Jugador jugador; //Es una referencia al jugador

    //Estos dos son eventos para cambiar los puntajes
    public Action<int> CambioDePuntaje;
    public Action<int> CambioDeRecord;

    public int Record { get => PlayerPrefs.GetInt("RECORD"); set => PlayerPrefs.SetInt("RECORD", value); } //Guardo la variable del record de forma permanente

    public int puntaje = 0;

    public float conteo = 0;

    public AudioSource AudioNave;

    public AudioSource AudioMuerte;

    public AudioSource MusicaJuego;

    public AudioSource MusicaMuerte;

    public bool contar = true;

    private IEnumerator Start()
    {
        jugador.MeMori += MuereElJugador;//Aqui escucho al jugador morirse
        yield return new WaitForEndOfFrame(); //Esto es para evitar errores (que se solicite información antes de que exista)
        CambioDeRecord?.Invoke(Record); //Este es evento que actualiza el record al principio de la partida
        meteoritosManager.Generar = true; //Aqui le digo que comience a generar meteoritos
        MusicaJuego.Play();
        AudioNave.Play();
    }

    private void Update()
    {
        if (!contar)
        {
            AudioNave.Stop();
            MusicaJuego.Stop();
            return; //Sino esta contando

        }
        MusicaMuerte.Stop();
        conteo += Time.deltaTime;//Este conteo es para ver cada cuanto pasa un segundo

        if(conteo > 1)
        {
            conteo = 0;
            puntaje++; //Aqui aumento el puntaje
            if(puntaje > Record) //Aqui si supero el record lo actualizo
            {
                Record = puntaje;
                CambioDeRecord?.Invoke(Record);
            }
            CambioDePuntaje?.Invoke(puntaje);
        }
    }

    private void MuereElJugador() //Cuando muere el jugador
    {
        contar = false; //Para el puntaje
        meteoritosManager.Generar = false; //Dejo de generar meteoritos
        AudioMuerte.Play();
        MusicaMuerte.Play();
        MusicaJuego.Stop();
    }
}
