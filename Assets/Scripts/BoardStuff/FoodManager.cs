using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FoodManager : MonoBehaviour
{
    #region Parameters Stuff
    [SerializeField]
    GameStats stats;

    //Patrick is used for eating visual effect
    [SerializeField]
    private GameObject patrick;

    public FoodEatenEventClass foodEaten;

    private Vector2 size;
    #endregion

    //Gets food item size
    void Awake()
    {
        size = gameObject.GetComponent<SpriteRenderer>().bounds.size;
    }

    #region Getting eaten on click & raising eaten event
    private void OnMouseDown()
    {
        if (!stats.isEating)
        {
            gameObject.GetComponent<AudioSource>().Play();
            //Preventing Patrick from moving with the food when it is adviced
            gameObject.GetComponent<Animator>().enabled = false;
            gameObject.transform.rotation =  Quaternion.identity;

            StartCoroutine(Eaten());
        }
    }
    

    public IEnumerator Eaten(bool verticalMatched = false)
    {
        stats.isEating = true;
        patrick.SetActive(true);
        yield return new WaitForSeconds(1f);
        patrick.SetActive(false);
        if (!verticalMatched)
            LiftUpperFoodsDown(1);
        foodEaten.Invoke(gameObject, verticalMatched);
    }
    #endregion

    #region Food Movement

    //Gets all the above food and starts thier moving down func
    public void LiftUpperFoodsDown(int ammount)
    {
        RaycastHit2D[] upperFood = Physics2D.RaycastAll(transform.position + new Vector3(0, size.y / 2 + 0.01f, 0), Vector2.up, size.y * stats.boardYSize);
        foreach (RaycastHit2D food in upperFood)
        {
            food.collider.gameObject.GetComponent<FoodManager>().MoveDown(ammount);
        }
    }

    //Just starts coroutine
    private void MoveDown(int ammount)
    {
        StartCoroutine(MoveObject(ammount));
    }

    //Smoothly move down for 1 position
    private IEnumerator MoveObject(int ammount = 1)
    {
        float startTime = Time.time;
        float overTime = 1f;
        Vector2 newYPos = new Vector2(transform.position.x, transform.position.y - size.y * (ammount));
        while (Time.time < startTime + overTime)
        {
            transform.position = Vector2.Lerp(transform.position, newYPos, (Time.time - startTime) / overTime);
            yield return null;
        }
        transform.position = newYPos;
    }


    // Iplements new food item got from object pool to the board.
    public IEnumerator FromHeavenToEarth(Vector2 targetPos, float overTime = 1f)
    {
        float startTime = Time.time;
        while (Time.time < startTime + overTime)
        {
            transform.position = Vector2.Lerp(transform.position, targetPos, (Time.time - startTime) / overTime);
            yield return null;
        }
        transform.position = targetPos;
        stats.isEating = false;
        stats.matched = false;
    }
    #endregion
}
