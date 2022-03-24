using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashPickup : MonoBehaviour
{
    public enum PickupType
    {
        Trash,
        Obstacle,
        Bonus
    }

    public PickupType type;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        switch (type)
        {
            case PickupType.Bonus:
                BonusManager.currentBonuses++;
                break;
            case PickupType.Obstacle:
                TrashMinigame.trashMinigame.ObstacleHit();
                break;
            case PickupType.Trash:
                TrashMinigame.trashMinigame.TrashPickedUp();
                TrashMinigame.trashMinigame.garbageTruck.AddTrash();
                break;
        }

        Destroy(this.gameObject);
    }
}
