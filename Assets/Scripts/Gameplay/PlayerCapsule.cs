using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCapsule : MonoBehaviour
{
    private Rigidbody2D body;
    public Rigidbody2D Body => body;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        body.velocity = Vector2.ClampMagnitude(body.velocity, 9f);
    }
}
