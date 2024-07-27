using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    [Header("Animations")]
    AnimationsBase Animations;

    public enum updateType
    {
        Update,
        LateUpdate,
        FixedUpdate
    }

    public enum startType
    {
        Awake,
        Start
    }

    [Header("Update Types")]
    public startType StartType;
    public updateType UpdateType;


    private void Awake()
    {
        if (StartType == startType.Awake) Animations.OnStart();
    }

    private void Start()
    {
        if (StartType == startType.Start) Animations.OnStart();
    }

    private void Update()
    {
        if (UpdateType == updateType.Update) Animations.AnimationUpdate();
    }

    private void LateUpdate()
    {
        if (UpdateType == updateType.LateUpdate) Animations.AnimationUpdate();
    }

    private void FixedUpdate()
    {
        if (UpdateType == updateType.FixedUpdate) Animations.AnimationUpdate();
    }
}
