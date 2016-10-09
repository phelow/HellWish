using UnityEngine;
using System.Collections;

public class Explosi : MonoBehaviour {
    [SerializeField]
    private SpriteRenderer m_sprite;
	// Use this for initialization
	void Start () {
        StartCoroutine(Explode());
	}
	
    private IEnumerator Explode()
    {
        float t = 0.0f;
        while(t < 4.0f)
        {
            t += Time.deltaTime;
            this.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 5.0f,t/4.0f);

            m_sprite.color = Color.Lerp(Color.white, Color.clear, t / 4.0f);
            yield return new WaitForEndOfFrame();
        }

        Destroy(this.gameObject);
    }

	// Update is called once per frame
	void Update () {
	
	}
}
