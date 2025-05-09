using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubObjectiveTrigger : MonoBehaviour
{
    public objectivesManager manager;
    public Objectives subObjective;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            manager.completeSubObjective(subObjective);
        }
    }
}
