using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    private InputSource inputSource;
    private Manipulator manipulator;

    private void Awake()
    {
        inputSource = gameObject.AddComponent<InputSource>();
        manipulator = FindObjectOfType<Manipulator>();
    }

    private void Update()
    {
        var input = inputSource.ReadInput();
        manipulator.DispatchedUpdate(input);
    }
}
