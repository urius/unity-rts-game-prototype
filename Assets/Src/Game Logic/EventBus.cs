using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventBus : MonoBehaviour
{
    public static Action<UnitAvatar, UnitAvatar> UnitDestroyedBy = delegate { };
}
