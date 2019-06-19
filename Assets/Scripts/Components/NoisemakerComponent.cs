using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class NoisemakerComponent : MonoBehaviour
{
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float fireRate = 10.0f;
    [SerializeField] private GameObject noisemaker;
    [SerializeField] private float zSpeed = 0;
    [SerializeField] private LayerMask obstacleMask;

    private float elapsedTime;
    private float maxDistance = 5000;

    private IInputController _inputController;
    private Noisemaker.Factory _noisemakerFactory;

    [Inject]
    public void Construct(IInputController inputController, Noisemaker.Factory noisemakerFactory)
    {
        _inputController = inputController;
        _noisemakerFactory = noisemakerFactory;
    }

    // Start is called before the first frame update
    void Start()
    {
        elapsedTime = fireRate;
    }


    // Update is called once per frame
    void Update()
    {
        // TODO: Fix once Weapon Control is available
        //bool launchNoise = _inputController.FireWeapon();
        bool launchNoise = Input.GetKeyDown(KeyCode.E);

        // Check that we aren't throwing it onto an object. Also make sure we aren't throwing over obstacles as well
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var distance = Vector2.Distance(transform.position, mousePos);
        RaycastHit2D initialHit = Physics2D.Raycast(mousePos, Vector2.zero, maxDistance, obstacleMask);
        RaycastHit2D obstructionHit = Physics2D.Raycast(transform.position, transform.up, distance, obstacleMask);

        Debug.Log(distance);

        if (!initialHit && !obstructionHit) {
            if (launchNoise) {
                Fire(distance, mousePos);
            }
        } else {
            // Change the cursor
        }
    }

    void Fire(float distance, Vector3 mousePos)
    {
        // If it is a small distance, we might as well drop it at the position as opposed to throwing it
        //if (distance <= 3.0f) {
        //    Noisemaker noise = Instantiate(noisemaker, mousePos, Quaternion.identity).GetComponent<Noisemaker>();
        //    noise.Landed();
        //} else {
        //Noisemaker noise = Instantiate(noisemaker, bulletSpawnPoint.position, Quaternion.identity).GetComponent<Noisemaker>();
        Noisemaker noise = _noisemakerFactory.Create();
        noise.transform.position = bulletSpawnPoint.position;
        noise.transform.rotation = Quaternion.identity;
        noise.Init(distance, mousePos);
        //}
    }
}
