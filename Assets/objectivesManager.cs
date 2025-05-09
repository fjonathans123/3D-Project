using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class objectivesManager : MonoBehaviour
{
    public TMP_Text ObjectiveText;
    public List<Objectives> objectives = new List<Objectives>();

    private int currentObjectiveIndex = 0;
    private List<Objectives> activeObj = new List<Objectives>();
    private List<Objectives> completedSubObj = new List<Objectives>();
    private List<Objectives> completedObj = new List<Objectives>();


    // Start is called before the first frame update
    void Start()
    {
        if (objectives.Count > 0)
        {
            activeObj = new List<Objectives>(objectives);
            DisplayObjective();
        }
    }

    void DisplayObjective()
    {
        ClearObjectiveText();
        string objectiveDisplayTxt = "Objectives:\n";

        objectiveDisplayTxt += DisplayObjectType(Objectives.ObjectiveType.ItemCollection, "Item Collection");
        objectiveDisplayTxt += DisplayObjectType(Objectives.ObjectiveType.EnemyDefeat, "Enemy Defeat");
        objectiveDisplayTxt += DisplayObjectType(Objectives.ObjectiveType.ObjectTrigger, "Triggered Event");


        ObjectiveText.text = objectiveDisplayTxt;
    }

    string DisplayObjectType(Objectives.ObjectiveType type, string objectiveTitle)
    {
        string objectivesoftypetext = objectiveTitle + ":\n";
        bool hasObjective = false;

        foreach (Objectives obj in activeObj)
        {

            if (obj.objType == type)
            {
                hasObjective = true;
                string status = completedObj.Contains(obj) ? "[Completed]" : "[Incomplete]";
                objectivesoftypetext += "- " + obj.objectiveName + " " + status + "\n";

                if (type == Objectives.ObjectiveType.ItemCollection || type == Objectives.ObjectiveType.EnemyDefeat)
                {
                    objectivesoftypetext += "   Progress: " + obj.currentCount + "/" + obj.requireCount + "\n";
                }
            }
        }


        if (!hasObjective)
        {
            objectivesoftypetext = string.Empty;
        }
        return objectivesoftypetext;
    }

    void ClearObjectiveText()
    {
        ObjectiveText.text = string.Empty;
    }

    public void CompleteCurrentObjective(Objectives mainObj)
    {
        completedObj.Add(mainObj);
        DisplayObjective();
        StartCoroutine(RemoveCompletedObj(mainObj, 3f));
    }

    public void CompleteCurrentObjective()
    {
        if (currentObjectiveIndex < objectives.Count)
        {
            Objectives currentObjective = objectives[currentObjectiveIndex];
            //currentObjective.CompleteObjective();

            if (currentObjective.subobjectives.Count > 0)
            {
                DisplaySubObjectives(currentObjective);
            }
            else
            {
                currentObjectiveIndex++;
                if (currentObjectiveIndex < objectives.Count)
                {
                    DisplayObjective();
                }
                else
                {
                    ObjectiveText.text = "All Objectives Completed!";
                }
            }
        }
    }

    string DisplaySubObjectives(Objectives currObj)
    {
        string subObjText = "\nSub-Objectives\n";
        foreach (Objectives subObj in currObj.subobjectives)
        {
            string status = completedSubObj.Contains(subObj) ? "[Completed]" : "[Incomplete]";
            subObjText += "- " + subObj.objectiveName + " " + status + "\n";
        }
        return subObjText;
    }

    public void completeSubObjective(Objectives subObj)
    {
        completedObj.Add(subObj);
        DisplayObjective();
        StartCoroutine(RemoveCompletedSubObj(subObj, 3f));
    }

    IEnumerator RemoveCompletedObj(Objectives mainObj, float delay)
    {
        yield return new WaitForSeconds(delay);
        RemoveMainObj(mainObj);
    }

    IEnumerator RemoveCompletedSubObj(Objectives mainObj, float delay)
    {
        yield return new WaitForSeconds(delay);
        RemoveSubObj(mainObj);
    }

    void RemoveMainObj(Objectives mainObj)
    {
        activeObj.Remove(mainObj);
        DisplayObjective();
        if (activeObj.Count == 0)
        {
            ObjectiveText.text = "All objectives completed!";
        }
    }

    void RemoveSubObj(Objectives subObj)
    {
        foreach (Objectives mainObj in activeObj)
        {
            if (mainObj.subobjectives.Contains(subObj))
            {
                mainObj.subobjectives.Remove(subObj);
                break;
            }
        }
        DisplayObjective();
    }

    public void UpdateQuestCount()
    {
        DisplayObjective();
    }

    public void AcceptQuest(Objectives newObj)
    {
        if (!activeObj.Contains(newObj))
        {
            activeObj.Add(newObj);
            DisplayObjective();
        }
        else
        {
            Debug.Log("Already Accepted");
        }
    }
}
