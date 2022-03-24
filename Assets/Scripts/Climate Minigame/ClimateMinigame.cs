using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClimateMinigame : IMinigame
{
    public OzoneLayer ozone;
    public GreenhouseSpawner spawner;
    public CloudSpawner cloudSpawner;
    public PlanetSpawner planetSpawner;

    public GameObject projectileParent;

    public UITimeDisplay timeDisplay;
    public UILivesDisplay livesDisplay;
    public Text projectilesShotDisplay;
    public ClimateScore climateScore;
    public ClimateImpact climateImpact;

    public static ClimateMinigame minigame;

    [Header("Sprites")]
    public Sprite ozoneSprite;
    public Sprite carbonDioxideSprite;
    public Sprite methaneSprite;

    [Header("Settings")]
    public DifficultyFloat time;
    public int maxHealth;

    float currentTime;
    int currentHealth;
    int numShot;

    [HideInInspector]
    public int currentDefeatedScore;

    [HideInInspector]
    public int numDefeated;

    bool isActive = false;

    void Start()
    {
        minigame = this;
        spawner.climateMinigame = this;
        climateScore.minigame = this;
        climateImpact.minigame = this;
        ozone.minigame = this;
        planetSpawner.climateMinigame = this;

        cloudSpawner.minigameTime = time.Get();
        spawner.minigameTime = time.Get();
    }

    void Update()
    {
        if (!isActive)
            return;

        currentTime -= Time.deltaTime;
        timeDisplay.currentTime = currentTime;
        livesDisplay.lives = currentHealth;
        projectilesShotDisplay.text = numShot.ToString();

        spawner.climateMinigame = this;
        
        if(currentTime < 0)
        {
            EndGame();
        }
    }

    public override string GetName()
    {
        return "Protect the Ozone Layer";
    }

    public override string GetDescription()
    {
        return "Shoot the greenhouse gasses to prevent them from getting to the Ozone Layer";
    }

    public override string GetControls()
    {
        return "Left Arrow: <b>Move Left</b>\nRight Arrow: <b>Move Right</b>\nSpace: <b>Shoot</b>";
    }

    public override string GetScoring()
    {
        return "CO2 Defeated: +20\nMethane Defeated: +60\nHealth Bonus: +50";
    }

    public override List<IntroImageConfig> GetIntroImageConfigs()
    {
        List<IntroImageConfig> configs = new List<IntroImageConfig>();
        configs.Add(new IntroImageConfig(ozoneSprite, "Shoot ozone out of the cannon"));
        configs.Add(new IntroImageConfig(carbonDioxideSprite, "CO2 goes down in 2 hits"));
        configs.Add(new IntroImageConfig(methaneSprite, "Methane shoots its own hydrogen atoms"));

        return configs;
    }

    public override void LoadMinigame()
    {
        cloudSpawner.minigameTime = time.Get();
        spawner.minigameTime = time.Get();

        cloudSpawner.ResetObjects();
        cloudSpawner.StartSpawning();
        climateScore.gameObject.SetActive(false);
        climateImpact.gameObject.SetActive(false);
        planetSpawner.ResetPlanets();

        projectilesShotDisplay.text = "0";

        numShot = 0;
    }

    public override void StartMinigame()
    {
        isActive = true;
        ozone.isActive = true;

        spawner.StartSpawning();
        planetSpawner.StartSpawning();

        currentTime = time.Get();
        currentHealth = maxHealth;
        timeDisplay.gameObject.SetActive(true);

        currentDefeatedScore = 0;
        numDefeated = 0;
    }

    void EndGame()
    {
        isActive = false;
        ozone.isActive = false;

        spawner.StopSpawning();
        cloudSpawner.StopSpawning();
        planetSpawner.StopSpawning();

        timeDisplay.gameObject.SetActive(false);

        // climateScore.gameObject.SetActive(true);
        // climateScore.StartScore();

        climateImpact.gameObject.SetActive(true);
        climateImpact.DisplayImpact();
    }

    public void OnHit()
    {
        currentHealth--;
        livesDisplay.lives = currentHealth;
        if (currentHealth <= 0)
        {
            EndGame();
        }
    }

    public void OnShoot()
    {
        numShot++;
    }

    public int GetCurrentHealth() { return currentHealth; }

    public int GetNumShot() { return numShot; }
}
