using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OzoneLayer : MonoBehaviour
{
    public float launchPointY;

    public float speed;

    public Transform projectileParent;

    public GameObject projectile;

    public float projectileTime;

    float timeUntilFire = -0.01f;

    RectTransform rectTransform;

    [HideInInspector]
    public bool isActive = false;

    [HideInInspector]
    public ClimateMinigame minigame;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isActive)
        {
            return;
        }

        float width = rectTransform.GetWidth() / 2;

        if(Input.GetKey(KeyCode.LeftArrow))
        {
            rectTransform.anchoredPosition = new Vector2(
                Mathf.Clamp(rectTransform.anchoredPosition.x - speed * Time.deltaTime, width, 
                (float)Screen.width - width), rectTransform.anchoredPosition.y);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            rectTransform.anchoredPosition = new Vector2(
                Mathf.Clamp(rectTransform.anchoredPosition.x + speed * Time.deltaTime, width,
                (float)Screen.width - width), rectTransform.anchoredPosition.y);
        }

        timeUntilFire -= Time.deltaTime;
        if(timeUntilFire < 0 && Input.GetKey(KeyCode.Space))
        {
            Fire();
        }
    }

    void Fire()
    {
        GameObject spawnedProjectile = Instantiate(projectile, projectileParent);
        RectTransform spawnedTransform = spawnedProjectile.GetComponent<RectTransform>();
        spawnedTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, launchPointY);
        timeUntilFire = projectileTime;

        minigame.OnShoot();
    }
}
