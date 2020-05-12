using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class Manipulator : MonoBehaviour
{
    public PlayerParams PlayerParams;

    public enum EState
    {
        Idle = 0,
        Spawning,
        Release
    }

    private struct State
    {
        public EState CurrentState;
        public Item PrototypedItemPrefab;

        public Item ItemInstance;
        public Vector3 SpawnOrigin;
    }

    private State state;
    
    private void Awake()
    {
        state = new State()
        {
            PrototypedItemPrefab = PlayerParams.DefaultItem
        };
    }

    public void DispatchedUpdate(RawInput input)
    {
        if(state.CurrentState == EState.Idle)
        {
            if (!input.SpawnItemDown)
                return;

            state.CurrentState = EState.Spawning;
            state.SpawnOrigin = input.ManipulatorPositionWorld;
            state.ItemInstance = PoolService.Spawn(state.PrototypedItemPrefab, state.SpawnOrigin, Quaternion.identity);
            state.ItemInstance.OnManipulatorBegin();
        }
        else if(state.CurrentState == EState.Spawning)
        {
            Time.timeScale = Mathf.MoveTowards(Time.timeScale, 0.01f, Time.unscaledDeltaTime * 6f);

            var desiredDirection = (state.SpawnOrigin - input.ManipulatorPositionWorld).normalized;

            var maxRotationDelta = PlayerParams.SpawnRotationSpeed * Mathf.Deg2Rad * Time.unscaledDeltaTime;
            var newDirection = Vector3.RotateTowards(state.ItemInstance.transform.right, desiredDirection, maxRotationDelta, 0f);

            state.ItemInstance.transform.right = newDirection;

            if (!input.SpawnItemUp)
                return;

            state.CurrentState = EState.Release;
        }
        else if(state.CurrentState == EState.Release)
        {
            Time.timeScale = 1f;

            state.ItemInstance.OnManipulatorRelease();
            state.ItemInstance = null;
            state.CurrentState = EState.Idle;
        }
        else
        {
            Debug.LogError($"Manipulator state {state.CurrentState} is not supported.");
        }
    }
}
