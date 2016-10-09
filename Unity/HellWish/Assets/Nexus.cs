using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Nexus : MonoBehaviour {
    private int wishes = 0;

    [SerializeField]
    private GameObject m_dog;

    [SerializeField]
    private Text m_nexusText;

    [SerializeField]
    private Text m_scoreText;

    [SerializeField]
    private Text m_playerText;

    [SerializeField]
    private GameObject[] m_wishPoints;

    [SerializeField]
    private GameObject mp_wishStone;
    
    private List<GameObject> m_wishStones;

    [SerializeField]
    private float m_minGenWishWaitTime;

    [SerializeField]
    private float m_maxGenWishWaitTime;

    [SerializeField]
    private GameObject [] m_choiceSpawnSlots;

    [SerializeField]
    private GameObject mp_choiceStone;

    // Use this for initialization
    void Start ()
    {
        m_wishPoints = GameObject.FindGameObjectsWithTag("GemLocations");
        m_wishStones = new List<GameObject>();
        m_nexusText.text = "Bring me my stones and I will grant you what you desire. Also I gave you telekinesis.";
        m_playerText.text = "I guess I should go up to that red one and press space to drag it.";
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void GrantAWish()
    {
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("WishStones"))
        {
            Destroy(go);
        }

        PlayerController.DontSaySpace();

        if (wishes == 0)
        {
            StartCoroutine(FirstWish());
        }

        StartCoroutine(GenerateNewWishes());
        wishes++;
        m_scoreText.text = "" + wishes;

        if (wishes == 3)
        {
            m_playerText.text = "I should move these corpses. The zombies are using them to navigate.";
        }
        else
        {
            m_playerText.text = "";
        }

        PlayerPrefs.SetInt("YourScore", wishes);
    }

    private void GiveChoice()
    {
        m_nexusText.text = "What do you wish for?";
        foreach(GameObject go in m_choiceSpawnSlots)
        {
            GameObject.Instantiate(mp_choiceStone, go.transform.position, go.transform.rotation, null);
        }
    }

    private IEnumerator GenerateNewWishes()
    {
        foreach (GameObject wish in m_wishStones)
        {
            if (wish == null)
            {
                continue;
            }
            else
            {
                Destroy(wish);
            }
        }

        m_wishStones = new List<GameObject>();
        yield return new WaitForSeconds(Random.Range(m_minGenWishWaitTime, m_maxGenWishWaitTime));

        HashSet<GameObject> spawnPoints = new HashSet<GameObject>();
        for(int i = 0; i <= this.m_wishPoints.Length/3; i++)
        {
            GameObject next = null;
            while (next == null) {
                next = this.m_wishPoints[Random.Range(0, this.m_wishPoints.Length)];

                if (spawnPoints.Contains(next))
                {
                    next = null;
                }
            }

            spawnPoints.Add(next);
        }

        foreach(GameObject sp in spawnPoints)
        {
            GameObject.Instantiate(mp_wishStone, sp.transform.position, sp.transform.rotation, null);
        }

        foreach(GameObject sp in m_choiceSpawnSlots)
        {
            GameObject.Instantiate(mp_choiceStone, sp.transform.position, sp.transform.rotation, null);

        }

    }

    private IEnumerator FirstWish()
    {
        m_playerText.text = "I wish for my dead dog back";
        yield return new WaitForSeconds(3.0f);
        m_nexusText.text = "I'll do you one better. I'll bring everyone back.";
        m_playerText.text = "";
        yield return new WaitForSeconds(3.0f);
        m_nexusText.text = "What do you wish for?";

    }
}
