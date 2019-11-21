using System.IO;
using log4net.Config;
using UnityEngine;

public static class LoggingConfiguration
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Configure()
    {
        Debug.Log($"{Application.dataPath}/log4net.xml");
        XmlConfigurator.Configure(new FileInfo($"{Application.dataPath}/log4net.xml"));
    }
}
