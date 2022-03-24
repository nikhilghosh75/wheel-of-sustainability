using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConservationMinigame : IMinigame
{
    public PlayerMovement playerMovement;
    public List<Bear> bears;
    public UIFractionDisplay scoreDisplay;
    public ConservationScore conservationScore;
    public ConservationImpact conservationImpact;
    public ConservationMist mist;
    public WildlifeManager wildlifeManager;
    public GameObject tooltip;
    public Slider progressSlider;
    public GameObject bonusPrefab;

    [Header("Sprites")]
    public Sprite bearSprite;
    public Sprite treeSprite;

    bool isActive = false;
    bool dead = false;
    int treeToSpawnBonus;

    Soil currentSoil;

    void Start()
    {
        conservationImpact.minigame = this;

        foreach (Soil soil in GetComponentsInChildren<Soil>())
        {
            soil.conservationMinigame = this;
        }
    }

    void Update()
    {
        if (!isActive) return;

        UpdateScore();
        CheckIfDone();
    }

    public override string GetName()
    {
        return "Plant Trees";
    }

    public override string GetDescription()
    {
        return "Plant the trees while avoiding the bears";
    }

    public override string GetControls()
    {
        return "W/Up: <b>Move Up</b>\nA/Left: <b>Move Left</b>\nS/Down: <b>Move Down</b>\nD/Right: <b>Move Right</b>\nSpace: <b>Plant Tree</b>";
    }

    public override string GetScoring()
    {
        int plantedTreeScore = conservationScore.treeMultiplier.Get();
        return "Planted Tree: +" + plantedTreeScore.ToString() + "\nSurvival Bonus: +300";
    }

    public override void LoadMinigame()
    {
        foreach (Soil soil in GetComponentsInChildren<Soil>())
        {
            soil.Unplant();
        }

        conservationScore.gameObject.SetActive(false);
        conservationImpact.gameObject.SetActive(false);

        playerMovement.Reset();
        foreach(Bear bear in bears)
        {
            bear.Reset();
        }

        mist.SetColor(mist.startColor);

        treeToSpawnBonus = Random.Range(1, 14);

        dead = false;

        progressSlider.gameObject.SetActive(false);
    }

    public override void StartMinigame()
    {
        playerMovement.isActive = true;

        foreach (Bear bear in bears)
        {
            bear.isActive = true;
        }

        isActive = true;
    }

    public override void PauseMinigame()
    {
        isActive = false;
        playerMovement.isActive = false;
        foreach (Bear bear in bears)
            bear.isActive = false;
    }

    public override void UnpauseMinigame()
    {
        isActive = true;
        playerMovement.isActive = true;
        foreach (Bear bear in bears)
            bear.isActive = true;
    }

    public override List<IntroImageConfig> GetIntroImageConfigs()
    {
        List<IntroImageConfig> configs = new List<IntroImageConfig>();
        configs.Add(new IntroImageConfig(bearSprite, "Avoid the bears"));
        configs.Add(new IntroImageConfig(treeSprite, "Plant Trees at plots of soil"));

        return configs;
    }

    public void Die()
    {
        dead = true;
        // AudioManager.instance.Play("Bear");
        EndGame();
    }

    public int NumPlanted()
    {
        int numPlanted = 0;
        foreach (Soil soil in GetComponentsInChildren<Soil>())
        {
            if (soil.IsPlanted())
            {
                numPlanted++;
            }
        }
        return numPlanted;
    }

    public bool IsDead() { return dead; }

    public void SetCurrentSoil(Soil soil)
    {
        currentSoil = soil;
        tooltip.SetActive(soil == null);

        if(soil != null)
        {
            progressSlider.GetComponent<RectTransform>().anchoredPosition = soil.GetComponent<RectTransform>().anchoredPosition;
        }
    }

    public void OnPlant()
    {
        int numPlanted = NumPlanted();
        if(numPlanted == treeToSpawnBonus && ScreenSwitcher.switcher.minigameMode == MinigameMode.Wheel)
        {
            Instantiate(bonusPrefab, transform);
        }

        float t = (float)numPlanted / (float)15;
        mist.SetColor(Color.Lerp(mist.startColor, mist.endColor, t));
        wildlifeManager.OnNumPlanted(numPlanted);
    }

    void EndGame()
    {
        playerMovement.isActive = false;

        foreach (Bear bear in bears)
            bear.isActive = false;

        isActive = false;

        // conservationScore.gameObject.SetActive(true);
        // conservationScore.StartScore();

        conservationImpact.gameObject.SetActive(true);
        conservationImpact.DisplayImpact();
    }

    void UpdateScore()
    {
        int numSoils = GetComponentsInChildren<Soil>().Length;
        int numPlanted = 0;
        foreach (Soil soil in GetComponentsInChildren<Soil>())
        {
            if (soil.IsPlanted())
            {
                numPlanted++;
            }
        }

        scoreDisplay.denominator = numSoils;
        scoreDisplay.numerator = numPlanted;
    }

    void CheckIfDone()
    {
        foreach (Soil soil in GetComponentsInChildren<Soil>())
        {
            if (!soil.IsPlanted()) return;
        }

        EndGame();
    }
}
