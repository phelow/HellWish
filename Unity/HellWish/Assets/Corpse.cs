using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Corpse : MonoBehaviour {
    private float m_score = 0.0f;

    [SerializeField]
    private Text m_text;

	// Use this for initialization
	void Start () {
        m_score = 1/Vector3.Distance(transform.position, PlayerController.GetPlayerPosition());

        foreach (GameObject wishStone in GameObject.FindGameObjectsWithTag("WishStones"))
        {
            m_score += 1 / Vector3.Distance(transform.position, wishStone.transform.position);
        }

        m_score *= 100.0f;

        m_text.text = "" + m_score;

    }

    public float GetScore()
    {
        return m_score;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
