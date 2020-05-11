using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRocket : Item
{
    public float Force;
    public float FuelSeconds = 0.33f;

    public Transform ForcePoint;

    private struct State
    {
        public bool ThrusterActive;
        public float RemainingFuelSeconds;
    }

    private Rigidbody2D body;
    private State state;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();

        state = new State()
        {
            RemainingFuelSeconds = FuelSeconds
        };
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

    private void Update()
    {
        if (!state.ThrusterActive)
            return;

        body.AddForceAtPosition(transform.right * Force, ForcePoint.position);
        state.RemainingFuelSeconds = Mathf.MoveTowards(state.RemainingFuelSeconds, 0.0f, Time.deltaTime);

        if (state.RemainingFuelSeconds == 0)
        {
            state.ThrusterActive = false;
        }
    }
}
