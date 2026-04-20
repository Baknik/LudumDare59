using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SignalManager : MonoBehaviour
{
    [Header("Settings")]
    public ContactFilter2D SignalContactFilter;

    private List<Building> _buildings;
    private List<InteractableSpace> _interactableSpaces;

    private List<Collider2D> _signalColliderOverlapResults;

    private void Awake()
    {
        _buildings = FindObjectsByType<Building>(FindObjectsSortMode.None).ToList();
        _interactableSpaces = FindObjectsByType<InteractableSpace>(FindObjectsSortMode.None).ToList();

        _signalColliderOverlapResults = new List<Collider2D>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        // Clear building signals
        foreach (var building in _buildings)
        {
            building.Signal = 0;
        }

        // Update signals from towers
        foreach (var interactableSpace in _interactableSpaces)
        {
            if (!interactableSpace.TowerBuilt)
            {
                continue;
            }

            _signalColliderOverlapResults.Clear();
            interactableSpace.SignalRangeCollider.Overlap(SignalContactFilter, _signalColliderOverlapResults);
            var availableSignal = interactableSpace.SignalStrength;

            foreach (var collider in _signalColliderOverlapResults)
            {
                if (collider.gameObject.TryGetComponent<Building>(out var building))
                {
                    var signalDemand = building.PeopleInNetwork;
                    var signalSupplyWithInterference = Mathf.Max(0, availableSignal - building.SignalInterference);
                    var signalSupplied = Mathf.Min(signalDemand, signalSupplyWithInterference);

                    building.Signal += signalSupplied;
                    availableSignal -= signalSupplied;
                }
            }
        }
    }
}
