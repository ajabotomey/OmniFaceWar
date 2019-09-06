using Zenject;
using System.Collections;
using UnityEngine.TestTools;

public class WeaponGunIntegrationTest : ZenjectIntegrationTestFixture
{
    private string weaponInstallerPath = "Installers/WeaponInstaller";
    private string playerPrefab = "Prefabs/Player";

    public void CommonInstall()
    {
        // Setup initial state by creating game objects from scratch, loading prefabs/scenes, etc

        PreInstall();

        // Call Container.Bind methods
        WeaponInstaller.InstallFromResource(weaponInstallerPath, Container);
        Container.Bind<PlayerControl>().FromResource(playerPrefab);

        PostInstall();
    }

    [UnityTest]
    public IEnumerator RunTest1()
    {
        CommonInstall();

        var player = Container.Resolve<PlayerControl>();



        // Add test assertions for expected state
        // Using Container.Resolve or [Inject] fields
        yield break;
    }
}