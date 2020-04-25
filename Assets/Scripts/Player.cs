using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float movementSpeed = 10.0f;
    public CharacterController controller;
    public float Health = 100.0f;
    public float sprintSpeed = 20.0f;
    public float stamina = 100.0f;

    public float currentStamina;

    public Text heathText;
    public Text staminaText;
    public GameObject weapon;

    public GameObject RedScreenObject;

    private Animator redScreen;
    private PlayDeadAnim cam;
    private PlayDeadAnim gameOverMenu;

    public AudioSource[] walkingSounds;
    public AudioSource[] hurtSounds;
    public AudioSource breathingSound;

    // Start is called before the first frame update
    void Start()
    {
        currentStamina = stamina;
        cam = GameObject.Find("Main Camera").GetComponent<PlayDeadAnim>();
        gameOverMenu = GameObject.Find("GameOver").GetComponent<PlayDeadAnim>();
        redScreen = RedScreenObject.GetComponent<Animator>();
        heathText.text = "100";
        staminaText.text = "100";
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if (Input.GetKey("w") && Input.GetKey("left shift") && currentStamina > 0)
        {
            Vector3 move = transform.right * x * 0.8f + transform.forward * z;
            controller.Move(move * sprintSpeed * Time.deltaTime);
            currentStamina -= 15 * Time.deltaTime;

            if (walkingSounds[1].isPlaying == false && walkingSounds[0].isPlaying == false) {
                walkingSounds[1].volume = Random.Range(0.05f, 0.15f);
                walkingSounds[1].pitch = Random.Range(0.8f, 1.1f);
                walkingSounds[1].Play();
            }

            // Set stamina text ui
            if (stamina >= 0 || stamina <= 100)
            {
                staminaText.text = currentStamina.ToString("0");
            }

        }
        else {
            Vector3 move = transform.right * x * 0.8f + transform.forward * z;
            controller.Move(move * movementSpeed * Time.deltaTime);

            if (currentStamina < stamina) {
                currentStamina += 5f * Time.deltaTime;

                // Set stamina text ui
                if (stamina >= 0 || stamina <= 100)
                {
                    staminaText.text = currentStamina.ToString("0");
                }
            }
        }

        if (walkingSounds[0].isPlaying == false && walkingSounds[1].isPlaying == false && (Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d")))
        {
            walkingSounds[0].volume = Random.Range(0.05f, 0.15f);
            walkingSounds[0].pitch = Random.Range(0.8f, 1.1f);
            walkingSounds[0].Play();
        }

        if (currentStamina >= 0 && currentStamina <= 10)
        {
            if (!breathingSound.isPlaying) {
                breathingSound.Play();
            }
            
        }


    }

    public void TakeDamage(float amount) {
        Health -= amount;
        if (Health <= 100)
        {
            heathText.text = Health.ToString("0");
            if (Health < 0)
            {
                heathText.text = "0";
            }
        }
        if (Health <= 0) {
            Debug.Log("Player died!");
            Die();
        }

        bool isHurtSoundPlaying = false;
        foreach (var hs in hurtSounds) {
            if (hs.isPlaying) {
                isHurtSoundPlaying = true;
            }
        }

        if (!isHurtSoundPlaying) {
            int randomSound = Random.Range(0, 5);
            hurtSounds[randomSound].Play();
        }

    }

    private void Die() 
    {
        cam.PlayDeadAnimation();
        gameOverMenu.PlayDeadAnimation();
        weapon.SetActive(false);
        RedScreenObject.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        //Stop player hurt sounds
        foreach (var hs in hurtSounds)
        {
            hs.Stop();
        }

        Destroy(this);
    }

    public void SetRedScreen() {
        redScreen.SetTrigger("hurt");
    }

    public void AddHealth() {
        Health += 50;
        if (Health > 100) {
            Health = 100;
        }
        heathText.text = Health.ToString("0");
    }

    public float GetCurrentHealth {
        get {
            return Health;
        }
    }

}
