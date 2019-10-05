using UnityEngine;
using System;
using NPC.Enemy;
using NPC.Ally;
using TMPro;
public class MyHero : MonoBehaviour
{
    static System.Random r = new System.Random(); // variable auxiliar para asignacion nativa del readonly
    public readonly float velHeroe = (float)r.NextDouble()*8.0f + 1.5f; // velocidad aleatoria del heroe
    VillagerStruct datosAldeano;
    ZombieStruct datosZombie;
    bool contactoZombi;
    bool contactoAldeano;
    public GameObject mensajito;    
    private void Start()
    {
        var mensajitos = FindObjectsOfType<GameObject>();// lista para detectar el GAME OVER
        foreach (var item in mensajitos)
        {
            if (item.name == "Mensaje Final")
            {
                mensajito = item; // asigna el texto en el canvas con el GAME OVER
                mensajito.SetActive(false); // desactiva el texto camvas del GAME OVER
            }
        }
    }
    void Update() // condiciones para mensajes por contacto
    {
        if (contactoAldeano)
        {
            Debug.Log(MensajeAldeano(datosAldeano));

            contactoAldeano = false;
        }
        if (contactoZombi)
        {
            Debug.Log(MensajeZombi(datosZombie));
            contactoZombi = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.name == "Aldeano")
        {
            contactoAldeano = true;
            datosAldeano = collision.gameObject.GetComponent<MyVillager>().datosAldeano;
        }
        if (collision.transform.name == "Zombie")
        {
            contactoZombi = true;
            datosZombie = collision.gameObject.GetComponent<MyZombie>().datosZombie; // Esto va en el colision de cada zombie o aldeano
            Debug.Log("Game Over");
            mensajito.SetActive(true);// aqui saca el game over cuando lo tocan
            Time.timeScale = 0; // timescale detener el juego cuando un zombie toque al heroe
        }
    }    // funciones devuelven el mensaje por contacto del heroe
    public string MensajeZombi(ZombieStruct datosZombie)
    {
        string mensajeZombi = "Waaaarrrr quiero comer " + datosZombie.gustoZombi;
        return mensajeZombi;
    }
    public string MensajeAldeano(VillagerStruct datosAldeano)
    {
        string mensajeAldeano = "Hola soy " + datosAldeano.nombreAldeano + " y tengo " + datosAldeano.edadAldeano + " años";
        return mensajeAldeano;
    }
}