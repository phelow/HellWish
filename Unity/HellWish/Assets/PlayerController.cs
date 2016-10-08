using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    private static PlayerController ms_instance;

    [SerializeField]
    private Rigidbody2D m_rigidbody;

    [SerializeField]
    private float m_speedModifier = 100.0f;

    [SerializeField]
    private GameObject m_spriteGameObject;

    private int[] wishes;

    // Use this for initialization
    void Start () {
        ms_instance = this;

        wishes = new int[ChoiceStone.Wish.GetNames(typeof(ChoiceStone.Wish)).Length];

        for(int i = 0; i < wishes.Length; i++)
        {
            wishes[i] = 0;
        }

    }

    public void GrantWish(ChoiceStone.Wish wish)
    {
        wishes[(int)wish] = wishes[(int)wish] + 1;
    }

    public static Rigidbody2D GetPlayerRigidBody()
    {
        return ms_instance.m_rigidbody;
    }

    public static Vector2 GetPlayerPosition()
    {
        return new Vector2(ms_instance.transform.position.x, ms_instance.transform.position.y);
    }
	
	// Update is called once per frame
	void Update () {
        m_rigidbody.AddForce(Input.GetAxis("Horizontal") * transform.right * m_speedModifier * Time.deltaTime);
        m_rigidbody.AddForce(Input.GetAxis("Vertical") * transform.up * m_speedModifier * Time.deltaTime);
    }
}
