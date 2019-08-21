using System;
using UnityEngine;

public class AwakableView : MonoBehaviour
{
    public Action OnAwake = delegate { };
    public Action OnEnabled = delegate { };
    public Action OnStart = delegate { };
    public Action OnDisabled = delegate { };

    
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
    }
    void OnDisable()
    {
        OnDisabled();
    }
}
