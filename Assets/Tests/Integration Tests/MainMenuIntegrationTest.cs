using Zenject;
using System.Collections;
using UnityEngine.TestTools;

public class MainMenuIntegrationTest : ZenjectIntegrationTestFixture
{
    private string sceneSettingsPath = "Installers/SceneInstaller";
    private string settingsPath = "Installers/SettingsInstaller";

    public void CommonInstall()
    {
        PreInstall();

        //BaseGameInstaller.Install(Container);
        SettingsInstaller.InstallFromResource(sceneSettingsPath, Container);
        SceneInstaller.InstallFromResource(settingsPath, Container);

        PostInstall();
    }

    [UnityTest]
    public IEnumerator RunTest1()
    {
        CommonInstall();

        // Add test assertions for expected state
        // Using Container.Resolve or [Inject] fields
        yield break;
    }
}