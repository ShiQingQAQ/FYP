using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI AiScoreText;
    public TextMeshProUGUI PlayerScoreText;
    public GameObject MainGame;
    void Start()
    {
        
    }
    void Update()
    {
        if (Convert.ToInt32(PlayerScoreText.text)  >= 3)
        {
            SceneManager.LoadScene("Vectory");
        }
        if (Convert.ToInt32(AiScoreText.text) >= 3)
        {
            SceneManager.LoadScene("Lose");
        }
    }   
}
