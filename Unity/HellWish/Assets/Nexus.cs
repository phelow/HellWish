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
        m_nexusText.text = " My name is meanie the genie. Bring me my stones and I will grant you what you desire. Also I gave you telekinesis that works when you make dumb wishes.";
        m_playerText.text = "I guess I should go up to that red one and press space to drag it.";
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void GrantAWish()
    {
        m_playerText.text = "";
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("WishStones"))
        {
            Destroy(go);
        }

        PlayerController.DontSaySpace();


        StartCoroutine(GenerateNewWishes());
        m_scoreText.text = "" + wishes;

        if (wishes == 0)
        {
            StartCoroutine(FirstWish());
        }
        else
        if (wishes == 1)
        {
            m_nexusText.text = "Finally my power is unleashed and the end is near. It is me that humanity will learn to fear. My name is Meanie the Genie.";
        }
        else
        if (wishes == 2)
        {
            m_nexusText.text = "Is it my story you wish to hear. Or would you like to die in confusion instead of just fear.";
            m_playerText.text = "Please, no distractions.";
        }
        else
        if (wishes == 3)
        {
            m_playerText.text = "I should move these corpses. The zombies are using them to navigate.";
        }
        else
        if (wishes == 4)
        {
            m_nexusText.text = "My power looms you must wish to know why it also dooms.";
            m_playerText.text = "Ugh! Go away.";
        }
        else
        if (wishes == 5)
        {
            m_nexusText.text = "Well despite what you desire my story will by your ire. Lend an ear and you will find yourself disturbed by what you hear.";
        }
        else
        if (wishes == 6)
        {
            m_nexusText.text = "I was like you once, consumed by want and desire. It motivated me and wouldn't let me tire.";
        }
        else
        if (wishes == 7)
        {
            m_nexusText.text = "I shut people out in a pursuit of power. Perfecting my craft I spent every hour.";
        }
        else
        if (wishes == 8)
        {
            m_nexusText.text = "But after a tragic accident I could craft no more. And so I lay there sobbing on the floor.";
        }
        else
        if (wishes == 9)
        {
            m_nexusText.text = "I needed a way to restore my identity. Without my profession there was nothing left of me.";
        }
        else
        if (wishes == 10)
        {
            m_nexusText.text = "So I heard a rumor of a magic lamp. To find it in the mountains I made my camp.";
        }
        else
        if (wishes == 11)
        {
            m_nexusText.text = "For years I searched pouring resources and energy. I lost all my friends and made enemies.";
        }
        else
        if (wishes == 12)
        {
            m_nexusText.text = "Until finally I found the magic lamp. In a a hidden cellar wet and damp.";
        }
        else
        if (wishes == 13)
        {
            m_nexusText.text = "Out poured a demon with which I struck a bargain. I was whole again.";
        }
        else
        if (wishes == 14)
        {
            m_nexusText.text = "But little did I know ther was a hidden clause. For concern I had no cause.";
        }
        else
        if (wishes == 15)
        {
            m_nexusText.text = "After years of success and prosperity my demons had accumulated to make the most of me.";
        }
        else
        if (wishes == 16)
        {
            m_nexusText.text = "They forced me into the lamp where I remain. I wouldnt wish on anyone that pain.";
        }
        else
        if (wishes == 17)
        {
            m_nexusText.text = "Which is why my deal must mean your doom. Better to die now than have this lamp be your tomb.";
        }
        else
        if (wishes == 18)
        {
            m_nexusText.text = "And so my minions will spare you. Just after they tear you.";
        }
        else
        if (wishes == 19)
        {
            m_nexusText.text = "I wish you the best of luck I really do. I wish he treated me like I treat you.";
        }
        else
        {
            m_nexusText.text = "";
        }
        wishes++;

        PlayerPrefs.SetInt("YourScore", wishes);
    }

    private void GiveChoice()
    {
        m_nexusText.text += "\nWhat do you wish for?";
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
