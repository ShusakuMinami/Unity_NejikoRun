using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleController : MonoBehaviour
{
    public Text highScoreLabel;
    
    public void OnStartButtonClicked()
    {
        Application.LoadLevel("Main");
    }
    
    // Start is called before the first frame update
    void Start()
    {
        // ハイスコアを表示
        highScoreLabel.text = "High Score : " + PlayerPrefs.GetInt("HighScore") + "m";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
