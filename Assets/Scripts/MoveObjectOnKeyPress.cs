using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveObjectOnKeyPress : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    Transform target;

    public float moveDelta = 0.001f;
    public float rotateDelta = 0.1f;

    public KeyCode key = KeyCode.Q;

    private void Update()
    {
        if (Input.GetKey(key))
        {
            target = endPoint;
        }
        else
        {
            target = startPoint;
        }


        if (target)
        {
            Vector3 newPos = Vector3.MoveTowards(transform.position, target.position, moveDelta);
            transform.position = newPos;

            Vector3 newEuler = Vector3.MoveTowards(transform.eulerAngles, target.eulerAngles, rotateDelta);
            transform.eulerAngles = newEuler;
        }
    }

}
