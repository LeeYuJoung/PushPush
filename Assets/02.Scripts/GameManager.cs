using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public int clearLv = 1;
    public int maxLv = 10;
    public int maxBackCount = 3;

    public bool isStart = false;
    public bool isEnd = false;

    void Start()
    {
        if(instance != this && instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        currentLv = PlayerPrefs.GetInt("CurrentLv");
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

            if(PlayerPrefs.GetInt("ClearLv") < currentLv)
            {
                PlayerPrefs.SetInt("ClearLv", currentLv);
            }
            PlayerPrefs.SetInt("CurrentLv", currentLv);

            if (currentLv != maxLv +1)
            {
                UIManager.Instance().StageClear();
                AudioManager.Instance().OnAudioPlay(AudioManager.Instance().stageClearSound);
                LankManager.Instance().AddLankData(currentLv - 1, UIManager.Instance().timer);
                LankManager.Instance().StageBestTime(currentLv - 1);
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
                AudioManager.Instance().OnAudioPlay(AudioManager.Instance().dieSound);
                StartCoroutine(OnDie());

                return;
            }
            UIManager.Instance().HPSlider(maxBackCount);
            AudioManager.Instance().OnAudioPlay(AudioManager.Instance().hpLessSound);
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
        UIManager.Instance().stageText.text = string.Format("Stage{0:00}", currentLv);
        isStart = true;
    }

    public void Retry()
    {
        mapGenerator.MapDestroy(currentLv);
        UIManager.Instance().timer = 0;
        UIManager.Instance().gameOverPanel.SetActive(false);

        for(int i = 0; i < 3; i++)
        {
            Slider _hpSlider = UIManager.Instance().hpSliders[i].GetComponent<Slider>();
            _hpSlider.value = 1;
        }
        maxBackCount = 3;
        isStart = true;
    }

    IEnumerator OnDie()
    {
        yield return new WaitForSeconds(0.85f);
        UIManager.Instance().GameOver();
        AudioManager.Instance().OnAudioPlay(AudioManager.Instance().gameOverSound);
    }
}
