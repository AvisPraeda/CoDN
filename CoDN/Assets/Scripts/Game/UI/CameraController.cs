using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Clase que controla el movimiento de la cámara
public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private float panSpeed = 5f;
    [SerializeField] private float panBorder = 10f;
    [SerializeField] private float scrollSpeed = 200f;
    [SerializeField] Vector2 panLimit;
    [SerializeField] Vector2 zoomLimit;
    private Vector3 pos;

    private void Start()
    {
        camera = GetComponent<Camera>();
    }
    private void Update()
    {
        pos = transform.position;
        if(Input.mousePosition.y >= Screen.height - panBorder)
        {
            pos.y += panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.y <= panBorder)
        {
            pos.y -= panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x >= Screen.width - panBorder)
        {
            pos.x += panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x <= panBorder)
        {
            pos.x -= panSpeed * Time.deltaTime;
        }

        float zoom = camera.orthographicSize;
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        zoom -= scroll * scrollSpeed * Time.deltaTime;

        pos.x = Mathf.Clamp(pos.x, -panLimit.x / Mathf.Log10(zoom + 1), panLimit.x / Mathf.Log10(zoom + 1));
        pos.y = Mathf.Clamp(pos.y, -panLimit.y / Mathf.Log10(zoom + 1), panLimit.y / Mathf.Log10(zoom + 1));
        camera.orthographicSize = Mathf.Clamp(zoom, zoomLimit.x, zoomLimit.y);

        transform.position = pos;
    }
}
