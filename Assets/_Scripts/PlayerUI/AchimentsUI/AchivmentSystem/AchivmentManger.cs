using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace AchievementSystem
{
    public class AchievementManager : MonoBehaviour
    {
        public AudioSource playNoteSound;

        [SerializeField] private GameObject achNotePrefab;
        [SerializeField] private Transform achNoteParent;

        [System.Serializable]
        public class Achievement
        {
            [HideInInspector] public Image bImage;
            [HideInInspector] public Text achTitle;
            [HideInInspector] public Text achDesc;

            [HideInInspector] public List<Image> woodImages;
            [HideInInspector] public List<Image> stoneImages;
            [HideInInspector] public List<Image> explorationImages;

            public int code;
            public string achTitleText;
            public string achDescText;
            public int count;
            public int triggerCount;
            public bool isAchieved;
            public AchievementType type;

            [HideInInspector]
            public GameObject uiObject; // Reference to the instantiated UI object
        }

        [SerializeField] private List<Achievement> allAchievements = new List<Achievement>();
        private HashSet<int> achievedAchievements = new HashSet<int>();

        private void Start()
        {
            InitializeAchievements();
            SetupAchievementUI();
        }

        private void InitializeAchievements()
        {
            // Initialize achievements
            allAchievements = InstantiateCountAchivments();
        }

        private void SetupAchievementUI()
        {
            foreach (var achievement in allAchievements)
            {
                // Check if the achievement is already achieved
                if (!achievedAchievements.Contains(achievement.code))
                {
                    GameObject achNote = Instantiate(achNotePrefab, achNoteParent);
                    achievement.uiObject = achNote; // Set the reference to the instantiated UI object
                    SetAchievementUI(achNote, achievement);

                    // Mark the achievement as achieved
                    achievedAchievements.Add(achievement.code);
                }
            }
        }

        private void SetAchievementUI(GameObject achNote, Achievement achievement)
        {
            Image achImage = achNote.transform.Find("achNote").GetComponent<Image>();
            Text titleText = achNote.transform.Find("achTitle").GetComponent<Text>();
            Text descText = achNote.transform.Find("achDesc").GetComponent<Text>();

            achImage.sprite = achievement.bImage.sprite;
            titleText.text = achievement.achTitleText;
            descText.text = achievement.achDescText;

            SetWoodImages(achNote, achievement);

            // Reset UI values after a delay
            Invoke("ResetUI", 4f);
        }

        private void SetWoodImages(GameObject achNote, Achievement achievement)
        {
            if (achievement.woodImages != null)
            {
                foreach (var woodImage in achievement.woodImages)
                {
                    Image instantiatedWoodImage = Instantiate(woodImage, achNote.transform);
                    // Set the position or other properties of instantiatedWoodImage as needed
                }
            }
        }

        private void ResetUI()
        {
            foreach (var achievement in allAchievements)
            {
                achievement.bImage.sprite = null;
                achievement.achTitleText = "";
                achievement.achDescText = "";
                DestroyWoodImages(achievement.uiObject);
            }
        }

        private void DestroyWoodImages(GameObject uiObject)
        {
            foreach (Transform child in uiObject.transform)
            {
                Destroy(child.gameObject);
            }
        }

        private List<Achievement> InstantiateCountAchivments()
        {
            List<Achievement> achievements = new List<Achievement>();

            // Assuming you have assigned the UI Images through the Unity Editor
            achievements.Add(new Achievement
            {
                code = 1,
                bImage = GameObject.Find("AchImage").GetComponent<Image>(),
                achTitleText = "FIRST TREE CUT!",
                achDescText = "You collected 5 wood!",
                triggerCount = 5,
                type = AchievementType.Wood,
                woodImages = new List<Image> { /* Add wood-related images here */ }
            });

            // Add more count-based achievements...

            return achievements;
        }

        public enum AchievementType
        {
            Wood,
            Stone,
            Enemies
            // Add more types as needed
        }
    }
}


