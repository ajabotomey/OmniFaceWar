using Zenject;
using NUnit.Framework;

[TestFixture]
public class SettingsManagerUnitTests : ZenjectUnitTestFixture
{
    private string settingsPath = "Installers/SettingsInstaller";

    [SetUp]
    public void BindInterfaces()
    {
        BaseGameInstaller.Install(Container);
        SettingsInstaller.InstallFromResource(settingsPath, Container);
    }

    [Test]
    public void ResolveSettingsManagerTest()
    {
        var settingsManager = Container.Resolve<SettingsManager>();
        Assert.NotNull(settingsManager);
    }

    [Test]
    public void DyslexicTextTest()
    {
        var settingsManager = Container.Resolve<SettingsManager>();
        var dyslexicEnabled = settingsManager.IsDyslexicTextEnabled();

        if (settingsManager != null)
            settingsManager.DyslexicToggle();

        Assert.AreNotEqual(settingsManager.IsDyslexicTextEnabled(), dyslexicEnabled);
    }

    [TearDown]
    public void TearDown()
    {
        Container.UnbindAll();
    }
}