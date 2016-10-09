using UnityEngine;
using System.Collections;

public class ZombieGrowling : MonoBehaviour {
    private static AudioClip[] ms_groans;

    [SerializeField]
    private AudioSource m_audioSource;
	// Use this for initialization
	void Start () {
        if(ms_groans == null)
        {
            ms_groans = Resources.LoadAll<AudioClip>("Sounds");
        }

        StartCoroutine(GroanRandomly());
	}

    private IEnumerator GroanRandomly()
    {
        while (true)
        {
            m_audioSource.PlayOneShot(ms_groans[Random.Range(0, ms_groans.Length)]);
            yield return new WaitForSeconds(Random.Range(6.0f, 20.0f));
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
