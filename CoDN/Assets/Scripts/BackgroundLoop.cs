using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundLoop : MonoBehaviour
{
    [Range(0, 10)]
    [SerializeField] private float speed;
    [SerializeField] private Vector2 position;
    [SerializeField] private Vector2 startPosition;
    [SerializeField] private Vector2 endPosition;

    

    void Start()
    {
        startPosition = transform.position;
        position = transform.position;
        if (endPosition == null)
        {
            endPosition = new Vector2(0, 0);
        }
    }

    void FixedUpdate()
    {
        position = transform.position;
        float distance = Vector2.Distance(endPosition, position);
        if (distance <= 0.1)
        {
            position = startPosition;
        } else
        {
            Vector2 dir = (endPosition - position).normalized;
            position = position + dir * speed * Time.deltaTime;
        }
        transform.position = position;
    }
}
