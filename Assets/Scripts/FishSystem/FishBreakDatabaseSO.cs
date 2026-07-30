using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Fish Break Database")]
public class FishBreakDatabaseSO : ScriptableObject
{
    [System.Serializable]
    public class FishBreakMap
    {
        public int growthStage;
        public List<Outcome> outcomes;
    }

    [System.Serializable]
    public class Outcome
    {
        public BreakCircumstance circumstance;
        public FishItem result;
    }

    public List<FishBreakMap> database;

    public FishItem GetBreakResult(int stage, BreakCircumstance circumstance)
    {
        var map = database.Find(x => x.growthStage == stage);
        if (map != null)
        {
            var outcome = map.outcomes.Find(o => o.circumstance == circumstance);
            if (outcome != null) return outcome.result;
        }

        Debug.LogError($"No break result found for {stage} under {circumstance}");
        return default;
    }
}
