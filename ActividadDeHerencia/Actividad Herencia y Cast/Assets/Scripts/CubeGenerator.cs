using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPC.Enemy;
using NPC.Ally;
using TMPro;

public class CubeGenerator : MonoBehaviour
{
    static System.Random r = new System.Random();// variable auxiliar para declarar e inicializar el readonly
    public readonly int limiteMinimo = r.Next(5,15); // linea nativa para asignar un limite aleatorio al readonly
    const int limiteMaximo = 25; // constante para la generacion maxima de cubos
    int nAlly = 0, nEnemy = 0, limiteGenerado,generadorRandom; // variables para la generacion de cubos
    // heroe variables y funcion generadora
    public GameObject cuboHeroe;
    public GameObject heroe;
    public GameObject camaraHeroe;
    GameObject camara;
    Vector3 posHero;
    Vector3 camPos;
    GameObject enemys;
    GameObject allys;
    public GameObject heroObject;
    // variables del texto del canvas
    public TextMeshProUGUI nEnemigos; 
    public TextMeshProUGUI nAliados;    
    public void CreacionHeroe()// funcion generadora del heroe
    {   // creacion del heroe
        posHero = new Vector3(Random.Range(-40.0f, -34.0f), 0.0f, Random.Range(-40.0f, -34.0f)); // calcula una posicion
        heroe = GameObject.Instantiate(cuboHeroe, posHero, Quaternion.identity); // instancia al heroe en escena
        heroe.name = "Heroe"; // lo nombra en la jerarquia de unity
        heroe.AddComponent<MyHero>();
        heroe.AddComponent<HeroMove>();
        // creacion de la camara que sigue al heroe
        camPos = new Vector3(heroe.transform.position.x, heroe.transform.position.y + 0.8f, heroe.transform.position.z); 
        camara.AddComponent<HeroCam>(); 
        camara.name = "Camara Heroe"; 
        camara.transform.SetParent(heroe.transform);
    }  // zombie variables y funcion generadora
    int colorZombie;
    public GameObject zombie;
    public GameObject mensaje;
    public GameObject mensajeZombi;
    public void CreacionZombie(GameObject enemigos) // funcion generadora de los zombies
    {
        zombie = GameObject.CreatePrimitive(PrimitiveType.Cube); // instancia un cubo como zombie en la escena
        zombie.name = "Zombie";
        zombie.transform.SetParent(enemigos.transform);
        Vector3 posZombi = new Vector3(Random.Range(-14.0f, 14.0f), 0.0f, Random.Range(-14.0f, 14.0f)); 
        zombie.AddComponent<Rigidbody>();
        zombie.GetComponent<Rigidbody>().freezeRotation = true;
        mensajeZombi = Instantiate(mensaje);
        mensajeZombi.name = "Mensaje"; 
        mensajeZombi.transform.SetParent(zombie.transform); 
        mensajeZombi.transform.localPosition = Vector3.zero; 
        mensajeZombi.transform.localPosition = Vector3.up; 
        zombie.AddComponent<MyZombie>();         
        switch (zombie.GetComponent<MyZombie>().datosZombie.colorZombi)
        {
            case 0:
                zombie.GetComponent<Renderer>().material.color = Color.cyan; 
                break;
            case 1:
                zombie.GetComponent<Renderer>().material.color = Color.green; 
                break;
            case 2:
                zombie.GetComponent<Renderer>().material.color = Color.magenta; 
                break;
        }
    } // aldeano variables y funcion generadora
    public GameObject aldeano;
    public GameObject mensajeAldeano;
    public void CreacionAldeano(GameObject aliados)
    {
        aldeano = GameObject.CreatePrimitive(PrimitiveType.Cube); // crea la figura solicitada
        Vector3 posAldeano = new Vector3(Random.Range(-10.0f, 10.0f), 0.0f, Random.Range(-10.0f, 10.0f)); 
        aldeano.transform.position = posAldeano; 
        aldeano.GetComponent<Renderer>().material.color = Color.black; 
        aldeano.GetComponent<Transform>().localScale = new Vector3(1.0f, 2.0f, 1.0f); 
        aldeano.AddComponent<Rigidbody>().freezeRotation = true; 
        aldeano.name = "Aldeano"; 
        aldeano.transform.SetParent(aliados.transform); 
        mensajeAldeano = Instantiate(mensaje); 
        mensajeAldeano.name = "Mensaje"; 
        mensajeAldeano.transform.SetParent(aldeano.transform); 
        mensajeAldeano.transform.localPosition = Vector3.zero; 
        mensajeAldeano.transform.localPosition = Vector3.up; 
        aldeano.AddComponent<MyVillager>(); 
    }
    void Start()
    {
        limiteGenerado = Random.Range(limiteMinimo, limiteMaximo); // genera aleatoriamente el limite de objetos a crear
        for (int i = 0; i < limiteGenerado; i++) 
        {
            generadorRandom = Random.Range(0, 2);
            if (generadorRandom == 0)
                nEnemy++;
            if (generadorRandom == 1)
                nAlly++;
        } // creacion del heroe
        CreacionHeroe(); // creacion de los zombis
        enemys = new GameObject();
        enemys.name = "Enemys";
        for (int i = 0; i < nEnemy; i++) 
        {
            CreacionZombie(enemys);
        }
        // creacion de los aldeanos
        allys = new GameObject();
        allys.name = "Allys";
        for (int i = 0; i < nAlly; i++) 
        {
            CreacionAldeano(allys);
        }
    }
    void Update() // actualiza el conteo de aliados y enemigos en la escena
    {
        var zombieList = FindObjectsOfType<MyZombie>();
        foreach (var item in zombieList)
        {
            nEnemigos.text = zombieList.Length.ToString();
        }
        var villagerList = FindObjectsOfType<MyVillager>();
        foreach (var item in villagerList)
        {
            nAliados.text = villagerList.Length.ToString();
        }
    }
}