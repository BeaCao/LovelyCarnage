using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] prefabs; // Lista de obstáculos
    // public Vector3 spawnPos; <--- YA NO USAMOS ESTA VARIABLE FIJA PARA LA POSICIÓN
    public float startDelay = 2f;
    public float repeatRate = 2f;

    void Start()
    {
        InvokeRepeating("SpawnObject", startDelay, repeatRate);
    }

    void SpawnObject()
    {
        // 1. Elegimos un número aleatorio
        int index = Random.Range(0, prefabs.Length);

        // 2. CALCULAMOS LA POSICIÓN DINÁMICA
        // X = 30 (Lejos a la derecha)
        // Y = La altura (Y) que tenga el prefab original. 
        //     (Si es el volador usará su altura, si es el normal usará la suya).
        Vector3 spawnLocation = new Vector3(30, prefabs[index].transform.position.y, 0);

        // 3. Creamos el objeto en esa posición calculada
        Instantiate(prefabs[index], spawnLocation, prefabs[index].transform.rotation);
    }
}