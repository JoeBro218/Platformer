using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;
    public float speed;
    public Text score;
    private int scoreValue = 0;
    public Text win;
    public int lives = 3;
    public Text livesText;
    public AudioClip musicClip;
    public AudioClip winClip;
    public AudioSource musicSource;
    Animator anim;
    private bool facingRight = true;

    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        score.text = scoreValue.ToString();
        win.text = "";
        livesText.text = "Lives: O O O";
        musicSource.clip = musicClip;
        musicSource.Play();
        musicSource.loop = true;
        anim = GetComponent<Animator>();
    }

    void Update()
    {  
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            anim.SetInteger("State", 1);
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            anim.SetInteger("State", 0);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            anim.SetInteger("State", 1);
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            anim.SetInteger("State", 0);
        }
    }

    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float verMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, verMovement * speed));

        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = scoreValue.ToString();
            Destroy(collision.collider.gameObject);
            if (scoreValue == 4)
            {
                transform.position = new Vector3(48.5f, 1.0f, 0.0f);
                lives = 3;
                livesText.text = "Lives: O O O";
            }
            if (scoreValue == 8)
            {
                musicSource.loop = false;
                musicSource.Stop();
                musicSource.clip = winClip;
                musicSource.Play();
                win.text ="You Win! Game made by Joseph Demeritt";
            }
        }
        if (collision.collider.tag == "Enemy")
        {
            lives -= 1;
            Destroy(collision.collider.gameObject);
            if(lives == 2)
            {
                livesText.text = "Lives: O O X";
            }
            if(lives == 1)
            {
                livesText.text = "Lives: O X X";
            }
            if(lives == 0)
            {
                livesText.text = "Lives: X X X";
                Destroy(this);
                win.text = "You lose, please try again.";
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            if (Input.GetKey(KeyCode.W))
            {
                anim.SetInteger("State", 2);
                rd2d.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
            }
        }
    }
    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }
}