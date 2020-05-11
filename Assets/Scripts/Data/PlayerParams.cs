using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerParams", menuName = "Custom/Player Params")]
public class PlayerParams : ScriptableObject
{
    [Header("Manipulator")]
    public float SpawnRotationSpeed = 18.0f;

    [Space]

    public Item DefaultItem;
}
