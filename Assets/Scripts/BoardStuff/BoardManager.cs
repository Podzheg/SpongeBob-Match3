using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    #region Parameters Stuff
    [SerializeField]
    GameStats stats;

    [SerializeField]
    List<Sprite> foodSprites = new List<Sprite>();

    [SerializeField]
    public GameObject foodItemPrefab;

    private GameObject[] matchingFood;

    private Vector2 foodItemSize;

    private Queue<GameObject> foodPool;
    #endregion

    void Start()
    {  
        //Setting up
        foodItemSize = foodItemPrefab.GetComponent<SpriteRenderer>().bounds.size;
        stats.itemWidth = foodItemSize.x;
        stats.itemHeight = foodItemSize.y;
        stats.boardStartPos = gameObject.transform.position;

        //Creating object pool
        foodPool = new Queue<GameObject>();
        FullfillPool(10);

        CreateBoard(foodItemSize.x, foodItemSize.y);  
    }

    #region Creating Board

    /// <summary>
    /// Creating board using food item size
    /// </summary>
    /// <param name="xOffset"></param> food item width
    /// <param name="yOffset"></param>  food item height
    private void CreateBoard(float xOffset, float yOffset)
    {
        float startX = transform.position.x;    
        float startY = transform.position.y;

        Sprite[] previousLeft = new Sprite[stats.boardYSize];
        Sprite previousBelow = null;

        for (int x = 0; x < stats.boardXSize; x++)
        {     
            for (int y = 0; y < stats.boardYSize; y++)
            {
                GameObject newFoodItem = Instantiate(foodItemPrefab, new Vector3(startX + (xOffset * x), startY + (yOffset * y), 0), foodItemPrefab.transform.rotation);

                newFoodItem.transform.SetParent(gameObject.transform);                

                //Checking not to create a board with preset matches
                List<Sprite> possiblefoodSprites = new List<Sprite>(); 
                possiblefoodSprites.AddRange(foodSprites); 
                possiblefoodSprites.Remove(previousLeft[y]); 
                possiblefoodSprites.Remove(previousBelow);              

                Sprite sprite = possiblefoodSprites[Random.Range(0, possiblefoodSprites.Count)];               
                newFoodItem.tag = sprite.name;
                newFoodItem.GetComponent<SpriteRenderer>().sprite = sprite;

                previousLeft[y] = sprite;
                previousBelow = sprite;
            }
        }
    }
    #endregion



    #region Object Pool Operations

    //Create object pool
    private void FullfillPool(int poolSize) {
        for (int i = 0; i <= poolSize; i++) {
            GameObject food = Instantiate(foodItemPrefab, new Vector3(0,0,0), Quaternion.identity);
            food.SetActive(false);
            food.transform.SetParent(gameObject.transform);
            ShuffleFood(food);
            foodPool.Enqueue(food);
        }
    }

    //Get new food from pool
    public void GetFromPool(GameObject oldFood) {
        //Lazy instantiation
        if (foodPool.Count <= 0) {
            GameObject food = Instantiate(foodItemPrefab, new Vector2(0,0), Quaternion.identity);
            food.SetActive(false);
            ShuffleFood(food); //Make sure the new food is not in predictable loop to keep the game interesting
            foodPool.Enqueue(food);
        }

        GameObject newFood = foodPool.Dequeue();
        ShuffleFood(newFood);
        newFood.transform.position = new Vector2(oldFood.transform.position.x, gameObject.transform.position.y + (foodItemSize.y * (stats.boardYSize + 3)));
        newFood.SetActive(true);
        Vector2 targetPos = new Vector2(oldFood.transform.position.x, gameObject.transform.position.y + (foodItemSize.y * (stats.boardYSize - 1)));
        //Setting new food to board
        StartCoroutine(newFood.GetComponent<FoodManager>().FromHeavenToEarth(targetPos));
    }

    //Special instantiation after vertical match
    public IEnumerator VerticalMatched(List<GameObject> matchedFood)
    {
        matchingFood = new GameObject[matchedFood.Count];
        for (int i = 0; i < matchedFood.Count; i++)
        {
            matchingFood[i] = matchedFood[i];
        }
        yield return new WaitForSeconds(1f);
        Debug.Log(matchingFood.Length);
        matchingFood[matchingFood.Length-1].GetComponent<FoodManager>().LiftUpperFoodsDown(ammount: matchingFood.Length);
        Debug.Log(matchingFood.Length);
        
        for (int i = 1; i <= matchingFood.Length; i++)
        {
            Debug.Log("created");
            GameObject newFood = foodPool.Dequeue();
            ShuffleFood(newFood);
            newFood.transform.position = new Vector2(matchingFood[i-1].transform.position.x, gameObject.transform.position.y + (foodItemSize.y * (stats.boardYSize + 3 + i)));
            newFood.SetActive(true);
            Vector2 targetPos = new Vector2(matchingFood[i-1].transform.position.x, gameObject.transform.position.y + (foodItemSize.y * (stats.boardYSize - i)));
            StartCoroutine(newFood.GetComponent<FoodManager>().FromHeavenToEarth(targetPos));
        }
    }

    //Randomizes food item
    private void ShuffleFood(GameObject food)
    {
        Sprite sprite = foodSprites[Random.Range(0, foodSprites.Count)];
        food.tag = sprite.name;
        food.GetComponent<SpriteRenderer>().sprite = sprite;
    }

    //Get the food back to pool
    public void BackToPool(GameObject food) {
        food.SetActive(false);
        foodPool.Enqueue(food);
    }
    #endregion


    #region On Event Raised Operations
    /// <summary>
    /// Method called when FootEaten event is raised. Returns clicked food object to pool and gets a new one from it, fullfilling the board
    /// </summary>
    /// <param name="food"></param> recives food gameobject that should be taken away and used for positioning new one
    public void OnFoodEaten(GameObject food, bool vertical)
    {
        if (!vertical)
            GetFromPool(food);

        BackToPool(food);
    }

    //On vertical match we call a special food instantiation
    public void Matched(List<GameObject> matchedFood, bool vertical)
    {
        if (vertical)
            StartCoroutine(VerticalMatched(matchedFood));
    }
    #endregion
}
