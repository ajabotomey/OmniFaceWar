using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Noisemaker : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private float soundRadius;

    private bool inFlight = true;

    private bool deployed = false;

    private float distance = 0;
    private Vector2 landPosition = Vector2.zero;
    private float midpoint = 0;
    private float zSpeed = 1.0f;

    public void Init(float distance, Vector2 landPosition)
    {
        this.distance = distance;
        this.landPosition = landPosition;
        midpoint = distance / 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (inFlight) {
            float step = zSpeed * Time.deltaTime;

            if (Vector2.Distance(transform.position, landPosition) > 0.2) {
                transform.position = Vector2.MoveTowards(transform.position, landPosition, step);
                UpdateSprite();
            } else {
                if (inFlight) {
                    Landed();
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy") {
            Destroy(this.gameObject);
        }
    }

    // TODO: Fix so enemies react to noisemaker
    public void Landed()
    {
        inFlight = false;
        if (source.isPlaying) {
            source.Stop();
        }
        source.Play();
        //EnemyController.instance.ReactToSound(soundRadius, transform.position);
        deployed = true;
    }

    private void UpdateSprite()
    {
        var currentDistance = Vector2.Distance(transform.position, landPosition);
        var percentage = (currentDistance / distance);

        if (percentage > 0.5) {

            zSpeed -= 0.0025f;
            transform.localScale += new Vector3(0.0025f, 0.0025f, 0);
        } else if (percentage <= midpoint) {
            zSpeed += 0.0025f;
            transform.localScale -= new Vector3(0.0025f, 0.0025f, 0);
        }
    }

    public class Factory : PlaceholderFactory<Noisemaker> { }
}
