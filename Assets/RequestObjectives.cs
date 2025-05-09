using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RequestObjectives : MonoBehaviour
{
    public objectivesManager objManager;
    public Objectives quest;

    public TextMeshProUGUI name, description, requirement;
    public conversationStarter cs;
    public bool isAccepted;
    public gameData data;

    private void Update()
    {
        if(cs != null)
        {
            DisplayQuest();
        }
    }
    public void DisplayQuest()
    {
        name.text = quest.objectiveName;
        description.text = quest.description;
        requirement.text = "Requirement to Complete: " + quest.requireCount.ToString();
    }
    public void AcceptQuest()
    {
        if (cs != null)
        {
            if(cs.inConversation == true && isAccepted == false)
            {
                isAccepted = true;
                objManager.AcceptQuest(quest);
            }
        }
    }

    public void DeclineQuest()
    {
        cs.inConversation = false;
    }

    public void RewardAccepted()
    {
        data.money += quest.Reward;
    }
}
