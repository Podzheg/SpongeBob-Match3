using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchSearcher : MonoBehaviour
{
    #region Parameters Stuff
    [SerializeField]
    GameStats stats;

    private List<GameObject> currentMatches;

    public MatchedEventClass matched;
    #endregion

    void Start()
    {
        StartCoroutine(Initialize());
    }

    //Checks if a game is created properly reloads if not
    IEnumerator Initialize()
    {
        currentMatches = new List<GameObject>();
        stats.isEating = false;
        yield return new WaitForSeconds(0.1f);
        //checking for at least 1 possible match
        if (!CheckPossibleMatches(stats.boardYSize, false))
        {
            if (!CheckPossibleMatches(stats.boardXSize, true)) SceneManager.LoadScene("Game");
        }
    }

    //Calls matches searching only when the board changes
    public void OnFoodEaten()
    {
        StartCoroutine(SearchForMatches(stats.boardYSize, false));
        StartCoroutine(SearchForMatches(stats.boardXSize, true));
    }


    #region Match Searching Core

    /// <summary>
    /// Core searching for matches function. 
    /// </summary>
    /// <param name="boardSize"></param> Board width for vertical search and board height for horizontal 
    /// <param name="vertical"></param> 'true' for searhing vertical matches, 'false' for horizontal
    /// <returns></returns>
    IEnumerator SearchForMatches(float boardSize, bool vertical)
    {
        Vector2 startPos = stats.boardStartPos;
        List<GameObject> matchingItems = new List<GameObject>();
        for (int y = 0; y < boardSize; y++)
        {
            RaycastHit2D[] itemsToMatch;
            matchingItems.Clear();
            GameObject previousItem = null;

            //Waiting if there is already a match that is being calculated
            while (stats.isEating)
            {
                yield return new WaitForEndOfFrame();
            }

            //Get food items to check match
            if (vertical)
            {
                itemsToMatch = Physics2D.RaycastAll(startPos, Vector2.up, stats.boardYSize * stats.itemHeight);
                startPos += new Vector2(stats.itemWidth, 0);
            } else
            {
                itemsToMatch = Physics2D.RaycastAll(startPos, Vector2.right, stats.boardXSize * stats.itemWidth);
                startPos += new Vector2(0, stats.itemHeight);
            }
            
            //Checking for a match
            for (int i = 0; i < itemsToMatch.Length; i++)
            {
                if (previousItem != null)
                {
                    if (previousItem.tag == itemsToMatch[i].collider.gameObject.tag)
                    {
                        //adding items to matched list
                        if (!matchingItems.Contains(previousItem)) matchingItems.Add(previousItem);
                        if (!matchingItems.Contains(itemsToMatch[i].collider.gameObject)) matchingItems.Add(itemsToMatch[i].collider.gameObject);
                        //if there is no more items and we have a match, we need to calculate it instead of just going next
                        if (i == itemsToMatch.Length - 1)
                            CountMatches(matchingItems, vertical);
                    }
                    else
                    {
                        CountMatches(matchingItems, vertical);
                    }
                }
                previousItem = itemsToMatch[i].collider.gameObject;
            }
        }
    }
   

    private void CountMatches(List<GameObject> matchingItems, bool vertical)
    {
        if (matchingItems.Count <= 5)
        {
            switch (matchingItems.Count)
            {
                case 0:
                case 1:
                case 2:
                    matchingItems.Clear();
                    break;
                case 3:
                    Matched(matchingItems, vertical);
                    break;
                case 4:
                    Matched(matchingItems, vertical);
                    break;
                case 5:
                    Matched(matchingItems, vertical);
                    break;
            }
        }
        //if player succeeded to get more than 5 items match
        else
        {
            Matched(matchingItems, vertical);
        }
    }

    //Calling matched event
    private void Matched(List<GameObject> matchingItems, bool verticalMatched)
    {
        stats.isEating = true;
        stats.matched = true;
        matched.Invoke(matchingItems, verticalMatched);
        //Could use foreach but I've read that 'for' is faster for Lists
        for (int i = 0; i < matchingItems.Count; i++)
        {
            StartCoroutine(matchingItems[i].GetComponent<FoodManager>().Eaten(verticalMatched));
        }
        matchingItems.Clear();
    }
    #endregion


    #region Possible Match Searching
    /// <summary>
    /// Additional search for possible matches function
    /// </summary>
    /// <param name="boardSize"></param> Board width for vertical search and board height for horizontal 
    /// <param name="vertical"></param> 'true' for searhing vertical matches, 'false' for horizont
    /// <returns></returns>
    private bool CheckPossibleMatches(float boardSize, bool vertical)
    {
        Vector2 startPos = stats.boardStartPos;
        List<GameObject> matchingItems = new List<GameObject>();
        for (int y = 0; y < boardSize; y++)
        {
            RaycastHit2D[] itemsToMatch;
            GameObject previousItem = null;

            if (vertical)
            {
                itemsToMatch = Physics2D.RaycastAll(startPos, Vector2.up, stats.boardYSize * stats.itemHeight);
                startPos += new Vector2(stats.itemWidth, 0);
            }
            else
            {
                itemsToMatch = Physics2D.RaycastAll(startPos, Vector2.right, stats.boardXSize * stats.itemWidth);
                startPos += new Vector2(0, stats.itemHeight);
            }

            for (int i = 0; i < itemsToMatch.Length; i++)
            {
                if (previousItem != null && (i+1) < boardSize)
                {
                    if (previousItem.tag == itemsToMatch[i+1].collider.gameObject.tag)
                    {
                       Vector2 checkPos = vertical ? new Vector2(itemsToMatch[i].transform.position.x, itemsToMatch[i].transform.position.y + (stats.itemHeight * 2)) : new Vector2(itemsToMatch[i].transform.position.x, itemsToMatch[i].transform.position.y + stats.itemHeight);
                        RaycastHit2D checkItem = Physics2D.Raycast(checkPos, Vector2.up);
                        if (checkItem.collider != null)
                        {
                            if (checkItem.collider.tag == previousItem.tag)
                            {
                                itemsToMatch[i].collider.gameObject.GetComponent<Animator>().SetTrigger("advice");
                                return true;
                            }
                        }
                    }
                }
                previousItem = itemsToMatch[i].collider.gameObject;
            }
        }
        return false;
    }
    #endregion
}
