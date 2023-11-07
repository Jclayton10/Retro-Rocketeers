using NUnit.Framework.Constraints;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
  public List<Achievement> achievements;

    //Test to see if it works
    public int integer;
    public float floatingpoint;

    public bool AchiementUnlocked(string achievmentName)
    {
        bool result = false;
        if (achievements == null)
            return false;
        Achievement[] achievementArray = achievements.ToArray();
        Achievement a = Array.Find(achievementArray, ach => achievmentName == ach.title);
        if (a == null)
            return false;
        result = a.isAchived;
        return result;
    }

    private void Start()
    {
        InitializeAchievments();
    }
    private void InitializeAchievments()
    {
        if (achievements != null)
            return;

        achievements = new List<Achievement>();
        achievements.Add(new Achievement("Step By Step", "Set your integer to or over 100", (object o) => integer>=100));
        achievements.Add(new Achievement("Not So Precise", "Set your floating point to or over 50", (object o) => floatingpoint >= 50f));
    }

    private void Update()
    {
        CheckAchieventCompletion();
    }
   private void CheckAchieventCompletion()
    {
        if (achievements == null)
            return;

        foreach (var achievement in achievements)
        {
            achievement.UpdateCompletion();
        }
    }

}
public class Achievement
{
    public Achievement(string title, string description, Predicate<object> requirment)
    {
        this.title = title;
        this.description = description;
        this.requirment = requirment;
    }
    public string title;
    public string description;
    public Predicate<object> requirment;

    public bool isAchived;

    public void UpdateCompletion()
    {
        if (isAchived)
            return;

        if(RequirementsMet())
        {
            Debug.Log($"{title}: {description}");
            isAchived = true;
        }
            
    }
    public bool RequirementsMet()
    {
        return requirment.Invoke(null);
    }
}
