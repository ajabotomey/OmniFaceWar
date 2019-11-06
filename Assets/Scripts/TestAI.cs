using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAI : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private int currentIndex;
    [SerializeField] private VoidEvent guardKilled;
    [SerializeField] private float moveSpeed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        currentIndex = Random.Range(0, waypoints.Length);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDead()) {
            guardKilled.Raise();
            Destroy(this.gameObject);
            return;
        }
            

        // If are within range of current point, select next point to move to
        if (Vector2.Distance(transform.position, waypoints[currentIndex].transform.position) <= 2.0f) {
            if (currentIndex == waypoints.Length - 1) {
                currentIndex = 0;
            } else if (currentIndex == 0) {
                currentIndex = waypoints.Length - 1;
            } else {
                currentIndex++;
            }
        }

        // Move to current point
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentIndex].transform.position, moveSpeed * Time.deltaTime);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    private bool IsDead()
    {
        return health <= 0;
    }
}
