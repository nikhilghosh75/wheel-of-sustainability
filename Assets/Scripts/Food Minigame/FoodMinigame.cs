using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodMinigame : IMinigame
{
    public ConveyorBelt conveyorBelt;
    public FoodSpawner foodSpawner;
    public FoodOrganizer foodOrganizer;
    public Stamp stamp;

    public FoodScore foodScore;
    public FoodImpact foodImpact;

    public GameObject checkBox;
    public GameObject wrongBox;
    public UITimeDisplay timeDisplay;

    [HideInInspector]
    public int correctLabeled;
    [HideInInspector]
    public int correctIgnored;

    [Header("Sprites")]
    public Sprite stampSprite;
    public Sprite processedSprite;
    public Sprite unprocessedSprite;

    [Header("Settings")]
    public DifficultyFloat beltMinSpeed;
    public DifficultyFloat beltMaxSpeed;
    public float maxFoodSpawnTime;
    public int beltSpeedSteps;
    public DifficultyFloat minigameTime;

    float currentTime;
    bool isActive = false;

    void Start()
    {
        conveyorBelt.speed = beltMinSpeed.Get();

        foodSpawner.foodSpeed = beltMinSpeed.Get();
        foodSpawner.minigame = this;

        foodScore.foodMinigame = this;
        foodImpact.minigame = this;

        stamp.isActive = true;
    }

    void Update()
    {
        if(isActive)
        {
            currentTime -= Time.deltaTime;
            timeDisplay.currentTime = currentTime;
            if (currentTime < 0)
            {
                StopMinigame();
            }
        }
    }

    public override string GetName()
    {
        return "Label Things Processed";
    }

    public override string GetDescription()
    {
        return "Label the processed foods as processed while not labelling the natural foods";
    }

    public override string GetControls()
    {
        return "Left-Click: <b>Stamp</b>";
    }

    public override string GetScoring()
    {
        return "Correctly Labeled: +100\nCorrectly Ignored: +100";
    }

    public override void LoadMinigame()
    {
        foodScore.gameObject.SetActive(false);
        foodImpact.gameObject.SetActive(false);

        checkBox.SetActive(false);
        wrongBox.SetActive(false);

        conveyorBelt.speed = beltMinSpeed.Get();
        foodSpawner.foodSpeed = beltMinSpeed.Get();
        foodSpawner.foodSpawnTime = maxFoodSpawnTime;

        foodOrganizer.text.text = "";
    }

    public override void StartMinigame()
    {
        stamp.isActive = true;
        foodSpawner.StartSpawning();

        correctLabeled = 0;
        correctIgnored = 0;

        currentTime = minigameTime.Get();
        timeDisplay.transform.parent.gameObject.SetActive(true);

        StartCoroutine(ControlSpeed());

        isActive = true;
    }

    public override List<IntroImageConfig> GetIntroImageConfigs()
    {
        List<IntroImageConfig> configs = new List<IntroImageConfig>();
        configs.Add(new IntroImageConfig(stampSprite, "Click to stamp food items"));
        configs.Add(new IntroImageConfig(processedSprite, "Label processed foods"));
        configs.Add(new IntroImageConfig(unprocessedSprite, "Avoid labelling unprocessed foods"));

        return configs;
    }

    public void OnLabeledCorrectly()
    {
        correctLabeled++;
        checkBox.SetActive(true);
        wrongBox.SetActive(false);
        // Invoke("SetBoxesInvisible", boxTime);
    }

    public void OnIgnoredCorrectly()
    {
        correctIgnored++;
        checkBox.SetActive(true);
        wrongBox.SetActive(false);
        // Invoke("SetBoxesInvisible", boxTime);
    }

    public void OnDecidedWrongly()
    {
        wrongBox.SetActive(true);
        checkBox.SetActive(false);
        // Invoke("SetBoxesInvisible", boxTime);
    }

    void SetBoxesInvisible()
    {
        checkBox.SetActive(false);
        wrongBox.SetActive(false);
    }

    void StopMinigame()
    {
        foodSpawner.StopSpawning();

        isActive = false;
        stamp.isActive = false;

        // foodScore.gameObject.SetActive(true);
        // foodScore.StartScore();

        foodImpact.gameObject.SetActive(true);
        foodImpact.DisplayImpact();

        timeDisplay.transform.parent.gameObject.SetActive(false);

        SetBoxesInvisible();
    }

    IEnumerator ControlSpeed()
    {
        float speedStep = (beltMaxSpeed.Get() - beltMinSpeed.Get()) / beltSpeedSteps;

        for(int i = 0; i < beltSpeedSteps; i++)
        {
            yield return new WaitForSeconds(minigameTime.Get() / beltSpeedSteps);

            conveyorBelt.speed += speedStep;
            foodSpawner.foodSpeed += speedStep;

            float originalSpeedRatio = conveyorBelt.speed / beltMinSpeed.Get();
            foodSpawner.foodSpawnTime = maxFoodSpawnTime / originalSpeedRatio;
        }
    }
}
