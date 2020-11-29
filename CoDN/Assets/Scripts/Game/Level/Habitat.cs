using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Clase que almacena la lista de partículas en el mundo, antes de entrar en contacto con las células
public class Habitat : MonoBehaviour
{
    [SerializeField] private int size;
    public int Size{get { return size; }}
    [SerializeField] private List<Particle> particles = null;
    public List<Particle> Particles { get { return particles; } }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addParticle(Particle particle)
    {
        particles.Add(particle);
        size++;
    }

    public void removeParticle(Particle particle)
    {
        particles.Remove(particle);
        size--;
    }

    public void RemoveAll()
    {
        for (int i = 0; i < particles.Count; i++)
        {
            if(particles[i] != null)
            {
                Destroy(particles[i].gameObject);
            }
        }
        particles.Clear();
        size = 0;
    }
}
