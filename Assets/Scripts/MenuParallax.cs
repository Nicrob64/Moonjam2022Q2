using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuParallax : MonoBehaviour
{

    Vector2 startLoc;
    public float maxMovement = 50.0f;
    public float speed = 2.0f;

    void Start()
    {
        startLoc = transform.position;
    }


    void Update()
    {
        Vector2 pz = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        float x = Mathf.Lerp(transform.position.x, startLoc.x + (1.0f-pz.x) * maxMovement, speed * Time.deltaTime);
        float y = Mathf.Lerp(transform.position.y, startLoc.y + (1.0f-pz.y) * maxMovement, speed * Time.deltaTime);

        transform.position = new Vector3(x, y, 0);
    }
}
