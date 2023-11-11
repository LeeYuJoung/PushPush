using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LankManager : MonoBehaviour
{
    private static LankManager instance;
    public static LankManager Instance()
    {
        return instance;
    }

    public Dictionary<int, List<float>> lankingData = new Dictionary<int, List<float>>();

    void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;

            for(int i = 1; i <= 10; i++)
            {
                lankingData.Add(i, new List<float>());
            }

            DontDestroyOnLoad(gameObject);
        }
    }

    public void AddLankData(int _stage, float _clearTime)
    {
        lankingData[_stage].Add(_clearTime);
    }

    public void StageBestTime(int _stage)
    {
        List<float> clearTime = lankingData[_stage];
        float bestTime = clearTime[0];

        if (!PlayerPrefs.HasKey((_stage).ToString() + "BestTime"))
        {
            PlayerPrefs.SetFloat((_stage).ToString() + "BestTime", 0);
        }
        if (PlayerPrefs.GetFloat(_stage.ToString() + "BestTime") != 0)
        {
            bestTime = PlayerPrefs.GetFloat(_stage.ToString() + "BestTime");
        }

        for (int i =0; i < clearTime.Count; i++)
        {
            if (clearTime[i] < bestTime)
            {
                bestTime = clearTime[i];
            }
        }

        PlayerPrefs.SetFloat(_stage.ToString() + "BestTime", bestTime);
        UIManager.Instance().stageBestTimeText.text = string.Format("{0:00}:{1:00}", bestTime / 60 % 60, bestTime % 60);
    }

    public void ShowLanking()
    {
        for(int i = 0; i < lankingData.Count; i++)
        {
            if(!PlayerPrefs.HasKey((i + 1).ToString() + "BestTime"))
            {
                PlayerPrefs.SetFloat((i + 1).ToString() + "BestTime", 0);
            }

            float _time = PlayerPrefs.GetFloat((i + 1).ToString() + "BestTime");
            UIManager.Instance().lankingTexts[i].text = string.Format("Stage{0:00} {1:00}:{2:00}", i + 1, _time / 60 % 60, _time % 60);
        }
    }
}
