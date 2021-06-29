using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public float speed = 1f;

    void FixedUpdate()
    {
        transform.position -= Vector3.right * Time.fixedDeltaTime * speed;
    }
}
