using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class Canvas_Control : MonoBehaviour
{
    [SerializeField]
    Game_Manager gameManager;

    [SerializeField]
    Jugador jugador;

    [SerializeField]
    GameObject panelDeMuerte;

    [SerializeField]
    TextMeshProUGUI puntajeInGame, puntajeMuerte, Record;

    [SerializeField]
    Button jugar, salir;

    
    public GameObject panelDeInicio;
    

    private void Start()
    {
        
    }

    public void Comenzar()
    {
        jugador.MeMori += CambiarPantalla;
        gameManager.CambioDePuntaje += CambiarPuntaje;
        gameManager.CambioDeRecord += CambiarRecord;
        jugar.onClick.AddListener(() => BotonJugar());
        panelDeMuerte.SetActive(false);
    }

    public void BotonJugar()
    {
            //SceneManager.LoadScene(0);
            gameManager.contar = true;
            gameManager.conteo = 0;
            panelDeMuerte.SetActive(false);
            gameManager.meteoritosManager.Generar = true;
            gameManager.puntaje = 0;
            gameManager.jugador.moverse = true;
            gameManager.AudioNave.Play();
            gameManager.MusicaJuego.Play();
    }
        

    private void CambiarPuntaje(int valor)
    {
        puntajeInGame.text = valor.ToString();
        puntajeMuerte.text = valor.ToString();
    }

    private void CambiarRecord(int valor)
    {
        Record.text = valor.ToString();
    }

    private void CambiarPantalla()
    {
        panelDeMuerte.SetActive(true);
    }
}
