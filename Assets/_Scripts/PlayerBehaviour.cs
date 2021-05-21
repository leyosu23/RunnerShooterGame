using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBehaviour : MonoBehaviour
{
    public CharacterController controller;

    [Header("Controls")]
    public Joystick joystick;
    public float horizontalSensitivity;
    public float verticalSensitivity;

    public float maxSpeed = 10.0f;
    public float gravity = -30.0f;
    public float jumpHeight = 3.0f;

    public Transform groundCheck;
    public float groundRadius = 0.5f;
    public LayerMask groundMask;

    public Vector3 velocity;
    public bool isGrounded;
    public bool gameIsPaused;
    public bool jumpBoost = false;


    public GameObject gameOverPanel;
    public GameObject youWonPanel;
    public GameObject NinjaStarting;
    public GameObject numberOfDepeated;
    public GameObject itemsUsed;
    public GameObject Achievement1;
    public GameObject Achievement2;
    

    private int enemyDefeated = 0;
    private int numOfItemUsed = 0;

    [Header("HealthBar")]
    public HealthBarScreenSpaceController healthBar;


    [Header("HealthBar")]
    public HealthBarWorldSpaceController ninjaHealthBar;

    public NinjaBehaviour ninja;

    [Header("Player Abilities")]
    [Range(0, 100)]
    public int health = 100;

    [Header("MiniMap")]
    public GameObject miniMap;

    public GameObject inventory1Button;
    public GameObject inventory2Button;
    public GameObject inventory3Button;
    public GameObject AttackButton;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        ninja = FindObjectOfType<NinjaBehaviour>();
    }

    // Update is called once per frame - once every 16.6666ms

    void Update()
    {
        #region player Control
        isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, groundMask);

        if (!jumpBoost)
        {
            jumpHeight = 3.0f;
        }

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2.0f;
        }

        if (isGrounded && controller.velocity.magnitude > 0)
        {
            // FindObjectOfType<AudioManager>().Play("Footstep");
        }

        float x = joystick.Horizontal;
        float z = joystick.Vertical;

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * maxSpeed * Time.deltaTime);

        if (Input.GetButton("Jump")) // && isGrounded
        {

            velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
            FindObjectOfType<AudioManager>().Play("Footstep");

        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        #endregion


        #region StopResume
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Main");
            gameOverPanel.SetActive(false);
            FindObjectOfType<GameManager>().Restart();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            if (gameIsPaused)
            {
                gameIsPaused = !gameIsPaused;
                PauseGame();
            }
            else if (!gameIsPaused)
            {
                gameIsPaused = !gameIsPaused;
                ResumeGame();
            }
        }
        #endregion

        // if player lost
        if (health <= 0)
        {
            FindObjectOfType<GameManager>().EndGame();
            gameOverPanel.SetActive(true);
        }

        // if player won
        if (ninjaHealthBar.currentHealth <= 0)
        {
            FindObjectOfType<GameManager>().Win();
            youWonPanel.SetActive(true);
            if (enemyDefeated < 1)
            {
                increaseNumOfDepeated();
            }
        }

    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
    public void increaseNumOfDepeated()
    {
        enemyDefeated++;
        numberOfDepeated.GetComponent<UnityEngine.UI.Text>().text = $"Enemy Defeated: {enemyDefeated}/1";
        if (numOfItemUsed >= 1)
        {
            Achievement1.SetActive(true);
        }
    }

    public void increaseitemsUsed()
    {
        numOfItemUsed++;
        itemsUsed.GetComponent<UnityEngine.UI.Text>().text = $"Items used: {numOfItemUsed}/3";
        if (numOfItemUsed >= 3)
        {
            Achievement2.SetActive(true);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthBar.TakeDamage(damage);
        if (health < 0)
        {
            health = 0;
        }
    }


    void PauseGame()
    {
        Time.timeScale = 0;
    }

    void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void restartGame()
    {
        FindObjectOfType<GameManager>().Restart();
        gameOverPanel.SetActive(false);
        youWonPanel.SetActive(false);
    }

    public void OnJumpButtonPressed()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
        FindObjectOfType<AudioManager>().Play("Footstep");
        jumpBoost = false;
    }

    public void OnMapButtonPressed()
    {
        miniMap.SetActive(!miniMap.activeInHierarchy);
    }

    public void OnPotionPressed()
    {
        increaseitemsUsed();
        health += 50;
        if (health> 100)
        {
            health = 100;
        }
        healthBar.Healing(health);
        inventory1Button.SetActive(false);
    }

    public void OnPotion2Pressed()
    {
        increaseitemsUsed();
        ninja.HasLOS = false;
        ninja.agent.SetDestination(NinjaStarting.transform.position);
        inventory2Button.SetActive(false);

    }

    public void OnPotion3Pressed()
    {
        increaseitemsUsed();
        jumpBoost = true;
      
        jumpHeight = 30.0f;
        inventory3Button.SetActive(false);
    }
    public void OnAttackPressed()
    {
        ninjaHealthBar.TakeNinjaDamage(20);
        /*
        ninja.ninjaHealth -= 20;
        ninjaHealthBar.currentHealth -= 20;
        */
    }

}
