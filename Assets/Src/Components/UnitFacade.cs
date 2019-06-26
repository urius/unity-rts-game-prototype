using UnityEngine;
using Zenject;

public class UnitFacade : MonoBehaviour
{
    [Inject]
    private UnitModel _model;

    public UnitModel UnitModel => _model;
}
