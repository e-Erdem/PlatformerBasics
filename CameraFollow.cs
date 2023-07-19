using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private float followSpeed = 4f;
    public Transform target;
    private float yOffset = 6.5f;

    void Update()
    {
        Vector3 newPos = new Vector3(target.position.x, target.position.y+yOffset, -10f);
        transform.position = Vector3.Slerp(transform.position, newPos, followSpeed* Time.deltaTime);
    }
}
