using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using System.Linq;

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
    public int maxScore2;
    public string maxScorer2;
    public int maxScore3;
    public string maxScorer3;
    private string playerID;
    public TMP_InputField inputField;
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
        Debug.Log(maxScore);
        Debug.Log(maxScorer);
    }

    public void AddSession(int points)
    {
        namesList.Add(new ScoreData { PlayerScore = points, Player = playerID });
        Debug.Log(namesList[0].Player);
        Debug.Log(namesList[1].Player);
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
        string json = JsonUtility.ToJson(data);

        // Here we need to change default directory back to something more generic also
        File.WriteAllText("C://Alf/Unity Projects" + "/savefile.json", json);
    }
public void LoadNames()
    {
        string path = "C://Alf/Unity Projects" + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Savedata data = JsonUtility.FromJson<Savedata>(json);
            if (data.savedList != null) namesList = data.savedList;
        }
    }
}
