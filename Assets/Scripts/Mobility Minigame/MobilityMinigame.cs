using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MobilityMinigame : IMinigame
{
    public Bicycle bicycle;
    public BikePathSpawner pathSpawner;
    public CoinSpawner coinSpawner;
    public Text coinText;
    public Slider staminaDisplay;
    public UILivesDisplay livesDisplay;
    public MobilityScore mobilityScore;
    public MobilityImpact mobilityImpact;
    public ParkSystem rightParkSystem;
    public ParkSystem leftParkSystem;

    int numCoins = 0;

    [Header("Sprites")]
    public Sprite bicycleSprite;
    public Sprite waterBottleSprite;
    public Sprite obstacleSprite;

    [Header("Settings")]
    public DifficultyFloat worldMinSpeed;
    public DifficultyFloat worldSpeedStep;
    public DifficultyFloat timeBetweenSpeedSteps;
    public DifficultyFloat distanceBetweenSpawns;
    public float maxStamina;
    public float staminaDrainRate;
    public Gradient staminaBarGradient;
    public float coinStamina;
    public int maxLives;

    float currentSpeed;
    float currentStamina;
    float currentDistance;
    int currentLives;

    Image staminaFill;

    bool isActive = false;

    void Start()
    {
        pathSpawner.speed = worldMinSpeed.Get();
        coinSpawner.speed = worldMinSpeed.Get();
        rightParkSystem.speed = worldMinSpeed.Get();
        leftParkSystem.speed = worldMinSpeed.Get();

        bicycle.minigame = this;
        mobilityScore.minigame = this;
        mobilityImpact.minigame = this;

        coinSpawner.minigameTime = 30;

        staminaFill = staminaDisplay.fillRect.GetComponent<Image>();
    }

    void Update()
    {
        if (!isActive) return;

        currentStamina -= staminaDrainRate * Time.deltaTime * Mathf.Pow(GetSpeedRatio(), 1.3f);
        currentDistance += currentSpeed * Time.deltaTime;

        staminaDisplay.value = currentStamina / maxStamina;
        staminaFill.color = staminaBarGradient.Evaluate(currentStamina / maxStamina);
        coinText.text = numCoins.ToString();
        livesDisplay.lives = currentLives;

        if(currentStamina <= 0 || currentLives <= 0)
        {
            EndGame();
        }
    }

    public override string GetName()
    {
        return "Bicycle Game";
    }

    public override string GetDescription()
    {
        return "Avoid obstacles and collect water bottles to get as far as possible on your bike";
    }

    public override string GetControls()
    {
        return "Left Arrow: <b>Move Left</b>\nRight Arrow: <b>Move Right</b>";
    }

    public override string GetScoring()
    {
        return "Coins Collected: +50\nLives Bonus: +100";
    }

    public override void LoadMinigame()
    {
        if(staminaFill == null)
            staminaFill = staminaDisplay.fillRect.GetComponent<Image>();

        staminaDisplay.value = 1f;
        staminaFill.color = staminaBarGradient.Evaluate(1f);

        livesDisplay.gameObject.SetActive(false);
        mobilityScore.gameObject.SetActive(false);
        mobilityImpact.gameObject.SetActive(false);

        pathSpawner.speed = worldMinSpeed.Get();
        coinSpawner.speed = worldMinSpeed.Get();
        rightParkSystem.speed = worldMinSpeed.Get();
        leftParkSystem.speed = worldMinSpeed.Get();

        rightParkSystem.isActive = true;
        leftParkSystem.isActive = true;

        numCoins = 0;
    }

    public override void StartMinigame()
    {
        isActive = true;
        bicycle.isActive = true;

        coinSpawner.spawnTime = distanceBetweenSpawns.Get() / worldMinSpeed.Get();
        coinSpawner.StartSpawning();

        StartCoroutine(ControlSpeed());

        currentStamina = maxStamina;
        currentLives = maxLives;
        currentDistance = 0;
        currentSpeed = worldMinSpeed.Get();

        livesDisplay.gameObject.SetActive(true);
    }

    public override List<IntroImageConfig> GetIntroImageConfigs()
    {
        List<IntroImageConfig> configs = new List<IntroImageConfig>();
        configs.Add(new IntroImageConfig(bicycleSprite, "Use arrow keys to move bicycle"));
        configs.Add(new IntroImageConfig(waterBottleSprite, "Collect water bottles for fuel"));
        configs.Add(new IntroImageConfig(obstacleSprite, "Avoid hitting obstacles"));

        return configs;
    }

    public void OnCoinCollected()
    {
        numCoins++;
        currentStamina += coinStamina;
    }

    public void OnObstacleHit()
    {
        currentLives--;
    }

    public int GetCoinsCollected() { return numCoins; }

    public int GetNumLives() { return currentLives; }

    public float GetCurrentDistance() { return currentDistance; }

    public float GetCurrentSpeed() { return currentSpeed; }

    public float GetSpeedRatio() { return currentSpeed / worldMinSpeed.Get(); }

    void EndGame()
    {
        isActive = false;
        bicycle.isActive = false;

        coinSpawner.StopSpawning();
        coinSpawner.Clean();

        livesDisplay.gameObject.SetActive(false);

        mobilityImpact.gameObject.SetActive(true);
        mobilityImpact.DisplayImpact();

        rightParkSystem.isActive = true;
        rightParkSystem.Clean();
        leftParkSystem.isActive = true;
        leftParkSystem.Clean();

        StopAllCoroutines();
    }

    IEnumerator ControlSpeed()
    {
        int i = 0;
        while(isActive)
        {
            yield return new WaitForSeconds(timeBetweenSpeedSteps.Get());

            if (!isActive)
                yield break;

            i++;

            pathSpawner.speed += worldSpeedStep.Get();
            coinSpawner.speed += worldSpeedStep.Get();
            rightParkSystem.speed += worldSpeedStep.Get();
            leftParkSystem.speed += worldSpeedStep.Get();
            currentSpeed += worldSpeedStep.Get();

            coinSpawner.spawnTime = (distanceBetweenSpawns.Get() + 5 * i) / coinSpawner.speed;
        }
    }
}
