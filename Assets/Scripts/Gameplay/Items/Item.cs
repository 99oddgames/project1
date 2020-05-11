using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoPoolable
{
    public virtual void OnManipulatorBegin() { }
    public virtual void OnManipulatorRelease() { }
}
