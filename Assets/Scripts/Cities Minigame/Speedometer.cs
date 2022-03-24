using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speedometer : MonoBehaviour
{
    public CitiesMinigame minigame;
    public float zeroRotation;
    public float maxRotation;
    public float maxSpeed;

    RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rotation = Vector3.zero;
        rotation.z = Mathf.Lerp(zeroRotation, maxRotation, minigame.GetCurrentSpeed() / maxSpeed);
        rectTransform.rotation = Quaternion.Euler(rotation);
    }
}
