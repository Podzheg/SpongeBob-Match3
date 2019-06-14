using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class GameRuler : MonoBehaviour
{
    [SerializeField]
    TMP_Text scoreCount;

    [SerializeField]
    TMP_Text moveCount;

    [SerializeField]
    GameStats stats;

    [SerializeField]
    GameObject sadScreen;

    private int score;

    private bool gotHs;

    private int Score
    {
        get { return score; }
        set
        {
            //Showing scroe change on screen
            score = value;
            scoreCount.text = score.ToString();
        }
    }

    private int moves;
    private int Moves
    {
        get { return moves; }
        set
        {
            //Showing scroe change on screen
            moves = value;
            moveCount.text = moves.ToString();
        }
    }
    void Awake()
    {
        moves = stats.InitialMoves;
        moveCount.text = moves.ToString();
        gotHs = false;
        Time.timeScale = 1;
    }

    //Calculating scores and moves on matched event
    public void Matched(List<GameObject> matchedItems, bool vertical)
    {
        switch (matchedItems.Count)
        {
            case 3:
                Moves += stats.matched3Moves;
                Score += stats.matched3Scores;
                break;
            case 4:
                Moves += stats.matched4Moves;
                Score += stats.matched4Scores;
                break;
            case 5:
                Moves += stats.matched5Moves;
                Score += stats.matched5Scores;
                break;
        }

        if (matchedItems.Count > 5)
        {
            Moves += stats.matchedManyMoves;
            Score += stats.matchedManyScores;
        }
    }

    #region Calculating moves if not matched
    public void OnFoodEaten()
    {
       StartCoroutine(OnClickMovesCount());
    }

     private IEnumerator OnClickMovesCount()
    {
        if (stats.matched)
            yield break;
        Moves -= stats.notMatchedClickMovesPenalty;
        Score += stats.notMatchedClickScore;
        moveCount.text = moves.ToString();
        //Waiting if there is a match that will save our player
        yield return new WaitForSeconds(1.25f);
        if (moves <= 0)
        {
            EndGame();
        }
    }
    #endregion

    #region End Game
    private void EndGame()
    {
        for (int i = 0; i < 5; i++)
        {
            string hs = PlayerPrefs.GetString(i.ToString(), "0");
            string[] hsa = hs.Split(new char[] { ' ' }); //split highscore string to stringarray to get scores
            if (score <= int.Parse(hsa[0]))
            {
                continue;
            }
            else
            {
                gotHs = true;               
                //moving all other scores down in rank
                for (int j = 4; j > i; j--)
                {
                    string tempHs = PlayerPrefs.GetString((j - 1).ToString(), "");
                    //if(j+1<5)
                    PlayerPrefs.SetString((j).ToString(), tempHs);
                }
                //setting new highscore
                string newHs = score.ToString() + " " + System.DateTime.Now.ToString() + " y"; //y for detecting new highscore on leaderboard screen
                PlayerPrefs.SetString(i.ToString(), newHs);
                SceneManager.LoadScene("Highscore");
                break;
            }
        }
        if (!gotHs)
        {
            sadScreen.SetActive(true);
        }
    }

    void CheckHighScore()
    {
        int tempScore = PlayerPrefs.GetInt("maxscore");
        if (score > tempScore)
            PlayerPrefs.SetInt("maxscore", Score);
    }
    #endregion
}
