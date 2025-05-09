using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Itemcollectiontrigger : MonoBehaviour
{
    public objectivesManager objectiveManager;
    public Objectives obj;


        public void CollectItem()
    {
        obj.currentCount++;
        objectiveManager.UpdateQuestCount();

        if(obj.objType == Objectives.ObjectiveType.ItemCollection && obj.currentCount >= obj.requireCount)
        {
            objectiveManager.CompleteCurrentObjective(obj);
        }
        Destroy(gameObject);
    }
}
