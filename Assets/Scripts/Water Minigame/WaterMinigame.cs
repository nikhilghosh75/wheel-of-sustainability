using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterMinigame : IMinigame
{
    public RiverSpawner riverSpawner;
    public RiverCleaner riverCleaner;
    public WaterSpawner waterSpawner;
    public WaterScore waterScore;
    public WaterImpact waterImpact;
    public GrassSpawner grassSpawner;

    public UITimeDisplay timeDisplay;

    public GameObject wildlifeRemovePenalty;

    [HideInInspector]
    public int correctlyCaught;

    [HideInInspector]
    public int correctlyIgnored;

    [HideInInspector]
    public int wildlifeRemoved;

    [Header("Sprites")]
    public Sprite riverCleanerSprite;
    public Sprite trashSprite;
    public Sprite fishSprite;
    public Sprite trashCanSprite;

    [Header("Settings")]
    public DifficultyFloat worldMinSpeed;
    public DifficultyFloat worldMaxSpeed;
    public int worldSpeedSteps;
    public float distanceBetweenObjects;
    public float distanceStep;
    public DifficultyFloat minigameTime;

    float currentTime;

    bool isActive = false;

    // Start is called before the first frame update
    void Start()
    {
        riverSpawner.speed = worldMinSpeed.Get();
        waterSpawner.speed = worldMinSpeed.Get();

        waterSpawner.minigame = this;
        waterScore.waterMinigame = this;
        riverCleaner.minigame = this;
        riverSpawner.minigame = this;
        waterImpact.minigame = this;

        waterSpawner.minigameTime = minigameTime.Get();
        waterSpawner.spawnTime = distanceBetweenObjects / worldMinSpeed.Get();

        wildlifeRemovePenalty.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            currentTime -= Time.deltaTime;
            timeDisplay.currentTime = currentTime;

            if (currentTime <= 0)
            {
                isActive = false;
                EndMinigame();
            }
        }
    }

    public override string GetName()
    {
        return "Clean the River";
    }

    public override string GetDescription()
    {
        return "Pick up trash in the river while avoiding catching the fish";
    }

    public override string GetControls()
    {
        return "Left Click: <b>Pick Up</b>";
    }

    public override string GetScoring()
    {
        return "Correctly Caught: +100\nCorrectly Ignored: +100\nWildlife Removed: -100";
    }

    public override void LoadMinigame()
    {
        waterScore.gameObject.SetActive(false);
        waterImpact.gameObject.SetActive(false);

        riverSpawner.speed = worldMinSpeed.Get();
        waterSpawner.speed = worldMinSpeed.Get();

        riverCleaner.Clear();

        grassSpawner.ResetGrass();

        correctlyCaught = 0;
        correctlyIgnored = 0;
        wildlifeRemoved = 0;
    }

    public override void StartMinigame()
    {
        isActive = true;
        riverCleaner.isActive = true;

        currentTime = minigameTime.Get();

        waterSpawner.StartSpawning();
        waterSpawner.spawnTime = distanceBetweenObjects / worldMinSpeed.Get();

        StartCoroutine(ControlSpeed());

        timeDisplay.gameObject.SetActive(true);
    }

    public override List<IntroImageConfig> GetIntroImageConfigs()
    {
        List<IntroImageConfig> configs = new List<IntroImageConfig>();
        configs.Add(new IntroImageConfig(riverCleanerSprite, "Use the River Cleaner to pick up items"));
        configs.Add(new IntroImageConfig(trashSprite, "Pick up trash from the river"));
        configs.Add(new IntroImageConfig(fishSprite, "Avoid picking up fish"));
        configs.Add(new IntroImageConfig(trashCanSprite, "Put trash in the trash can"));

        return configs;
    }

    public void OnCorrectlyCaught()
    {
        correctlyCaught++;

        grassSpawner.AddGrasses(Random.Range(1, 5));
    }

    public void OnCorrectlyIgnored()
    {
        correctlyIgnored++;
    }

    public void OnWildlifeRemoved()
    {
        wildlifeRemoved++;

        wildlifeRemovePenalty.SetActive(true);
        wildlifeRemovePenalty.GetComponent<Text>().text = "Wildlife Removal Penalty: -100";
        Invoke("SetPenaltyInvisible", 1.1f);
    }

    public void OnTrashIgnored()
    {
        wildlifeRemoved++;

        wildlifeRemovePenalty.SetActive(true);
        wildlifeRemovePenalty.GetComponent<Text>().text = "Trash Ignored Penalty: -100";
        Invoke("SetPenaltyInvisible", 1.1f);
    }

    void SetPenaltyInvisible()
    {
        wildlifeRemovePenalty.SetActive(false);
    }

    void EndMinigame()
    {
        riverCleaner.isActive = false;
        riverCleaner.Clear();

        waterSpawner.StopSpawning();

        waterImpact.gameObject.SetActive(true);
        waterImpact.DisplayImpact();

        // waterScore.gameObject.SetActive(true);
        // waterScore.StartScore();

        timeDisplay.gameObject.SetActive(false);

        SetPenaltyInvisible();
    }

    IEnumerator ControlSpeed()
    {
        float speedStep = (worldMaxSpeed.Get() - worldMinSpeed.Get()) / worldSpeedSteps;

        for (int i = 0; i < worldSpeedSteps; i++)
        {
            yield return new WaitForSeconds(minigameTime.Get() / worldSpeedSteps);

            riverSpawner.speed += speedStep;
            waterSpawner.speed += speedStep;

            waterSpawner.spawnTime = (distanceBetweenObjects + i * distanceStep) / riverSpawner.speed;
        }
    }
}