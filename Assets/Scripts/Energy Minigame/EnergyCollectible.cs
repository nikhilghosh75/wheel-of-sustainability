using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyCollectible : MonoBehaviour
{
    public enum CollectableType
    {
        Sun,
        Lightning,
        Bonus
    }

    // public float speed = 40;
    public DifficultyFloat speed = new DifficultyFloat();
    public CollectableType type;

    RectTransform rectTransform;
    Collider2D panelCollider;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        panelCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 position = rectTransform.anchoredPosition;
        position.y -= speed.Get() * Time.deltaTime;
        rectTransform.anchoredPosition = position;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        EnergyMinigame minigame = (EnergyMinigame)(GetComponent<MinigameTracker>().minigame);
        if(collider.CompareTag("Solar Panel"))
        {
            switch (type)
            {
            case CollectableType.Sun:
                minigame.SunCollected();
                break;
            case CollectableType.Lightning:
                minigame.LightningCollected();
                break;
            case CollectableType.Bonus:
                BonusManager.currentBonuses++;
                break;
            }
            Destroy(this.gameObject);
        }
    }
}
