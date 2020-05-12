using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    public Transform Target;
    public float Speed;

    private void LateUpdate()
    {
        //mock
        transform.position = Vector3.Lerp(transform.position, Target.position, Time.deltaTime * 18f);
    }
}
