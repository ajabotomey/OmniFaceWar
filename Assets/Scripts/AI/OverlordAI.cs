using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class OverlordAI : MonoBehaviour
{
    [SerializeField] private List<AIAgent> agents = new List<AIAgent>();

    public List<AIAgent> GetAgents() { 
        return agents; 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        AIAgent agent = other.GetComponent<AIAgent>();
        if (!agents.Contains(agent)) { 
            agents.Add(agent);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        agents.Remove(other.GetComponent<AIAgent>());
    }

    public void AlertAllAgents()
    {
        foreach (AIAgent agent in agents) {
            agent.SetAgentToFullAlert();
        }
    }
}