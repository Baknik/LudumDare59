using UnityEngine;
using TMPro;

public class InteractableSpace : MonoBehaviour
{
    public delegate void MoneySpentHandler(int money);
    public static event MoneySpentHandler OnMoneySpent;

    [Header("Settings")]
    public int SignalIncreasePerUpgrade;
    public int RangeIncreasePerUpgrade;
    public int BaseSignal;
    public int BaseRange;
    public float ScaleColliderSizeMultiplier;
    public int UpgradeSignalCostMultiplier;
    public int UpgradeRangeCostMultiplier;
    public int BuildCost;
    public float DemolishPayoutMultiplier;

    [Header("References")]
    public SpriteRenderer TowerSprite;
    public SpriteRenderer SignalRangeSprite;
    public SpriteRenderer SelectedEffectSprite;
    public Collider2D SignalRangeCollider;
    public TextMeshProUGUI CellTowerTitleText;
    public TextMeshProUGUI SignalTitleText;
    public TextMeshProUGUI RangeTitleText;
    public TextMeshProUGUI EmptyPlotTitleText;
    public ActionPanel BuildTowerPanel;
    public ActionPanel DemolishTowerPanel;
    public ActionPanel UpgradeSignalTowerPanel;
    public ActionPanel UpgradeRangeTowerPanel;
    public GameObject PopupMenuObject;

    public bool TowerBuilt { get; set; }
    public int SignalStrength { get; set; }
    public int Range { get; set; }
    public bool Selected { get; set; }

    public int UpgradeSignalCost { get; set; }
    public int UpgradeRangeCost { get; set; }
    public int DemolishPayout { get; set; }

    void Awake()
    {
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TowerBuilt = false;
        Selected = false;

        ResetStats();
    }

    // Update is called once per frame
    void Update()
    {
        // Tower built enable
        CellTowerTitleText.gameObject.SetActive(TowerBuilt);
        SignalTitleText.gameObject.SetActive(TowerBuilt);
        RangeTitleText.gameObject.SetActive(TowerBuilt);
        TowerSprite.gameObject.SetActive(TowerBuilt);
        DemolishTowerPanel.gameObject.SetActive(TowerBuilt);
        UpgradeSignalTowerPanel.gameObject.SetActive(TowerBuilt);
        UpgradeRangeTowerPanel.gameObject.SetActive(TowerBuilt);

        // Tower built disable
        EmptyPlotTitleText.gameObject.SetActive(!TowerBuilt);
        BuildTowerPanel.gameObject.SetActive(!TowerBuilt);

        // Selected enable
        PopupMenuObject.gameObject.SetActive(Selected);
        SelectedEffectSprite.gameObject.SetActive(Selected);

        // Other enable
        SignalRangeSprite.gameObject.SetActive(Selected && TowerBuilt);

        // Signal size
        SignalRangeCollider.transform.localScale = new Vector3(
            ScaleColliderSizeMultiplier * (float)Range,
            ScaleColliderSizeMultiplier * (float)Range,
            1);

        // Menu panel title display
        SignalTitleText.text = $"{SignalStrength} <sprite=0>";
        RangeTitleText.text = $"{Range} <sprite=0>";
    }

    public void UpdateCosts(int currentMoney)
    {
        // Build cost
        BuildTowerPanel.CostDisplay.text = $"{BuildCost}k <sprite=0>";
        BuildTowerPanel.Button.enabled = (currentMoney >= BuildCost);

        // Demolish cost
        var demolishPayout = Mathf.FloorToInt(BuildCost * DemolishPayoutMultiplier);
        DemolishTowerPanel.CostDisplay.text = $"Gain {demolishPayout}k <sprite=0>";
        DemolishPayout = demolishPayout;

        // Upgrade signal cost
        var signalUpgradeCost = CalcUpgradeSignalCost();
        UpgradeSignalTowerPanel.CostDisplay.text = $"{signalUpgradeCost}k <sprite=0>";
        UpgradeSignalTowerPanel.Button.enabled = (currentMoney >= signalUpgradeCost);
        UpgradeSignalCost = signalUpgradeCost;

        // Upgrade range cost
        var rangeUpgradeCost = CalcUpgradeRangeCost();
        UpgradeRangeTowerPanel.CostDisplay.text = $"{rangeUpgradeCost}k <sprite=0>";
        UpgradeRangeTowerPanel.Button.enabled = (currentMoney >= rangeUpgradeCost);
        UpgradeRangeCost = rangeUpgradeCost;
    }

    public void BuildTower()
    {
        TowerBuilt = true;

        ResetStats();

        if (OnMoneySpent != null)
        {
            OnMoneySpent.Invoke(BuildCost);
        }
    }

    public void DemolishTower()
    {
        TowerBuilt = false;

        ResetStats();

        if (OnMoneySpent != null)
        {
            OnMoneySpent.Invoke(DemolishPayout * -1);
        }
    }

    public void UpgradeSignal()
    {
        SignalStrength += SignalIncreasePerUpgrade;

        if (OnMoneySpent != null)
        {
            OnMoneySpent.Invoke(UpgradeSignalCost);
        }
    }

    public void UpgradeRange()
    {
        Range += RangeIncreasePerUpgrade;

        if (OnMoneySpent != null)
        {
            OnMoneySpent.Invoke(UpgradeRangeCost);
        }
    }

    public int CalcUpgradeSignalCost()
    {
        return SignalStrength * UpgradeSignalCostMultiplier;
    }

    public int CalcUpgradeRangeCost()
    {
        return Range * UpgradeRangeCostMultiplier;
    }

    private void ResetStats()
    {
        SignalStrength = BaseSignal;
        Range = BaseRange;
    }
}
