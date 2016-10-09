using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SetText : MonoBehaviour {

    [SerializeField]
    private Text m_text;

	// Use this for initialization
	void Start () {
        int yourScore = PlayerPrefs.GetInt("YourScore", 0);
        int highScore = PlayerPrefs.GetInt("HighScore",0);

        if(yourScore > highScore)
        {
            PlayerPrefs.SetInt("HighScore", yourScore);
        }

        m_text.text = "You deaded, high score:" + highScore + " your score:" + yourScore + " space to try again";

	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("MainScene");
        }
	}
}
