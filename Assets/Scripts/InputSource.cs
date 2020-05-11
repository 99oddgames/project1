using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RawInput
{
    public bool SpawnItemDown;
    public bool SpawnItemUp;
    public bool SpawnItem;

    public bool PrototypeItem;

    public Vector3 ManipulatorPositionScreen;
    public Vector3 ManipulatorPositionWorld;
}

public class InputSource : MonoBehaviour
{
    public string SpawnButton = "Fire1";
    public string PrototypeButton = "Fire2";

    public RawInput ReadInput()
    {
        var result = new RawInput();
        
        if(Input.GetButton(SpawnButton))
        {
            result.SpawnItemDown = Input.GetButtonDown(SpawnButton);
            result.SpawnItem = true;
        }
        else
        {
            result.SpawnItemUp = Input.GetButtonUp(SpawnButton);
        }

        result.PrototypeItem = Input.GetButtonDown(PrototypeButton);

        result.ManipulatorPositionScreen = Input.mousePosition;
        result.ManipulatorPositionWorld = MathUtils.ScreenToWorldPosition(result.ManipulatorPositionScreen);

        return result;
    }
}
