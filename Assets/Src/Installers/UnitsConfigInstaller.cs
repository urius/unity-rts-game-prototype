using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "UnitsConfigInstaller", menuName = "Installers/UnitsConfigInstaller")]
public class UnitsConfigInstaller : ScriptableObjectInstaller<UnitsConfigInstaller>
{
    [SerializeField]
    private UnitsConfig _unitsConfig;
    public override void InstallBindings()
    {
        Container.BindInstance(_unitsConfig).AsSingle();
    }
}