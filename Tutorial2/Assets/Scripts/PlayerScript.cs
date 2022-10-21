using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;

    public float speed;
    
    public Text score;
    public TextMeshProUGUI livesText;
    public GameObject winText;
    public GameObject loseText;
    public GameObject player;
    public Transform level2;
    public AudioClip victory;
    public AudioClip backgroundMusic;
    public AudioSource musicSource;
    public Animator anim;

    private int scoreValue = 0;
    private int lives = 0;
    private bool facingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        score.text = scoreValue.ToString();
        winText.SetActive(false);
        loseText.SetActive(false);
        lives = 3;
        musicSource.clip = backgroundMusic;
        musicSource.volume = 0.05f;
        musicSource.loop = true;
        musicSource.Play();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));
        livesText.text = "Lives: " + lives.ToString();
        if(lives == 0){
            loseText.SetActive(true);
            speed = 0;
        }
        anim.SetFloat("HorizontalValue", Mathf.Abs(Input.GetAxis("Horizontal")));
        anim.SetFloat("VerticalValue", Mathf.Abs(Input.GetAxis("Vertical")));
        if (facingRight == false && hozMovement > 0){
            Flip();
        }
        else if (facingRight == true && hozMovement < 0){
            Flip();
        }
    }
    void Flip(){
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = scoreValue.ToString();
            Destroy(collision.collider.gameObject);
            if(scoreValue == 4){
                lives = 3;
                player.transform.position = level2.transform.position;
            }            
            if(scoreValue == 8){
                winText.SetActive(true);
                speed = 0;
                musicSource.loop = false;
                musicSource.clip = victory;
                musicSource.Play();
            }
        }
       if (collision.collider.tag == "Enemy"){
            lives -= 1;
            Destroy(collision.collider.gameObject);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse); //the 3 in this line of code is the player's "jumpforce," and you change that number to get different jump behaviors.  You can also create a public variable for it and then edit it in the inspector.
            }
        }
    }
}