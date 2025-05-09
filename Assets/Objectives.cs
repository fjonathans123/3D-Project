using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewObjective", menuName = "Objective system/Objective")]
public class Objectives : ScriptableObject
{
    public string objectiveName;
    public bool isComplete = false;
    [TextArea] public string description;
    public List<Objectives> subobjectives = new List<Objectives>();

    public int requireCount;
    public int currentCount = 0;

    public int Reward;

    public ObjectiveType objType;
    public enum ObjectiveType
    {
        ItemCollection,
        EnemyDefeat,
        ObjectTrigger
    }
}
