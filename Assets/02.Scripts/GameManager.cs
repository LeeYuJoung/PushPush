using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance()
    {
        return instance;
    }

    public MapGenerator mapGenerator;
    public PlayerController playerController;

    public Stack<Vector3> playerPos = new Stack<Vector3>();
    public Stack<List<Vector3>> ballPos = new Stack<List<Vector3>>();

    public GameObject[] buckets;
    public GameObject[] balls;

    public int currentCount;
    public int maxCount;
    public int correctCount;
    public int inCorrectCount;
    public int currentLv = 1;
    public int maxLv = 10;

    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void SetBucketsAndBalls()
    {
        StartCoroutine(GetBucketsAndBalls());
    }

    IEnumerator GetBucketsAndBalls()
    {
        yield return buckets = new GameObject[GameObject.FindGameObjectsWithTag("Bucket").Length];
        yield return balls = new GameObject[GameObject.FindGameObjectsWithTag("Ball").Length];

        yield return buckets = GameObject.FindGameObjectsWithTag("Bucket");
        yield return balls = GameObject.FindGameObjectsWithTag("Ball");

        currentCount = 0;
        maxCount = balls.Length;
    }

    public void CheckBallPosition()
    {
        for(int i = 0; i < buckets.Length; i++)
        {
            for(int j =  0; j < balls.Length; j++)
            {
                if (buckets[i].transform.position == balls[j].transform.position)
                {
                    correctCount++;
                    currentCount = correctCount;
                }
            }
        }

        correctCount = 0;

        if(currentCount == maxCount)
        {
            currentLv++;
            currentCount = 0;
            maxCount = 0;

            if(currentLv != maxLv +1)
            {
                mapGenerator.MapDestroy(currentLv);
            }
            else
            {
                Debug.Log("Stage All Clear");
            }
        }
    }

    public void RollBack()
    {

    }

    public void Retry()
    {
        mapGenerator.MapDestroy(currentLv);
    }
}
