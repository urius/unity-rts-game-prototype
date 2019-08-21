using System;
using RSG;
using UnityEngine;

public class AwakableView : MonoBehaviour
{
    public Action OnAwake = delegate { };
    public Action OnEnabled = delegate { };
    public Action OnStart = delegate { };
    public Action OnDisabled = delegate { };

    public Promise OnStartPromise = new Promise();


    void Awake()
    {
        OnAwake();
    }

    void OnEnable()
    {
        OnEnabled();
    }
    void Start()
    {
        OnStart();
        OnStartPromise.Resolve();
    }
    void OnDisable()
    {
        OnDisabled();
    }
}
