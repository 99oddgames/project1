using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemRocket : Item
{
    public float Force;
    public float FuelSeconds = 0.33f;

    public Transform ForcePoint;

    private Joint2D joint;

    public Rigidbody2D Body => body;


    private struct State
    {
        public bool ThrusterActive;
        public float RemainingFuelSeconds;

        public Rigidbody2D ConnectedBody;
    }

    private Rigidbody2D body;
    private State state;

    public override void Reinitialize()
    {
        if(!body)
        {
            body = GetComponent<Rigidbody2D>();
        }

        if(!joint)
        {
            joint = GetComponent<Joint2D>();
        }

        body.velocity = Vector3.zero;
        body.angularVelocity = 0f;

        state = new State()
        {
            RemainingFuelSeconds = FuelSeconds
        };
    }

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    public override void OnManipulatorBegin()
    {
        body.simulated = false;
    }

    public override void OnManipulatorRelease()
    {
        body.simulated = true;
        state.ThrusterActive = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var capsule = collision.gameObject.GetComponent<PlayerCapsule>();

        Rigidbody2D bodyToConnect;

        if (!capsule)
        {
            var rocket = collision.gameObject.GetComponent<ItemRocket>();

            if(!rocket)
                return;

            bodyToConnect = rocket.body;
        }
        else
        {
            bodyToConnect = capsule.Body;
        }

        transform.SetParent(collision.transform);
        state.ConnectedBody = bodyToConnect;

        body.simulated = false;
    }

    private void Update()
    {
        if (!state.ThrusterActive)
            return;

        if(state.ConnectedBody)
        {
            state.ConnectedBody.AddForceAtPosition(transform.right * Force, ForcePoint.position);
        }
        else
        {
            body.AddForceAtPosition(transform.right * Force, ForcePoint.position);
        }

        state.RemainingFuelSeconds = Mathf.MoveTowards(state.RemainingFuelSeconds, 0.0f, Time.deltaTime);

        if (state.RemainingFuelSeconds == 0)
        {
            state.ThrusterActive = false;
        }
    }
}
