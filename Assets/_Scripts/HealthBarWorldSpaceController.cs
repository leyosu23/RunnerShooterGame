using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class HealthBarWorldSpaceController : MonoBehaviour
{
    public Transform playerCamera;

   

    [Header("Health Properties")]
    [Range(0, 100)]
    public int currentHealth = 100;
    [Range(1, 100)]
    public int maximumHealth = 100;

    [Header("HealthBar")]
    public HealthBarScreenSpaceController ninjaHealthBar;

    private Slider ninjaHealthBarSlider;
    void Start()
    {
        playerCamera = GameObject.Find("PlayerCamera").transform;
        ninjaHealthBarSlider = ninjaHealthBar.GetComponent<Slider>();
       


        currentHealth = maximumHealth;
    }

    public void TakeNinjaDamage(int damage)
    {
        ninjaHealthBarSlider.value -= damage;
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            ninjaHealthBarSlider.value = 0;
            currentHealth = 0;
        }
    }

    void LateUpdate()
    {
        // billboard the healthBar
        transform.LookAt(transform.position + playerCamera.forward);
    }
}