using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using SimpleJSON;
 
public class ScoreManager : MonoBehaviour
{
    public int score = 0;
    public GameObject scoreGameObject;
    public GameObject gameOverScoreGameObject;
    public GameObject lastRecordGameObject;
    public GameObject newRecordScreen;
    public GameObject newRecordGameObject;

    public string recordsURL = "http://192.168.56.101:5000/v1/records";
    WWW records;

    Text inGameScore;               // score in game
    Text gameOverScore;             // final score from the last game from the game over screen
    Text gameOverScreenRecord;      // last record from game over screen
    Text newRecord;                 // new record score text from new record screen

    private SnakeController snake;
    private Animator gameOverAnimator;

    private int[] highScores;

    //private DebugUI debugUI;

    void Awake()
    {
        snake = GameObject.Find("_GM").GetComponent<SnakeController>();
        gameOverAnimator = GameObject.Find("GameOver").GetComponent<Animator>();
        inGameScore = scoreGameObject.GetComponent<Text>();
        gameOverScore = gameOverScoreGameObject.GetComponent<Text>();
        gameOverScreenRecord = lastRecordGameObject.GetComponent<Text>();
        newRecord = newRecordGameObject.GetComponent<Text>();
        score = 0;

        // set the debug ui component
        //debugUI = GameObject.Find("_GM").GetComponent<DebugUI>();

        highScores = new int[11];

        highScores[0] = PlayerPrefs.GetInt("score01");
        highScores[1] = PlayerPrefs.GetInt("score02");
        highScores[2] = PlayerPrefs.GetInt("score03");
        highScores[3] = PlayerPrefs.GetInt("score04");
        highScores[4] = PlayerPrefs.GetInt("score05");
        highScores[5] = PlayerPrefs.GetInt("score06");
        highScores[6] = PlayerPrefs.GetInt("score07");
        highScores[7] = PlayerPrefs.GetInt("score08");
        highScores[8] = PlayerPrefs.GetInt("score09");
        highScores[9] = PlayerPrefs.GetInt("score10");

        // this is to hold the last score. When we sort it later
        // if still less than all the scores, will be left out.
        highScores[10] = 0;

        // Start to get the last records
        records = new WWW(recordsURL);
    }

    // Update is called once per frame
    void Update()
    {
        inGameScore.text = "" + score;
    }

    public void IncreaseScore(int amount)
    {
        score += amount;
    }

    public void gameOver()
    {
        Debug.Log("Game Over");
        snake.pauseGame();

        // saveRecordToNetwork();

        // save current score and sort the list of records
        if (saveScoreAndReturnTrueifNewRecord())
        {
            Debug.Log("This is a new record!");

            newRecord.text = highScores[0] + " pts";
            newRecordScreen.SetActive(true);
        }

        // fill record from the game over screen
        gameOverScreenRecord.text = highScores[0] + " pts";

        // fill this game score from the game over screen
        gameOverScore.text = score + "";
        gameOverAnimator.SetTrigger("gameOver");
    }

    void saveRecordToNetwork()
    {
        // Parse records from JSON
        //JSONNode j = JSON.Parse(records.text);
        //Debug.Log("WWW Records: " + j["records"].Value);
        //Debug.Log("First Record:" + j["records"][0]);

    }

    void eraseAllScores()
    {
        PlayerPrefs.SetInt("score01", 0);
        PlayerPrefs.SetInt("score02", 0);
        PlayerPrefs.SetInt("score03", 0);
        PlayerPrefs.SetInt("score04", 0);
        PlayerPrefs.SetInt("score05", 0);
        PlayerPrefs.SetInt("score06", 0);
        PlayerPrefs.SetInt("score07", 0);
        PlayerPrefs.SetInt("score08", 0);
        PlayerPrefs.SetInt("score09", 0);
        PlayerPrefs.SetInt("score10", 0);
    }

    /**
     * Check if this is a new record
     * Higher value is returned in position 0
     */
    bool saveScoreAndReturnTrueifNewRecord()
    {
        bool isHighScore = false;

        if (score > highScores[0])
        {
            isHighScore = true;
        }
      
        highScores[10] = score;

        Debug.Log(highScores[0] + " " + highScores[1] + " " + highScores[2]);

        Array.Sort(highScores);
        Array.Reverse(highScores);

        PlayerPrefs.SetInt("score01", highScores[0]);
        PlayerPrefs.SetInt("score02", highScores[1]);
        PlayerPrefs.SetInt("score03", highScores[2]);
        PlayerPrefs.SetInt("score04", highScores[3]);
        PlayerPrefs.SetInt("score05", highScores[4]);
        PlayerPrefs.SetInt("score06", highScores[5]);
        PlayerPrefs.SetInt("score07", highScores[6]);
        PlayerPrefs.SetInt("score08", highScores[7]);
        PlayerPrefs.SetInt("score09", highScores[8]);
        PlayerPrefs.SetInt("score10", highScores[9]);

        if(Debug.isDebugBuild)
        {
            return true;
        }

        // return true if this is a new record!!!
        return isHighScore;
    }
}
