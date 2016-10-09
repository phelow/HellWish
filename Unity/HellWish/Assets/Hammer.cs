using UnityEngine;
using System.Collections;

public class Hammer : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {

        StartCoroutine(DestroyLater());
    }

    private IEnumerator DestroyLater()
    {
        yield return new WaitForSeconds(Random.Range(100.0f, 500.0f));
        Destroy(this.gameObject);
    }
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            coll.gameObject.GetComponent<PlayerController>().ActivateHammer();

            foreach (GameObject go in GameObject.FindGameObjectsWithTag("ChoiceStones"))
            {
                Destroy(go);
            }
            Destroy(gameObject);
        }
    }
}
