using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Building : MonoBehaviour
{
    [Header("Settings")]
    public int SignalInterference;
    public int ResidentPeople;
    public float ApprovalEffectMultiplier;
    public LayerMask BuildingLayerMask;

    [Header("References")]
    public TextMeshProUGUI PeopleDisplay;
    public TextMeshProUGUI ApprovalDisplay;
    public TextMeshProUGUI SignalDisplay;
    public TextMeshProUGUI SignalInterferenceDisplay;
    public GameObject StatsDisplayObject;
    public Collider2D Collider;

    [Header("In Game Don't Touch")]
    public int PeopleInNetwork { get; set; }
    public int Approval { get; set; }
    public int Signal { get; set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PeopleInNetwork = Mathf.FloorToInt(ResidentPeople / 4);
        Approval = 0;
        Signal = 0;

        StatsDisplayObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Update approval
        var peopleWithoutSignal = PeopleInNetwork - Signal;
        Approval = Signal - peopleWithoutSignal;

        // Update displays
        PeopleDisplay.text = $"<sprite=0> {PeopleInNetwork}/{ResidentPeople} in-network";
        ApprovalDisplay.text = $"<sprite=0> {(Approval > 0 ? "+" + Approval : Approval)} approval";
        SignalDisplay.text = $"<sprite=0> {Signal}/{PeopleInNetwork} have signal";
        SignalInterferenceDisplay.text = $"<sprite=0> {SignalInterference} building interference";

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 50, BuildingLayerMask);

        if (hit.collider != null &&
            hit.collider == Collider)
        {
            StatsDisplayObject.SetActive(true);
        }
        else
        {
            StatsDisplayObject.SetActive(false);
        }
    }

    public void UpdateInNetwork()
    {
        var delta = Mathf.FloorToInt((float)Approval * (Random.value * ApprovalEffectMultiplier));
        PeopleInNetwork = Mathf.Clamp(PeopleInNetwork + delta, 0, ResidentPeople);
    }
}
