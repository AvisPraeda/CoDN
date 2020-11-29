using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawner : MonoBehaviour
{
    [SerializeField] private bool isPaused = true;

    [SerializeField] private List<GameObject> particles;
    
    [SerializeField] private List<int> choiceWeights;

    [Header("Spawner Atributtes")]
    
    [SerializeField] private float spawnDistance = 10f;

    [SerializeField] private float spawnRate = 2f;

    [SerializeField] private float timeNextParticle = 0f;
    
    public bool IsPaused { get => isPaused; set => isPaused = value; }
    public List<GameObject> Particles { get => particles; set => particles = value; }
    public List<int> ChoiceWeights { get => choiceWeights; set => choiceWeights = value; }

    // Update is called once per frame
    void Update()
    {
        if (!IsPaused)
        {
            timeNextParticle -= Time.deltaTime;
        }

        if (timeNextParticle <= 0)
        {
            timeNextParticle = spawnRate;
            SpawnParticle();
        }       
    }

    //Genera una partícula entre la lista de partículas disponibles en una posición aleatoria de una circunferencia.
    private void SpawnParticle()
    {
        //Se calcula la posición en la que aparece la partícula
        Vector3 offset = Random.onUnitSphere;
        offset.z = 0;
        offset = offset.normalized * spawnDistance;
        Vector3 spawnPosition = transform.position + offset;
        //Se obtiene el tipo de partícula que va a instanciarse
        int type = randomWeights(choiceWeights);
        //Se instancia la partícula
        Instantiate(particles[type], spawnPosition, Quaternion.identity, transform);
    }

    //Devuelve un valor aleatorio dentro de una lista de probabilidades por peso.
    public int randomWeights(List<int> choices)
    {
        int sumWeight = 0;
        //Se calcula la suma de los pesos de cada opción
        for (int i = 0; i < choices.Count; i++)
        {
            sumWeight += choices[i];
        }
        //Se escoge un valor aleatorio entre 0 y el valor de la suma anterior
        int rnd = Random.Range(0, sumWeight);
        //Se recorre la lista de pesos hasta encontrar un peso inferior al valor aleatorio.
        //En caso de que el valor aleatorio sea superior, se le resta el valor del peso.
        for (int i = 0; i < choices.Count; i++)
        {           
            if (rnd < choices[i]) return i;
            rnd -= choices[i];
        }
        return choices.Count - 1;
    }
}
