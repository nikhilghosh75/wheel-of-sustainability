using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitiesMinigame : IMinigame
{
    public RoadSystem roadSystem;
    public Ambulance ambulance;
    public TrafficSystem trafficSystem;
    public Hospital hospital;
    public UITimeDisplay timeDisplay;
    public CitiesProgress progress;
    public CitiesImpact impact;
    public CitiesScore score;
    public BuildingSystem building;

    [Header("Sprites")]
    public Sprite carSprite;
    public Sprite taxiSprite;
    public Sprite officeSprite;

    [Header("Settings")]
    public float regularSpeed;
    public float slowSpeed;
    public float boostSpeed;
    public float acceleration;
    public DifficultyFloat distanceToHospital;
    public int minConstantMPG;
    public int maxConstantMPG;

    float currentSpeed;
    float distance;
    float currentTime;
    float totalMPG;
    int numFrames;
    bool isAccelerating;

    bool isActive = false;

    // Start is called before the first frame update
    void Start()
    {
        roadSystem.speed = 0;
        currentSpeed = 0;

        ambulance.citiesMinigame = this;
        trafficSystem.minigame = this;
        progress.minigame = this;
        score.citiesMinigame = this;
        hospital.minigame = this;
        impact.minigame = this;

        trafficSystem.minigameTime = 45;

        distance = 0;
    }

    void Update()
    {
        if (!isActive)
            return;

        UpdateSpeed();
        currentTime += Time.deltaTime;
        distance += currentSpeed * Time.deltaTime;
        UpdateUI();

        if(distance > distanceToHospital.Get())
        {
            EndGame();
        }

        if (distance / distanceToHospital.Get() > 0.95)
            building.isSpawning = false;
    }

    void UpdateSpeed()
    {
        isAccelerating = true;
        if (Input.GetKey(KeyCode.Space))
        {
            if (currentSpeed >= boostSpeed)
            {
                isAccelerating = false;
                currentSpeed = boostSpeed;
            }
            else
                currentSpeed += acceleration;
        }
        else if (Input.GetKey(KeyCode.F))
        {
            if (currentSpeed <= slowSpeed)
            {
                isAccelerating = false;
                currentSpeed = slowSpeed;
            }
            else
                currentSpeed -= acceleration;
        }
        else
        {
            if (currentSpeed > regularSpeed)
                currentSpeed = Mathf.Max(regularSpeed, currentSpeed - acceleration);
            else if (currentSpeed < regularSpeed)
                currentSpeed = Mathf.Min(regularSpeed, currentSpeed + acceleration);

            if (Mathf.Approximately(currentSpeed, regularSpeed))
                isAccelerating = false;
        }

        // Debug.Log(currentSpeed);
        roadSystem.speed = currentSpeed;
        building.speed = currentSpeed;
    }

    void UpdateUI()
    {
        timeDisplay.currentTime = currentTime;
        progress.DisplayProgress(distance);
    }

    public override string GetName()
    {
        return "Get to the Office";
    }

    public override string GetDescription()
    {
        return "Maneuver the sports car through vehicles as you get to the office as fast as possible";
    }

    public override string GetControls()
    {
        return "Left Arrow: <b>Move Left</b>\nRight Arrow: <b>Move Right</b>\nSpace: <b>Boost</b>\nF: <b>Brake</b>";
    }

    public override string GetScoring()
    {
        return "Distance Driven: +10 per %\nHospital Bonus: +150\nTime Bonus: ?";
    }

    public override List<IntroImageConfig> GetIntroImageConfigs()
    {
        List<IntroImageConfig> configs = new List<IntroImageConfig>();
        configs.Add(new IntroImageConfig(carSprite, "Maneuver the car"));
        configs.Add(new IntroImageConfig(taxiSprite, "Dodge the other cars"));
        configs.Add(new IntroImageConfig(officeSprite, "Get to the Office"));

        return configs;
    }

    public override void LoadMinigame()
    {
        building.isSpawning = true;

        hospital.Reset();

        distance = 0;
        currentTime = 0;

        UpdateUI();

        score.gameObject.SetActive(false);
        impact.gameObject.SetActive(false);

        progress.minigame = this;
        progress.DisplayProgress(0);

        trafficSystem.ResetTraffic();
    }

    public override void StartMinigame()
    {
        isActive = true;
        ambulance.isActive = true;

        building.isSpawning = true;

        trafficSystem.StartSpawning();

        totalMPG = 0;
        numFrames = 0;
    }

    public void AddMPG(int mpg)
    {
        totalMPG += mpg;
        numFrames++;
    }

    public float GetCurrentSpeed() { return currentSpeed; }

    public void EndGame()
    {
        if(!isActive)
        {
            return;
        }

        isActive = false;
        ambulance.isActive = false;

        building.isSpawning = false;

        currentSpeed = 0;
        roadSystem.speed = 0;
        building.speed = 0;

        trafficSystem.StopSpawning();

        impact.gameObject.SetActive(true);
        impact.DisplayImpact();
    }

    public float GetCurrentDistance() { return distance; }
    public float GetCurrentTime() { return currentTime; }
    
    public bool GetIsAccelerating() { return isAccelerating; }

    public float GetTotalMPG() { return totalMPG; }

    public float GetAverageMPG() { return totalMPG / numFrames; }

    public int GetEfficiencyScore()
    {
        float average = totalMPG / numFrames;
        float t = (average - minConstantMPG) / (maxConstantMPG - minConstantMPG);
        return (int)Mathf.Lerp(0, 200, t);
    }
}
