namespace NPC
{
    namespace Ally
    {
        using UnityEngine;
        using System.Collections;
        using NPC.Enemy;
        public struct VillagerStruct // estructura del villager
        {  // Datos del villager
            public enum nombresAldeano
            {
                Ruby, Yang, Blake, Weiss, Fernando, Jaune, Pyrrha, Nora, Lie, Sun,
                Neptune, Penny, Velvet, Glynda, Qrow, Winter, Neopolitan, Salem, Raven, Ozpin
            };
            public nombresAldeano nombreAldeano;
            public int edadAldeano;
            public enum estadosAldeano { Idle, Moving, Rotating, Running};
            public estadosAldeano estadoAldeano;
            public float velocidadAldeano;
            public static explicit operator ZombieStruct(VillagerStruct md1) // funcion de que permite la combercion de  villager a zombie    
            {
                ZombieStruct despuesStruct = new ZombieStruct();
                despuesStruct.edadZombi = md1.edadAldeano;
                despuesStruct.velocidadZombi = md1.velocidadAldeano;
                despuesStruct.colorZombi = Random.Range(0, 3);
                despuesStruct.gustoZombi = (ZombieStruct.gustosZombi)Random.Range(0, 5);           
                return despuesStruct;
            }
        }
        public class MyVillager : NPCRegulator
        {
            public VillagerStruct datosAldeano; //estructura del villager            
            void Awake() // datos de villager
            {
                datosAldeano.nombreAldeano = (VillagerStruct.nombresAldeano)Random.Range(0, 20); 
                datosAldeano.edadAldeano = Random.Range(15, 101); 
                datosAldeano.velocidadAldeano = 4.0f;
                edad = datosAldeano.edadAldeano; 
                velocidad = datosAldeano.velocidadAldeano;
                gameObject.GetComponentInChildren<TextMesh>().text = "Hola soy " + gameObject.GetComponent<MyVillager>()
                    .datosAldeano.nombreAldeano.ToString() + " y tengo " + gameObject.GetComponent<MyVillager>().datosAldeano.edadAldeano.ToString() + " años";
                
            }
            void Start()
            {
                VerificarAgresor(); // objetos en la escena
                StartCoroutine(EstadosComunes()); // corrutina del npc
            }
            void OnDrawGizmos()
            {
                Gizmos.DrawLine(transform.localPosition, transform.localPosition + direction); 
            }
            void Update()
            {
                if (distanciaAZombi <= distanciaEntreObjetos) // huida
                {
                    ActualizadorDeEstadoAldeano();
                    VerificarAgresor();
                    HuirAgresor(datosAldeano);
                    mostrarMensaje();
                }
                 else 
                 {
                        ActualizadorDeEstadoAldeano();
                        ComportamientoNormal();
                        VerificarAgresor();
                        mostrarMensaje();
                 }
            }
            public void ActualizadorDeEstadoAldeano() // actuliza el estado del villager
            {
                datosAldeano.estadoAldeano = (VillagerStruct.estadosAldeano)estadoActual;
            }
            public void mostrarMensaje() 
            {
                if (distanciaAJugador <= distanciaEntreObjetos) 
                {
                    // mensage de vollager
                    gameObject.GetComponentInChildren<TextMesh>().text = "Hola soy " + gameObject.GetComponent<MyVillager>()
                        .datosAldeano.nombreAldeano.ToString() + " y tengo " + gameObject.GetComponent<MyVillager>().datosAldeano.edadAldeano.ToString() + " años";

                    gameObject.GetComponentInChildren<TextMesh>().transform.rotation = heroObject.transform.rotation; 
                }
                else
                {
                    gameObject.GetComponentInChildren<TextMesh>().text = ""; 
                }
            }
            private void OnCollisionEnter(Collision collision) // conbercion villager zombie
            {
                if (collision.transform.name == "Zombie") 
                {
                    ZombieStruct zombieStruct = gameObject.AddComponent<MyZombie>().datosZombie;
                    zombieStruct = (ZombieStruct)gameObject.GetComponent<MyVillager>().datosAldeano;

                    switch (gameObject.GetComponent<MyZombie>().datosZombie.colorZombi)
                    {
                        case 0:
                            gameObject.GetComponent<Renderer>().material.color = Color.cyan;
                            break;
                        case 1:
                            gameObject.GetComponent<Renderer>().material.color = Color.green;
                            break;
                        case 2:
                            gameObject.GetComponent<Renderer>().material.color = Color.magenta;
                            break;
                    }
                    gameObject.name = "Zombie";
                    StopAllCoroutines(); 
                    Destroy(gameObject.GetComponent<MyVillager>());
                }
            }
        }
    }
}