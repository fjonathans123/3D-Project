using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectivesTrigger : MonoBehaviour
{
    public objectivesManager manager;
    public Objectives obj;
    private bool isTrigger = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isTrigger && other.CompareTag("Player"))
        {
            isTrigger = true;
            manager.CompleteCurrentObjective(obj);
            gameObject.SetActive(false);
        }
    }
}
