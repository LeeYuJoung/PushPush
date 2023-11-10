using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject[] mapObjectsPrefabs;
    public string dataPath;
    public List<Dictionary<string, object>> data;

    void Start()
    {
        GameManager.Instance().isStart = true;
        UIManager.Instance().stageText.text = string.Format("Stage{0:00}", PlayerPrefs.GetInt("CurrentLv"));
        LoadMapData(PlayerPrefs.GetInt("CurrentLv"));

        for (int i = 0; i < 12; i++)
        {
            for(int j = 0; j < 12; j++)
            {
                GameObject ground = Instantiate(mapObjectsPrefabs[0]);
                ground.gameObject.name = ground.tag + $"( {j}, {i} )";
                ground.transform.parent = GameObject.Find("Ground").transform;
                ground.transform.localPosition = new Vector3(j, -i, 0);
            }
        }

        MakeMap();
    }

    public void LoadMapData(int stringNum)
    {
        dataPath = "Map/Lv" + stringNum;
        data = CSVReader.Read(dataPath);
    }

    public void MakeMap()
    {
        for(int i = 0; i < 12; i++)
        {
            for(int j = 0; j < 12; j++)
            {
                int dataSet = (int)data[i][j.ToString()];

                if(dataSet != 0)
                {
                    GameObject mapObject = Instantiate(mapObjectsPrefabs[dataSet]);

                    switch(mapObject.tag)
                    {
                        case "Wall":
                            mapObject.gameObject.name = mapObject.tag + $"( {j}, {i} )";
                            mapObject.transform.parent = GameObject.Find("Map").transform;
                            mapObject.transform.localPosition = new Vector3(j, -i, 0);
                            break;
                        case "Bucket":
                            mapObject.gameObject.name = mapObject.tag + $"( {j}, {i} )";
                            mapObject.transform.parent = GameObject.Find("Map").transform;
                            mapObject.transform.localPosition = new Vector3(j, -i, 0);
                            break;
                        case "Ball":
                            mapObject.gameObject.name = mapObject.tag + $"( {j}, {i} )";
                            mapObject.transform.parent = GameObject.Find("Map").transform;
                            mapObject.transform.localPosition = new Vector3(j, -i, 0);
                            break;
                        case "Player":
                            mapObject.gameObject.name = mapObject.tag + $"( {j}, {i} )";
                            mapObject.transform.parent = GameObject.Find("Map").transform;
                            mapObject.transform.localPosition = new Vector3(j, -i, 0);
                            GameManager.Instance().playerController = mapObject.GetComponent<PlayerController>();
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        GameManager.Instance().SetBucketsAndBalls();
    }

    public void MapDestroy(int lv)
    {
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
        GameObject[] buckets = GameObject.FindGameObjectsWithTag("Bucket");
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        for(int i = 0; i <  walls.Length; i++)
        {
            Destroy(walls[i]);
        }

        for (int i = 0; i < buckets.Length; i++)
        {
            Destroy(buckets[i]);
        }

        for (int i = 0; i < balls.Length; i++)
        {
            Destroy(balls[i]);
        }

        Destroy(player);

        LoadMapData(lv);
        MakeMap();
    }
}
