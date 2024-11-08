using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PlayerController player;
    [SerializeField] private TextMeshProUGUI goldText;
    public int gold;

    [SerializeField] private float exp;
    public float Exp
    {
        get { return exp; }
        set 
        {
            exp = value; 
            if(exp >= maxExp)
            {
                exp = 0;
                Level++;
                maxExp = maxExp + 50;
            }
        }
    }
    public float maxExp;
    [SerializeField] private Slider expSlider;

    [SerializeField] private int level;
    public int Level
    { 
        get { return level; }
        set { level = value; }
    }
    
    private void Awake()
    {
        instance = this;
        maxExp = 50;
    }

    private void Update()
    {
        goldText.text = gold.ToString();
        expSlider.value = Exp / maxExp;
    }
}
