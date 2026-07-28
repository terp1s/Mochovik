using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestState { NotStarted, InProgress, ReadyToComplete, Completed }
public class PotionQuestManager : MonoBehaviour
{
    public QuestState State;
    public Logik logik;
    public SardinePuzzleManager SardinePuzzleManager;

    public void StartQuest()
    {
        State = QuestState.InProgress;
    }
}
