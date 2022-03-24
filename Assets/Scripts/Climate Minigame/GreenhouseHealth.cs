using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GreenhouseHealth : MonoBehaviour
{
    [HideInInspector]
    public ClimateMinigame minigame;

    public int maxHealth;
    public int score;
    public bool flash;
    public float flashTime;
    public bool isBonus = false;

    int currentHealth;

    Image image;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        image = GetComponent<Image>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Ozone"))
        {
            currentHealth--;
            Destroy(collision.gameObject);

            if (currentHealth <= 0)
            {
                if(minigame != null)
                {
                    minigame.currentDefeatedScore += score;
                    minigame.numDefeated++;
                }

                if(isBonus)
                {
                    BonusManager.currentBonuses++;
                }

                Destroy(gameObject);
                return;
            }

            if(flash)
            {
                StartCoroutine(Flash());
            }
        }
    }

    IEnumerator Flash()
    {
        image.color = new Color(0.8f, 0.8f, 0.8f);
        yield return new WaitForSeconds(flashTime);
        image.color = Color.white;
    }
}
