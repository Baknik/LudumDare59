using UnityEngine;
using TMPro;

public class InteractableSpace : MonoBehaviour
{
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TowerBuilt = false;
        SignalStrength = 0;
        Range = 0;
        Selected = false;
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

        // Other
        SignalRangeSprite.gameObject.SetActive(Selected && TowerBuilt);
    }
}
