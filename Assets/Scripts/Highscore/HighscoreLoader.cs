using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighscoreLoader : MonoBehaviour
{
    [SerializeField]
    TMP_Text[] scoreRankingPos;

    void Start()
    {
        Time.timeScale = 1;
        for (int i = 0; i < scoreRankingPos.Length; i++)
        {
            string hs = PlayerPrefs.GetString(i.ToString(), "");
            Debug.Log(hs);
        } 
            for (int i = 0; i < scoreRankingPos.Length; i++)
        {
            string hs = PlayerPrefs.GetString(i.ToString(), "");
            if (hs == "")
            {
                scoreRankingPos[i].text = ReadFormCSV();
            }
            else
            {
                if (hs.Contains("y"))
                {
                    scoreRankingPos[i].transform.parent.gameObject.GetComponent<Animator>().enabled = true;
                    hs = hs.Replace("y", "n");
                    PlayerPrefs.SetString(i.ToString(), hs);
                }
                string[] hsa = hs.Split(new char[] { ' ' });
                scoreRankingPos[i].text = hsa[0] + " " + hsa[1];
            }
        }
    }

    

    string ReadFormCSV()
    {
        TextAsset scores =  (TextAsset)Resources.Load("FirstTimeScore", typeof(TextAsset));
        return scores.text;
    }
}
