using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "WeaponInstaller", menuName = "Installers/WeaponInstaller")]
public class WeaponInstaller : ScriptableObjectInstaller<WeaponInstaller>
{
    [SerializeField] private WeaponController weaponController;
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<WeaponController>().FromInstance(weaponController).AsSingle();
        weaponController.Initialize();
    }
}