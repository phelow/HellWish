using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
    private static PlayerController ms_instance;

    [SerializeField]
    private Rigidbody2D m_rigidbody;

    [SerializeField]
    private Text m_space;

    [SerializeField]
    private float m_speedModifier = 100.0f;

    [SerializeField]
    private GameObject m_spriteGameObject;

    [SerializeField]
    private float m_maxHealth = 100.0f;

    private float m_curHealth = 100.0f;

    public static float m_wishSpeedModifier = 1.0f;

    [SerializeField]
    private Image m_health;

    [SerializeField]
    private Camera m_camera;

    [SerializeField]
    private float m_powerTime = 10.0f;

    private float m_glassesMultiplier = 1.5f;

    private int[] wishes;

    private bool m_invincible = false;

    public void ActivateHammer()
    {
        StartCoroutine(HammerTime());
    }

    private IEnumerator HammerTime()
    {
        m_invincible = true;
        float t = 4.0f;

        while (t > 0.0f)
        {
            t -= Time.deltaTime;
            yield return new WaitForSeconds(.1f);
            m_spriteGameObject.GetComponent<SpriteRenderer>().color = Color.blue;
            yield return new WaitForSeconds(.1f);
            m_spriteGameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }


        m_invincible = false;
    }

    private static bool m_invisible = false;

    [SerializeField]
    private Sprite m_maskedPlayer;

    public static bool GetInvisible()
    {
        return m_invisible;
    }

    public void ActivateMask()
    {
        StartCoroutine(MaskRoutine());
    }

    private IEnumerator MaskRoutine()
    {
        m_invisible = true;
        Sprite tmp = this.GetComponentInChildren<SpriteRenderer>().sprite;
        this.GetComponentInChildren<SpriteRenderer>().sprite = m_maskedPlayer;

        yield return new WaitForSeconds(m_powerTime);
        this.GetComponentInChildren<SpriteRenderer>().sprite = tmp;

        m_invisible = false;
    }

    void Awake()
    {
        ms_instance = this;
    }

    // Use this for initialization
    void Start () {

        wishes = new int[ChoiceStone.Wish.GetNames(typeof(ChoiceStone.Wish)).Length];

        for(int i = 0; i < wishes.Length; i++)
        {
            wishes[i] = 0;
        }

    }

    public void GrantWish(ChoiceStone.Wish wish)
    {
        wishes[(int)wish] = wishes[(int)wish] + 1;

        m_wishSpeedModifier = wishes[(int)ChoiceStone.Wish.Power] + 1.0f;
        m_camera.orthographicSize = 2 + wishes[(int)ChoiceStone.Wish.Knowledge];

        m_maxHealth = 100.0f + PlayerController.m_wishSpeedModifier * wishes[(int)ChoiceStone.Wish.Immortality];
        m_curHealth = m_maxHealth;

        if(wish == ChoiceStone.Wish.Peace)
        {
            int i = 0;

            foreach(GameObject go in GameObject.FindGameObjectsWithTag("Zombie"))
            {
                i++;
                Destroy(go);

                if(i > 7)
                {
                    return;
                }
            }
        }

    }

    public static void SaySpace()
    {
        ms_instance.m_space.text = "space";
    }


    public static void DontSaySpace()
    {
        ms_instance.m_space.text = "";
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Zombie")
        {
            if (m_invincible == false)
            {
                m_curHealth -= Random.Range(10.0f, 90.0f);
            }
            Destroy(coll.gameObject);
        }
    }

    public static Rigidbody2D GetPlayerRigidBody()
    {
        return ms_instance.m_rigidbody;
    }

    public static Vector2 GetPlayerPosition()
    {
        return new Vector2(ms_instance.transform.position.x, ms_instance.transform.position.y);
    }
	
    public void ActivateGlasses()
    {
        StartCoroutine(GlassesRoutine());
    }

    private IEnumerator GlassesRoutine()
    {
        m_camera.orthographicSize = m_camera.orthographicSize * m_glassesMultiplier;
        yield return new WaitForSeconds(m_powerTime);
        m_camera.orthographicSize = m_camera.orthographicSize /m_glassesMultiplier;
    }

    // Update is called once per frame
    void Update () {
        m_rigidbody.AddForce(Input.GetAxis("Horizontal") * transform.right * m_speedModifier * m_wishSpeedModifier * Time.deltaTime);
        m_rigidbody.AddForce(Input.GetAxis("Vertical") * transform.up * m_speedModifier * m_wishSpeedModifier *Time.deltaTime);

        m_curHealth += Time.deltaTime;
        if(m_curHealth > m_maxHealth)
        {
            m_curHealth = m_maxHealth;
        }

        if(m_curHealth < 10.0f)
        {
            SceneManager.LoadScene("Failure");
        }

        m_health.color = Color.Lerp(Color.red, Color.clear, m_curHealth / m_maxHealth);

        float angle = (Mathf.Atan2(m_rigidbody.velocity.y, m_rigidbody.velocity.x) + 90) * Mathf.Rad2Deg;
        m_spriteGameObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        //Wishing mechanic
    }
}
