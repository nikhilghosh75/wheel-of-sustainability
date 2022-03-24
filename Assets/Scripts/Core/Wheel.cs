using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Wheel : MonoBehaviour
{
    public int numCategories;

    float wheelSpeed;
    public float wheelDrag;

    public UnityEvent OnWheelStop;

    public bool locked = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spin(float speed)
    {
        if (locked)
            return;

        wheelSpeed = speed;
        StartCoroutine(DoSpin());
    }

    public int GetCurrentlySelectedCategory()
    {
        Quaternion quat = transform.rotation;
        Vector3 euler = quat.eulerAngles;
        return (int)(euler.z / (360.0f / numCategories));
    }

    IEnumerator DoSpin()
    {
        locked = true;
        float currentWheelSpeed = wheelSpeed;
        while(currentWheelSpeed > 0.01f)
        {
            currentWheelSpeed -= wheelDrag;
            Quaternion quat = transform.rotation;
            Vector3 euler = quat.eulerAngles;
            euler.z += currentWheelSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(euler);
            yield return null;
        }

        OnWheelStop.Invoke();
    }
}
