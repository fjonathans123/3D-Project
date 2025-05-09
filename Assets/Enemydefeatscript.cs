using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemydefeatscript : MonoBehaviour
{
    public objectivesManager manager;
    public Objectives obj;
    
    public void EnemyDefeated()
    {
        obj.currentCount++;
        manager.UpdateQuestCount();
        if (obj.objType == Objectives.ObjectiveType.EnemyDefeat && obj.currentCount >= obj.requireCount)
        {
            obj.isComplete = true;
            manager.CompleteCurrentObjective(obj);
        }
    }
}
