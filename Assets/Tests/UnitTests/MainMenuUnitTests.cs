using Zenject;
using NUnit.Framework;

[TestFixture]
public class MainMenuUnitTests : ZenjectUnitTestFixture
{
    private string sceneSettingsPath = "Installers/SceneInstaller";
    private string settingsPath = "Installers/SettingsInstaller";

    [SetUp]
    public void BindInterfaces()
    {
        BaseGameInstaller.Install(Container);
        SettingsInstaller.InstallFromResource(sceneSettingsPath, Container);
        SceneInstaller.InstallFromResource(settingsPath, Container);
    }

    [Test]
    public void RunTest01()
    {
        
    }

    [TearDown]
    public void TearDown()
    {
        Container.UnbindAll();
    }
}