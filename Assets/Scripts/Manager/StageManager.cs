using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager instance;
    public Stage stage;
    public int stageCount;

    private void Awake()
    {
        instance = this;
    }
}
