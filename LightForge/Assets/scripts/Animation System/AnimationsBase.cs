using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsBase : MonoBehaviour
{
    public virtual void OnStart() { }

    public virtual void OnStartInternal() { }

    public virtual void AnimationUpdate() { }

    public virtual void AnimationUpdateInternal () { }
}
