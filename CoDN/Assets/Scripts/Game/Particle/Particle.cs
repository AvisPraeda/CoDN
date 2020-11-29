using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    //Parameters
    [SerializeField] private string sound;
    [SerializeField] private int value;    
    [SerializeField] private particleType type;
    [SerializeField] private float speed;
    [SerializeField] private Vector2 position;
    [SerializeField] private Vector2 velocity;
    [SerializeField] private Vector2 target;

    public Rigidbody2D rb;
    [SerializeField] private Habitat environment;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;

    public enum particleState
    {
        Waiting,
        Contact,
        Moving,
        Processing
    }

    public enum particleType
    {
        Negative,
        Positive,
        Neutral
    }

    

    [SerializeField] private particleState state;
    public particleState State { get { return state; } }

    [SerializeField] private bool inside;
    public bool Inside { get => inside; set => inside = value; }
    public string Sound { get => sound; set => sound = value; }
    public particleType Type { get => type; set => type = value; }
    public float Speed { get => speed; set => speed = value; }
    public Vector2 Position { get => position; set => position = value; }
    public Vector2 Target { get => target; set => target = value; }
    public int Value { get => value; set => this.value = value; }

    public void SetState(particleState newState)
    {
        state = newState;
    }

    void Start()
    {
        position = transform.localPosition;
        target = (new Vector2() - position).normalized * 20;
        //target = new Vector3(position.x, -10);
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        environment = GameObject.FindWithTag("Environment").GetComponent<Habitat>();
        environment.addParticle(this);
        state = particleState.Moving;
        speed = 2f;
    }


    void Update()
    {
        UpdateSprite();
        switch (state)
        {
            case particleState.Waiting:
                velocity = new Vector2(0,0);
                break;
            case particleState.Moving:
                MoveParticle();
                break;
            case particleState.Processing:
                velocity = new Vector2(0, 0);
                break;
            case particleState.Contact:
                velocity = new Vector2(0, 0);
                break;
        }
    }

    private void FixedUpdate()
    {
        transform.Translate(velocity * Time.fixedDeltaTime);
        position = transform.localPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var obj = collision.gameObject;

        if (obj.tag == "Cell" && !inside)
        {
            transform.SetParent(collision.transform);
            state = particleState.Contact;
        }

    }

    //Establece la posición a la que se dirige la partícula
    public void SetTarget(Vector3 t)
    {
        target = t;
    }

    //Mueve la partícula hacia Target y la destruye en caso de que sobrepase los límites del nivel
    private void MoveParticle()
    {
        position = transform.localPosition;
        Vector3 dir = (target - position).normalized;
        velocity = dir * speed;
        if (Vector2.Distance(target, position) < 0.1)
        {
            state = particleState.Waiting;
        }
        else if (Mathf.Abs(position.x) > 11 || Mathf.Abs(position.y) > 11)
        {
            environment.removeParticle(this);
            Destroy(this.gameObject);
        }
    }

    //Modifica la partícula al ser procesada
    public void Process()
    {
        if (value == 0)
        {
            value = 3;
        }
        else if (value > 0)
        {
            value = 0;
        }

        state = particleState.Waiting;
    }

    //Devuelve un string con el tipo de la partícula
    public string TypeToString()
    {
        string s = "";
        switch (type)
        {
            case particleType.Positive:
                s = "Positive";               
                break;
            case particleType.Neutral:
                s = "Neutral";
                break;
            case particleType.Negative:
                s = "Negative";
                break;
        }
        return s;
    }

    //Actualiza el sprite de la partícula dependiendo de su energía
    public void UpdateSprite()
    {
        if(value == 0)
        {
            type = particleType.Neutral;
        } else if (value > 0)
        {
            type = particleType.Positive;
        }
        else
        {
            type = particleType.Negative;
        }
        animator.SetInteger("Type", value);
        animator.SetBool("Inside",inside);
    }
    
    //Actualiza el color de la vesícula que la rodea
    public void UpdateColor(Color color)
    {
        spriteRenderer.color = color;
    }
}
