using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
    private static PlayerController ms_instance;

    [SerializeField]
    private Rigidbody2D m_rigidbody;

    [SerializeField]
    private GameObject m_zombie;

    [SerializeField]
    private AudioSource m_audioSource;

    [SerializeField]
    private AudioClip []m_blips;

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

    [SerializeField]
    private Text m_dumbWish;

    private bool m_invincible = false;

    public void ActivateHammer()
    {
        StartCoroutine(HammerTime());
    }

    private IEnumerator HammerTime()
    {
        m_invincible = true;
        for (int i = 0; i< 20; i++) { 
            yield return new WaitForSeconds(.1f);
            m_spriteGameObject.GetComponent<SpriteRenderer>().color = Color.blue;
            yield return new WaitForSeconds(.1f);
            m_spriteGameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }


        m_invincible = false;
    }

    public static void PlayBlip()
    {
        ms_instance.m_audioSource.PlayOneShot(ms_instance.m_blips[Random.Range(0, ms_instance.m_blips.Length)]);
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
        PlayBlip();

        if (coll.gameObject.tag == "Zombie")
        {
            PlayBlip();
            if (m_invincible == false)
            {
                m_curHealth -= Random.Range(10.0f, 90.0f);
            }
            coll.gameObject.GetComponent<ZombieAI>().DestroyZombie();
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
        PlayBlip();
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayBlip();
            StartCoroutine(MakeDumbWish());
        }
    }

    private IEnumerator JackedRoutine()
    {
        this.gameObject.transform.localScale = Vector3.one * 3;

        yield return new WaitForSeconds(10.0f);

        this.gameObject.transform.localScale = Vector3.one;
    }

    private IEnumerator BlueRoutine()
    {
        this.GetComponentInChildren<SpriteRenderer>().color = Color.blue;

        yield return new WaitForSeconds(10.0f);

        this.GetComponentInChildren<SpriteRenderer>().color = Color.white;
    }



    private IEnumerator RedRoutine()
    {
        this.GetComponentInChildren<SpriteRenderer>().color = Color.red;

        yield return new WaitForSeconds(10.0f);

        this.GetComponentInChildren<SpriteRenderer>().color = Color.white;
    }
    

    private IEnumerator CamShrinkRoutine()
    {
        this.m_camera.orthographicSize *= .5f;

        yield return new WaitForSeconds(10.0f);
        this.m_camera.orthographicSize *= 2.0f;
    }

    private IEnumerator SlowDownRoutine()
    {
        float d = m_rigidbody.drag;

        m_rigidbody.drag *= 3.0f;
        yield return new WaitForSeconds(10.0f);



        m_rigidbody.drag = d;
    }

    private IEnumerator MakeDumbWish()
    {

        int choice = Random.Range(0, 11);

        switch (choice){
            case 0:
                this.m_dumbWish.text = "I wish I was jacked.";
                StartCoroutine(JackedRoutine());
                break;

            case 1:
                this.m_dumbWish.text = "I wish I was blue.";
                StartCoroutine(BlueRoutine());
                break;


            case 2:
                this.m_dumbWish.text = "I wish I was red.";
                StartCoroutine(RedRoutine());
                break;


            case 3:
                this.m_dumbWish.text = "I wish I had a chocolate snake.";
                break;


            case 4:
                this.m_dumbWish.text = "I wish I had some friends.";
                for(int i = 0; i < Random.Range(0, 2); i++)
                {

                    GameObject go = (GameObject.Instantiate(m_zombie, new Vector3(transform.position.x + Random.Range(-10.0f,10.0f), transform.position.y + Random.Range(-10.0f, 10.0f), transform.position.z), transform.rotation, null) as GameObject);
                    go.GetComponent<ZombieAI>().InitZombie(30.0f);
                    
                }
                break;

            case 5:
                this.m_dumbWish.text = "I wish for world peace.";
                break;

               
            case 6:
                this.m_dumbWish.text = "I wish I didn't have to vote.";
                break;



            case 7:
                this.m_dumbWish.text = "I wish chocolate was illegal.";
                break;



            case 8:
                this.m_dumbWish.text = "I wish I could take it easy.";
                StartCoroutine(SlowDownRoutine());
                break;



            case 9:
                this.m_dumbWish.text = "I wish I could relax.";
                StartCoroutine(CamShrinkRoutine());
                break;
            case 10:
                this.m_dumbWish.text = "I wish I was less healthy.";
                this.m_curHealth -= 30.0f;
                break;

        }

        yield return new WaitForSeconds(4.0f);

        this.m_dumbWish.text = "";

    }
}
