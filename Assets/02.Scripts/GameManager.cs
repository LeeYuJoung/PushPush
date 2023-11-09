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

    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
    }


}
