using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class conversationStarter : MonoBehaviour
{
    [SerializeField] private NPCConversation myConvversation;
    public bool inConversation;
    public Animator anim;
    public gunScript[] guns;
    public playerVision vision;
    public pauseMenuController pause;
    public RequestObjectives request;
    public Objectives obj;

    public bool inRange = false;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
            pause.cs = this;
            vision.cs = this;
            for(int i=0; i < guns.Length; i++)
            {
                guns[i].cs = this;
            }
        }
    }

    void Update()
    {
        if(inRange == true)
        {
            if (Input.GetKeyDown(KeyCode.I) && inConversation == false)
            {
                anim.SetBool("isTalking", true);
                inConversation = true;
                ConversationManager.Instance.StartConversation(myConvversation);
                if (request != null)
                        {
                    if (request.isAccepted == true)
                    {
                        ConversationManager.Instance.SetBool("isAcceptedQuest", true);
                    }
                   else
                    { 
                        ConversationManager.Instance.SetBool("isAcceptedQuest", false);
                    }
                }
                if (obj.isComplete == true)
                {
                    ConversationManager.Instance.SetBool("isQuestCleared", true);
                }
                else
                {
                    ConversationManager.Instance.SetBool("isQuestCleared", false);
                }
                
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            conversationDone();
        }
    }

    public void conversationDone()
    {
        inRange = false;
        pause.cs = null;
        vision.cs = null;
        for (int i = 0; i <guns.Length; i++)
        {
            guns[i].cs = null;
        }
        anim.SetBool("isTalking", false);
        inConversation = false;
        ConversationManager.Instance.EndConversation();
    }
}