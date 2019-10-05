using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPC.Enemy;
using NPC.Ally;
public class NPCRegulator : MonoBehaviour
{   // variables reguladoras
    public float distanciaEntreObjetos = 5.0f;
    public int seMueve, selectorDireccional, edad, estadoActual;
    public float velocidad;
    public GameObject heroObject;
    public GameObject villagerObject;
    public GameObject zombiObject;
    public Vector3 direction;
    Vector3 dPlayer;
    Vector3 dZombi;
    Vector3 dAldeano;
    public float distanciaAJugador;
    public float distanciaAZombi;
    public float distanciaAldeano;
    public void ComportamientoNormal() //funcion que ejecutan ambos npc
    {
        if (estadoActual == 0) { } //idle
        if (estadoActual == 1) //moving
        {
            transform.position += transform.forward * velocidad * (15 / (float)edad) * Time.deltaTime;
        }
        if (estadoActual == 2) //rotating
        {
            if (selectorDireccional == 0) // Rotacion Positiva
            {
                transform.eulerAngles += new Vector3(0, Random.Range(10f, 150f) * Time.deltaTime, 0);
            }
            if (selectorDireccional == 1) // Rotacion Negativa
            {
                transform.eulerAngles += new Vector3(0, Random.Range(-10f, -150f) * Time.deltaTime, 0);
            }
        }
    }
    public IEnumerator EstadosComunes()// corrutina que ejecutan ambos npc
    {
        while (true)
        {
            estadoActual = Random.Range(0, 3);
            if (estadoActual == 2) // Rotating
            {
                selectorDireccional = Random.Range(0, 2);
            }
            yield return new WaitForSeconds(3.0f); // Espera 3 segundos y cambia de comportamiento
        }
    }
    public void VerificarVictima() // verifica la distancia de los objetos en la escena
    {
        if (heroObject == null)  // detecta por primera vez al heroe
            heroObject = GameObject.Find("Heroe");
        dPlayer = heroObject.transform.position - transform.position; 
        distanciaAJugador = dPlayer.magnitude; 
        GameObject[] AllGameObjects = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        foreach (GameObject aGameObject in AllGameObjects)
        {
            Component bComponent = aGameObject.GetComponent<MyVillager>(); 
            if (bComponent != null)
            {
                villagerObject = aGameObject;
                dAldeano = villagerObject.transform.position - transform.position; 
                distanciaAldeano = dAldeano.magnitude; 
                if (distanciaAldeano <= distanciaEntreObjetos) 
                    break;
            }
        }
    }
    public void PerseguirVictima(ZombieStruct zs) // inicia la persecusion si encuentra una victima cercana
    {
        estadoActual = 3;
        if (distanciaAldeano <= distanciaEntreObjetos) 
        {
            direction = Vector3.Normalize(villagerObject.transform.position - transform.position); 
            transform.position += direction * zs.velocidadZombi * (15 / (float)zs.edadZombi) * Time.deltaTime; 
        }
        else if (distanciaAJugador <= distanciaEntreObjetos) 
        {
            direction = Vector3.Normalize(heroObject.transform.position - transform.position); 
            transform.position += direction * zs.velocidadZombi * (15 / (float)zs.edadZombi) * Time.deltaTime;
        }
    }
    public void VerificarAgresor() 
    {
        if(heroObject == null) 
            heroObject = GameObject.Find("Heroe");

        dPlayer = heroObject.transform.position - transform.position;
        distanciaAJugador = dPlayer.magnitude;
        GameObject[] AllGameObjects = FindObjectsOfType(typeof(GameObject)) as GameObject[]; 
        foreach (GameObject aGameObject in AllGameObjects)
        {
            Component bComponent = aGameObject.GetComponent<MyZombie>();
            if (bComponent != null)
            {
                zombiObject = aGameObject;
                dZombi = zombiObject.transform.position - transform.position; 
                distanciaAZombi = dZombi.magnitude;
                if (distanciaAZombi <= distanciaEntreObjetos) 
                    break;
            }                
        }
    }
    public void HuirAgresor(VillagerStruct als) // funcion escapar del zombie
    {
        estadoActual = 3;
        direction = Vector3.Normalize(zombiObject.transform.position - transform.position); // buscador direccion que apunte al objeto para llegar
        transform.position += -1 * direction * als.velocidadAldeano * (15 / (float)als.edadAldeano) * Time.deltaTime; // transforma la posicion para alejarse de objeto
    }
}
