using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashMinigame : IMinigame
{
    public RoadSystem roadSystem;
    public GarbageTruckController garbageTruck;
    public TrashSpawner trashSpawner;
    public TrashImpact trashImpact;
    public BuildingSystem buildingSystem;

    public UILivesDisplay livesDisplay;
    public UITimeDisplay timeDisplay;

    public TrashScore trashScore;

    public DifficultyFloat worldMinSpeed;
    public DifficultyFloat worldMaxSpeed;
    public int worldSpeedSteps;
    public float distanceBetweenCones;
    float originalSpawnTime;

    [Header("Sprites")]
    public Sprite garbagemanSprite;
    public Sprite trashSprite;
    public Sprite coneSprite;

    [Header("Settings")]
    public int maxLives;
    public DifficultyFloat minigameTime;

    int lives = 0;
    int pickedUp = 0;
    float currentTime;
    bool gotToEnd = false;
    bool isActive = false;

    public static TrashMinigame trashMinigame;

    void Awake()
    {
        trashMinigame = this;
    }

    void Start()
    {
        roadSystem.speed = worldMinSpeed.Get();
        trashSpawner.speed = worldMinSpeed.Get();
        buildingSystem.speed = worldMinSpeed.Get();

        trashSpawner.minigameTime = minigameTime.Get();

        trashScore.trashMinigame = this;
        trashImpact.minigame = this;

        originalSpawnTime = trashSpawner.spawnTime;
    }

    void Update()
    {
        if(isActive)
        {
            livesDisplay.lives = lives;

            timeDisplay.currentTime = currentTime;
            currentTime -= Time.deltaTime;
            if (currentTime < 0 && !gotToEnd)
            {
                gotToEnd = true;
                EndMinigame();
            }
        }
    }

    public override string GetName()
    {
        return "Pick Up Trash";
    }

    public override string GetDescription()
    {
        return "Pick up the trash while avoiding the cones";
    }

    public override string GetControls()
    {
        return "Left Arrow: <b>Move Left</b>\nRight Arrow: <b>Move Right</b>";
    }

    public override string GetScoring()
    {
        return "Trash Collected: +100\nLives Bonus: +40\nSurvival Bonus: +200";
    }

    public override void LoadMinigame()
    {
        roadSystem.speed = worldMinSpeed.Get();
        trashSpawner.speed = worldMinSpeed.Get();
        buildingSystem.speed = worldMinSpeed.Get();

        trashScore.gameObject.SetActive(false);
        trashImpact.gameObject.SetActive(false);

        buildingSystem.isSpawning = true;

        garbageTruck.ClearTrash();
    }

    public override void StartMinigame()
    {
        garbageTruck.isActive = true;

        lives = maxLives;
        pickedUp = 0;
        currentTime = minigameTime.Get();
        gotToEnd = false;
        isActive = true;

        trashSpawner.spawnTime = distanceBetweenCones / worldMinSpeed.Get();
        trashSpawner.StartSpawning();

        StartCoroutine(ControlSpeed());

        timeDisplay.gameObject.SetActive(true);
    }

    public override List<IntroImageConfig> GetIntroImageConfigs()
    {
        List<IntroImageConfig> configs = new List<IntroImageConfig>();
        configs.Add(new IntroImageConfig(garbagemanSprite, "Use arrow keys to move garbageman"));
        configs.Add(new IntroImageConfig(trashSprite, "Collect trash from the pathway"));
        configs.Add(new IntroImageConfig(coneSprite, "Avoid the cones"));

        return configs;
    }

    public void ObstacleHit()
    {
        lives--;
        if(lives <= 0)
        {
            EndMinigame();
        }
    }

    public void TrashPickedUp()
    {
        pickedUp++;
    }

    public int GetNumTrash()
    {
        return pickedUp;
    }

    public int GetNumLives()
    {
        return lives;
    }

    int CalculateScore()
    {
        int trashCollectedScore = trashMinigame.GetNumTrash() * 100;
        int livesBonusScore = trashMinigame.GetNumLives() * 40;
        int completionBonusScore = 200;
        if(lives == 0)
        {
            completionBonusScore = 0;
        }

        return trashCollectedScore + livesBonusScore + completionBonusScore;
    }

    void EndMinigame()
    {
        isActive = false;

        garbageTruck.isActive = false;
        trashSpawner.StopSpawning();
        trashSpawner.Reset();

        // trashScore.gameObject.SetActive(true);
        // trashScore.StartScore();

        trashImpact.gameObject.SetActive(true);
        trashImpact.DisplayImpact();

        timeDisplay.gameObject.SetActive(false);
    }

    IEnumerator ControlSpeed()
    {
        float speedStep = (worldMaxSpeed.Get() - worldMinSpeed.Get()) / worldSpeedSteps;

        for(int i = 0; i < worldSpeedSteps; i++)
        {
            yield return new WaitForSeconds(minigameTime.Get() / worldSpeedSteps);

            roadSystem.speed += speedStep;
            trashSpawner.speed += speedStep;
            buildingSystem.speed += speedStep;

            // float originalSpeedRatio = roadSystem.speed / worldMinSpeed;
            // trashSpawner.spawnTime = originalSpawnTime / originalSpeedRatio;

            trashSpawner.spawnTime = distanceBetweenCones / roadSystem.speed;
        }
    }
}
