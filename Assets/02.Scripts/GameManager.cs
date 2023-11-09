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
    public int maxBackCount = 3;

    public float currentTiem = 0;
    public float dieCoolTime = 0.35f;

    public bool isStart = true;
    public bool isEnd = false;

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

        playerPos.Clear();
        ballPos.Clear();
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
                UIManager.Instance().StageClear();
            }
            else
            {
                Debug.Log("Stage All Clear");
            }
        }
    }

    public void CheckBackPosition()
    {
        playerPos.Push(playerController.transform.position);

        List<Vector3> _ballPos = new List<Vector3>();
        for(int i = 0; i < balls.Length; i++)
        {
            _ballPos.Add(balls[i].transform.position);
        }
        ballPos.Push(_ballPos);
    }

    public void RollBack()
    {
        if (playerPos.Count != 0)
        {
            if (maxBackCount == 0)
            {
                isEnd = true;
                playerController.playerAnimation.SetTrigger("DIE");

                while (currentTiem < dieCoolTime)
                {
                    currentTiem += Time.deltaTime;
                }

                UIManager.Instance().GameOver();
                return;
            }
            UIManager.Instance().HPSlider(maxBackCount);
            maxBackCount--;

            playerController.transform.position = playerPos.Pop();

            List<Vector3> _ballPos = ballPos.Pop();
            for (int i = 0; i < balls.Length; i++)
            {
                balls[i].transform.position = _ballPos[i];
            }
        }
    }

    public void NextStage()
    {
        mapGenerator.MapDestroy(currentLv);
        UIManager.Instance().timer = 0;
        UIManager.Instance().stageClearPanel.SetActive(false);
        isStart = true;
    }

    public void Retry()
    {
        mapGenerator.MapDestroy(currentLv);
    }
}
