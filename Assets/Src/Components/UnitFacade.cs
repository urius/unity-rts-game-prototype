using UnityEngine;
using Zenject;

public class UnitFacade : MonoBehaviour
{
    [Inject]
    protected UnitModel model;

    public UnitModel UnitModel => model;
}
