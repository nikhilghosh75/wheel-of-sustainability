using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConservationBonus : MonoBehaviour
{
    public float upYPosition;
    public float downYPosition;
    public List<float> xPositions;

    public float speed;
    float velocity;

    RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        float xPosition = xPositions[Random.Range(0, xPositions.Count)];

        if (Random.Range(0, 2) == 0)
        {
            velocity = speed;
            rectTransform.anchoredPosition = new Vector2(xPosition, downYPosition);
        }
        else
        {
            velocity -= speed;
            rectTransform.anchoredPosition = new Vector2(xPosition, upYPosition);
        }

        float time = Mathf.Abs(upYPosition - downYPosition / speed);
        Invoke("DoDestroy", time);
    }

    // Update is called once per frame
    void Update()
    {
        rectTransform.anchoredPosition += new Vector2(0, velocity * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            BonusManager.currentBonuses++;
            DoDestroy();
        }
    }

    void DoDestroy()
    {
        Destroy(gameObject);
    }
}
