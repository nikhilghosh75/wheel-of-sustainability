using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyMinigame : IMinigame
{
    [Header("Game Objects")]
    public CloudSpawner cloudSpawner;
    public SolarPanel solarPanel;
    public EnergySpawner energySpawner;
    public EnergyScore scoreManager;
    public EnergyImpact energyImpact;
    public UILivesDisplay livesDisplay;
    public UITimeDisplay timeDisplay;

    [Header("Sprite")]
    public Sprite solarPanelSprite;
    public Sprite sunSprite;
    public Sprite lightnngSprite;

    [Header("Settings")]
    public int maxLives;
    public float minigameTime;

    int lives;
    int sunsCollected;
    bool gotToEnd = false;
    bool isActive = false;
    float currentTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        scoreManager.energyMinigame = this;
        energyImpact.minigame = this;

        energySpawner.minigameTime = minigameTime;
        cloudSpawner.minigameTime = minigameTime;
    }

    void Update()
    {
        if(isActive)
        {
            livesDisplay.lives = lives;
            timeDisplay.currentTime = currentTime;

            currentTime -= Time.deltaTime;
            if(currentTime < 0)
            {
                gotToEnd = true;
                EndGame();
            }
        }
    }

    public override string GetName()
    {
        return "Collect That Solar Energy";
    }

    public override string GetDescription()
    {
        return "Move the solar panel around to capture sun rays and turn it into electricity while avoiding the lightning bolts";
    }

    public override string GetControls()
    {
        return "Mouse Left-Right: <b>Move</b>";
    }

    public override string GetScoring()
    {
        return "Collecting Suns: +50\nSurvival Bonus: +300";
    }

    public override void LoadMinigame()
    {
        scoreManager.gameObject.SetActive(false);
        energyImpact.gameObject.SetActive(false);

        cloudSpawner.ResetObjects();
        energySpawner.ResetObjects();

        lives = maxLives;
        sunsCollected = 0;
        gotToEnd = false;
    }

    public override void StartMinigame()
    {
        cloudSpawner.StartSpawning();
        solarPanel.isActive = true;
        energySpawner.StartSpawning();
        energySpawner.isPaused = false;

        isActive = true;
        currentTime = minigameTime;
        timeDisplay.gameObject.SetActive(true);
    }

    public override void PauseMinigame()
    {
        isActive = false;
        solarPanel.isActive = false;
        cloudSpawner.isPaused = true;
        energySpawner.isPaused = true;
    }

    public override void UnpauseMinigame()
    {
        isActive = true;
        solarPanel.isActive = true;
        cloudSpawner.isPaused = false;
        energySpawner.isPaused = false;
    }

    public override List<IntroImageConfig> GetIntroImageConfigs()
    {
        List<IntroImageConfig> configs = new List<IntroImageConfig>();
        configs.Add(new IntroImageConfig(solarPanelSprite, "Use mouse to move solar panel"));
        configs.Add(new IntroImageConfig(sunSprite, "Collect the suns"));
        configs.Add(new IntroImageConfig(lightnngSprite, "Avoid the lightning"));

        return configs;
    }

    public void SunCollected()
    {
        if(isActive)
        {
            sunsCollected++;
        }
    }

    public void LightningCollected()
    {
        if(isActive)
        {
            lives--;
            if (lives <= 0)
            {
                gotToEnd = false;
                EndGame();
            }
        }
    }

    public int GetSunsCollected() { return sunsCollected; }

    public int GetSunsCollectedScore() { return sunsCollected * 23; }

    public bool GotToEnd() { return gotToEnd; }

    public int GetLives() { return lives; }

    void EndGame()
    {
        int energyScore = GetSunsCollectedScore() + lives * 40;
        if (gotToEnd)
        {
            energyScore += 200;
        }
        ScoreManager.AddScore(energyScore, Category.Energy);
        OnScoreReached(energyScore);

        cloudSpawner.StopSpawning();
        solarPanel.isActive = false;
        energySpawner.StopSpawning();
        energySpawner.ResetObjects();

        // scoreManager.gameObject.SetActive(true);
        // scoreManager.StartScore();

        energyImpact.gameObject.SetActive(true);
        energyImpact.DisplayImpact();

        timeDisplay.gameObject.SetActive(false);

        isActive = false;
    }
}
