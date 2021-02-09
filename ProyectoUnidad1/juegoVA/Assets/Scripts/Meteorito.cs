using System.Collections;
using UnityEngine;

public class Meteorito : MonoBehaviour
{
    [SerializeField]

    Sprite[] sprites = new Sprite[0]; //Este es el arreglo de sprites de meteoritos

    SpriteRenderer render;


    [SerializeField]
    float duracion = 10f;

    public float Speed { get; set; }

    public Vector3 Target { get; set; } //Target es hacia donde van a ir los meteoritos

    private void Start()
    {
        
        if (Target == null) //Esto es para evitar errores, si target es nulo, hago que el target sea la misma posición 
            Target = transform.position;

        render = GetComponent<SpriteRenderer>();

        if (sprites.Length > 0)//Si el arreglo de sprites es mayor a cero, escojo uno al azar
            render.sprite = sprites[Random.Range(0, sprites.Length)];
        StartCoroutine(Destruir()); //Inicia la coroutina para destruirlo en x tiempo
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, Target.y, transform.position.z), Speed * Time.deltaTime);
    }

    IEnumerator Destruir()
    {
        yield return new WaitForSeconds(duracion);
        Destroy(gameObject);
    }
}
