using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementManager : MonoBehaviour
{
    public static List<Achievement> achievements;

    public GameObject achNote;
    public GameObject achTitle;
    public GameObject achDesc;

    public Image woodImage;
    public Image rockImage;
    public Image enemyImage;
    public Image blankImage;

    public static int woodAchCount;
    public static int rockAchCount;
    public static int enemyAchCount;

    private bool isActive;

    private void Start()
    {
        InitializeAchievements();
    }

    private void InitializeAchievements()
    {
        if (achievements != null)
            return;

        achievements = new List<Achievement>();

        // Wood Achievements
        achievements.Add(new Achievement("FIRST TREE CUT!", "You cut 5 wood!", (object o) => woodAchCount >= 5, new List<Image> { woodImage }));
        achievements.Add(new Achievement("Do you have enough wood yet?", "You cut 20 wood!", (object o) => woodAchCount >= 20, new List<Image> { woodImage }));
        achievements.Add(new Achievement("How much wood do you need?", "You cut 40 wood!", (object o) => woodAchCount >= 40, new List<Image> { woodImage }));
        achievements.Add(new Achievement("Ok that's enough wood", "You cut 60 wood!", (object o) => woodAchCount >= 60, new List<Image> { woodImage }));
        achievements.Add(new Achievement("THE GOD OF DEFORESTATION", "You cut 100 wood!", (object o) => woodAchCount >= 100, new List<Image> { woodImage }));

        // Stone Achievements
        achievements.Add(new Achievement("Mining your first rocks!", "You mined 5 rocks!", (object o) => rockAchCount >= 5, new List<Image> { rockImage }));
        achievements.Add(new Achievement("Need more rocks!", "You mined 20 rocks!", (object o) => rockAchCount >= 20, new List<Image> { rockImage }));
        achievements.Add(new Achievement("The Miner", "You mined 40 rocks!", (object o) => rockAchCount >= 40, new List<Image> { rockImage }));
        achievements.Add(new Achievement("The Operation", "You mined 60 rocks!", (object o) => rockAchCount >= 60, new List<Image> { rockImage }));
        achievements.Add(new Achievement("GOD OF ROCK COLLECTING", "You mined 100 rocks!", (object o) => rockAchCount >= 100, new List<Image> { rockImage }));

        // Enemy Achievements
        achievements.Add(new Achievement("Youngling", "You killed your first enemy!", (object o) => enemyAchCount >= 1, new List<Image> { enemyImage }));
        achievements.Add(new Achievement("Monster Hunter!", "You killed 10 enemies!", (object o) => enemyAchCount >= 10, new List<Image> { enemyImage }));
        achievements.Add(new Achievement("Knight!", "You killed 20 enemies!", (object o) => enemyAchCount >= 20, new List<Image> { enemyImage }));
        achievements.Add(new Achievement("Master", "You killed 40 enemies!", (object o) => enemyAchCount >= 40, new List<Image> { enemyImage }));
        achievements.Add(new Achievement("Sensei", "You killed 60 enemies!", (object o) => enemyAchCount >= 60, new List<Image> { enemyImage }));
        achievements.Add(new Achievement("THE VENGEFUL ONE!", "You killed 100 enemies!", (object o) => enemyAchCount >= 80, new List<Image> { enemyImage }));
        // Add more achievements as needed
    }

    private void Update()
    {
        CheckAchievementCompletion();
    }

    private void CheckAchievementCompletion()
    {
        if (achievements == null)
            return;

        foreach (var achievement in achievements)
        {
            achievement.UpdateCompletion();
            if (achievement.achieved && !achievement.displayed)
            {
                DisplayAchievement(achievement);
            }
        }
    }

    private void DisplayAchievement(Achievement achievement)
    {
        StartCoroutine(ShowAchievement(achievement));
    }

    IEnumerator ShowAchievement(Achievement achievement)
    {
        isActive = true;
        achTitle.GetComponent<Text>().text = achievement.title;
        achDesc.GetComponent<Text>().text = achievement.description;

        foreach (Image image in achievement.achImages)
        {
            image.gameObject.SetActive(true);
        }

        achNote.SetActive(true);

        yield return new WaitForSeconds(4);

        ResetUI(achievement);
    }

    private void ResetUI(Achievement achievement)
    {
        achNote.SetActive(false);
        foreach (Image image in achievement.achImages)
        {
            image.gameObject.SetActive(false);
        }
        achTitle.GetComponent<Text>().text = "";
        achDesc.GetComponent<Text>().text = "";
        isActive = false;
        achievement.displayed = true;
    }
}

[System.Serializable]
public class Achievement
{
    public string title;
    public string description;
    public Predicate<object> requirement;
    public List<Image> achImages;

    public bool achieved;
    public bool displayed;

    public Achievement(string title, string description, Predicate<object> requirement, List<Image> achImages)
    {
        this.title = title;
        this.description = description;
        this.requirement = requirement;
        this.achImages = achImages;
    }

    public void UpdateCompletion()
    {
        if (achieved)
            return;

        if (RequirementsMet())
        {
            Debug.Log($"{title}: {description}");
            achieved = true;
        }
    }

    public bool RequirementsMet()
    {
        return requirement.Invoke(null);
    }
}