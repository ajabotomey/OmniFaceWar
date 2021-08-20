using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TestAIScriptingEvents : MonoBehaviour
{
    [Inject] private WeaponController weaponControl;
    [Inject] private MusicPlayer musicPlayer;

    // Start is called before the first frame update
    void Start()
    {
        musicPlayer.Initialize();
        weaponControl.SelectPistol();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
