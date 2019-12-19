using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private float playerOneScore;
    private float playerTwoScore;

    [SerializeField] private GameObject scorePanel;
    [SerializeField] private Text playerOneScoreDisplay;
    [SerializeField] private Text playerTwoScoreDisplay;

    static private ScoreManager instance;
    static public ScoreManager Instance { get { return instance; }}


    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        
    }

    public void StartGame()
    {
        Invoke("RemoveDisplay", 1.5f);
    }

    public void AddScore(string scoringPlayer)
    {
        if (scoringPlayer == "Player1")
        {
            playerOneScore++;
        }
        else
        {
            playerTwoScore++;
        }

        UpdateDisplay();
    }

    private void ResetScores()
    {
        playerOneScore = 0;
        playerTwoScore = 0;
    }

    private void UpdateDisplay()
    {
        scorePanel.SetActive(true);
        playerOneScoreDisplay.text = playerOneScore.ToString();
        playerTwoScoreDisplay.text = playerTwoScore.ToString();
        Invoke("RemoveDisplay", 1.5f);
    }

    void RemoveDisplay()
    {
        scorePanel.SetActive(false);
    }
}
