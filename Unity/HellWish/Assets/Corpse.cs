using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Corpse : DraggableObject {
    private float m_score = 0.0f;

    [SerializeField]
    private Text m_text;

	// Use this for initialization
	void Start ()
    {
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();

        float dragDistance = this.gameObject.GetComponent<BoxCollider2D>().size.magnitude * 1.7f;

        CircleCollider2D cd = this.gameObject.AddComponent<CircleCollider2D>();

        cd.radius = dragDistance;
        cd.isTrigger = true;

        StartCoroutine(CalculateScore());

        StartCoroutine(DestroyAfterTime());

    }

    private IEnumerator CalculateScore()
    {
        while (true)
        {
            yield return new WaitForSeconds(10.0f);
            m_score = 100 / Vector3.Distance(transform.position, PlayerController.GetPlayerPosition());

            foreach (GameObject wishStone in GameObject.FindGameObjectsWithTag("WishStones"))
            {
                m_score += 1 / Vector3.Distance(transform.position, wishStone.transform.position);
            }

            m_score *= 100.0f;

            m_text.text = "" + Mathf.RoundToInt(m_score);
        }
    }

    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(1000.0f);
        Destroy(this.gameObject);
    }

    public float GetScore()
    {
        return m_score;
    }
	
}
