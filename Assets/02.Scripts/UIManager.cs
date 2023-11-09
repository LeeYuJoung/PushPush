using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance()
    {
        return instance;
    }

    public GameObject stageClearPanel;
    public GameObject gameOverPanel;
    public GameObject[] hpSliders;

    public Text stageFinalTimeText;
    public Text timeText;
    public float timer = 0;

    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Update()
    {
        if (GameManager.Instance().isStart)
        {
            TimeControll();
        }
    }

    public void TimeControll()
    {
        timer += Time.deltaTime;
        float min = timer / 60 % 60;
        float sec = timer % 60;

        timeText.text = string.Format("{0:00}:{1:00}", min, sec);
    }

    public void HPSlider(int backNum)
    {
        StartCoroutine(HPSliderControll(backNum));
    }

    IEnumerator HPSliderControll(int backNum)
    {
        Slider _slider = hpSliders[backNum - 1].GetComponent<Slider>();

        while(_slider.value != 0)
        {
            _slider.value -= 0.1f;
            yield return new WaitForSeconds(0.15f);
        }


        hpSliders[backNum - 1].SetActive(false);

    }

    public void StageClear()
    {
        stageFinalTimeText.text = string.Format("Time {0:00}:{1:00}", timer / 60 % 60, timer % 60);
        GameManager.Instance().isStart = false;
        stageClearPanel.SetActive(true);
    }

    public void GameOver()
    {
        GameManager.Instance().isStart = false;
        gameOverPanel.SetActive(true);
    }
}
