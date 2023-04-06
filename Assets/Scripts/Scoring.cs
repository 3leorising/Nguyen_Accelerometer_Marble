using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoring : MonoBehaviour
{
    public static Scoring instance;

    public Text scoreText;

    public int score = 0;

    private void Awake()
    {
        instance = this;
    }
   
    void Start()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    public void AddScore()
    {
        score += 10;
        scoreText.text = "Score: " + score.ToString();
    }
}
