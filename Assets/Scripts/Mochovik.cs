using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mochovik : MonoBehaviour, IInteractable
{
    PotionQuestManager quest;
    public void Interact()
    {
        switch (quest.State)
        {
            case QuestState.NotStarted:
                quest.StartQuest();
                break;

            case QuestState.InProgress:
                break;

            case QuestState.ReadyToComplete:
                break;

            case QuestState.Completed:
                break;
        }
    }
}

