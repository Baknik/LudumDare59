using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using System;

public class StatsManager : MonoBehaviour
{
    [Header("Settings")]
    public float StartTime;
    public int StartMoney;
    public float IncomePerPersonMultiplier;
    public float TimeSpeed;
    public float PayoutTimeFrequency;

    [Header("References")]
    public TextMeshProUGUI PeopleDisplay;
    public TextMeshProUGUI ApprovalDisplay;
    public TextMeshProUGUI MoneyDisplay;
    public TextMeshProUGUI TimeDisplay;
    public TextMeshProUGUI WinText;
    public TextMeshProUGUI LoseText;
    public GameObject GameOverScreen;

    public int Money { get; set; }
    public float TimeRemaining { get; set; }
    public float LastPayoutTime { get; set; }

    private List<Building> _buildings;
    private List<InteractableSpace> _interactableSpaces;

    private void Awake()
    {
        _buildings = FindObjectsByType<Building>(FindObjectsSortMode.None).ToList();
        _interactableSpaces = FindObjectsByType<InteractableSpace>(FindObjectsSortMode.None).ToList();
    }

    private void OnEnable()
    {
        InteractableSpace.OnMoneySpent += OnMoneySpent;
    }

    private void OnDisable()
    {
        InteractableSpace.OnMoneySpent -= OnMoneySpent;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Money = StartMoney;
        TimeRemaining = StartTime;
        LastPayoutTime = TimeRemaining;
        GameOverScreen.SetActive(false);
        WinText.gameObject.SetActive(false);
        LoseText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate temp stats
        var totalPeople = 0;
        var totalPeopleInNetwork = 0;
        var totalApproval = 0;
        foreach (var building in _buildings)
        {
            totalPeople += building.ResidentPeople;
            totalPeopleInNetwork += building.PeopleInNetwork;
            totalApproval += building.Approval;
        }

        var income = Mathf.FloorToInt((float)totalPeopleInNetwork * IncomePerPersonMultiplier);

        // Update time
        TimeRemaining -= Time.deltaTime * TimeSpeed;
        if (TimeRemaining <= 0)
        {
            HandleGameOver(totalPeopleInNetwork >= totalPeople);
        }

        // Update payout
        var nextPayoutTime = LastPayoutTime - PayoutTimeFrequency;
        if (TimeRemaining <= nextPayoutTime)
        {
            LastPayoutTime = nextPayoutTime;
            Money += income;

            // This is also when people enter or leave the network
            foreach (var building in _buildings)
            {
                building.UpdateInNetwork();
            }
        }

        // Update action costs
        foreach (var interactableSpace in _interactableSpaces)
        {
            interactableSpace.UpdateCosts(Money);
        }

        // Update people display
        PeopleDisplay.text = $"<sprite=0>  {totalPeopleInNetwork}/{totalPeople}";

        // Update approval display
        ApprovalDisplay.text = $"<sprite=0> {(totalApproval > 0 ? "+" + totalApproval : totalApproval)}";

        // Update money display
        MoneyDisplay.text = $"<sprite=0> {Money}k (+{income}k monthly)";

        // Update time display
        TimeDisplay.text = $"{TimeRemaining.ToString("F1")} months left";
    }

    private void OnMoneySpent(int money)
    {
        Money = Mathf.Max(0, Money - money);
    }

    private void HandleGameOver(bool win)
    {
        GameOverScreen.SetActive(true);

        if (win)
        {
            WinText.gameObject.SetActive(true);
        }
        else
        {
            LoseText.gameObject.SetActive(true);
        }
    }
}
