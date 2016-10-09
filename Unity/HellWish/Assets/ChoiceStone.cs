using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChoiceStone : MonoBehaviour {

    public enum Wish
    {
        Knowledge,
        Power,
        Immortality,
        Peace
    }

    [SerializeField]
    private Wish m_thisWish;

    [SerializeField]
    private Text m_text;
    // Use this for initialization
    void Start () {
        System.Array values = System.Enum.GetValues(typeof(Wish));
        m_thisWish = (Wish)values.GetValue(Random.Range(0,values.Length));

        m_text.text = m_thisWish.ToString() + "?";
	}
	
	// Update is called once per frame
	void Update () {

    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            coll.gameObject.GetComponent<PlayerController>().GrantWish(m_thisWish);

            foreach(GameObject go in GameObject.FindGameObjectsWithTag("ChoiceStones"))
            {
                Destroy(go);
            }
        }
    }
}
