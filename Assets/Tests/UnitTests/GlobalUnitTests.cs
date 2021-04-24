using Zenject;
using NUnit.Framework;
using Rewired;
using UnityEngine;

[TestFixture]
public class GlobalUnitTests : ZenjectUnitTestFixture
{
    private string sceneSettingsPath = "Installers/SceneInstaller";
    private string settingsPath = "Installers/SettingsInstaller";

    [SetUp]
    public void BindInterfaces()
    {
        BaseGameInstaller.Install(Container);
        SettingsInstaller.InstallFromResource(settingsPath, Container);
        SceneInstaller.InstallFromResource(sceneSettingsPath, Container);
    }

    [TearDown]
    public void TearDown()
    {
        Container.UnbindAll();
    }
}