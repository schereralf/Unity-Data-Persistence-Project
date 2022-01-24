using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR 
using UnityEditor;
#endif

public class BonkleGameManager : MonoBehaviour
{
    public static BonkleGameManager Instance { get; set; }
    public List<ScoreData> namesList;
    public int maxScore;
    public string maxScorer;
    private int maxScore2;
    private string maxScorer2;
    private int maxScore3;
    private string maxScorer3;
    private string playerID;
    public TMP_InputField inputField;
    public TMP_Text Scorer1;
    public TMP_Text Scorer2;
    public TMP_Text Scorer3;

    private void Awake()
    {
        // Awake ensures this is the instance to be shared across scenes and its the only one, plus we initialize a simple list of player+score data
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        namesList = new List<ScoreData>();
    }

    public void AddNewPlayer()
    {
        //Load player name from start menu input field, load past data from json repository.
        playerID = inputField.text;
        LoadNames();

        // next, check the past scores for highest three using three breathtakingly brutal if statements.....

        foreach (ScoreData item in namesList)
        {
            if (item.PlayerScore > maxScore)
            {
                maxScore = item.PlayerScore;
                maxScorer = item.Player;
            }
            else if (item.PlayerScore > maxScore2)              
            {                    
                maxScore2 = item.PlayerScore;                    
                maxScorer2 = item.Player;                
            }
            else if (item.PlayerScore > maxScore3)
            {
                maxScore3 = item.PlayerScore;
                maxScorer3 = item.Player;
            }
        }

        //Here we still need to List highest top scores and names on the glory board
        Scorer1.text = $"{maxScorer} : {maxScore}";
        Scorer2.text = $"{maxScorer2} : {maxScore2}";
        Scorer3.text = $"{maxScorer3} : {maxScore3}";
    }

    public void AddSession(int points)
    {
        namesList.Add(new ScoreData { PlayerScore = points, Player = playerID });
    }
// Begin links to Start button
    public void Begin()
    {
        SceneManager.LoadScene(1);
    }
// Exit links to Exit process thats initiated from MainManager
    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
    Application.Quit();
#endif
    }

    // Serializable section.  Key is that the struct for the player+score data and its two variables are serializable, otherwise a list of such structs converts to {} when we use JsonUtility

[System.Serializable]

    public struct ScoreData
    {
        public int PlayerScore;
        public string Player;
    }
    public class Savedata
    {
        public List<ScoreData> savedList;
    }

public void SaveNames()
    {
        Savedata data = new Savedata();    
        data.savedList = Instance.namesList;
        //Debug.Log(data.savedList[0].PlayerScore);
        string json = JsonUtility.ToJson(data);
        //Debug.Log(json);
        // Saving json copy of savedList in generic directory
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }
public void LoadNames()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Savedata data = JsonUtility.FromJson<Savedata>(json);
            if (data.savedList != null) namesList = data.savedList;
        }
    }
}
