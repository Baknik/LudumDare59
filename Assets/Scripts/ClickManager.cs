using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickManager : MonoBehaviour
{
    [Header("Settings")]
    public LayerMask InteractableSpaceLayerMask;

    private bool _clicked;
    private List<InteractableSpace> _interactableSpaces;

    private void Awake()
    {
        _interactableSpaces = FindObjectsByType<InteractableSpace>(FindObjectsSortMode.None).ToList();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _clicked = false;

        DeselectAllInteractableSpaces();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // If UI is blocking, stop here
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            // Otherwise store clicked for raycasting
            _clicked = true;
        }
    }

    void FixedUpdate()
    {
        if (_clicked)
        {
            _clicked = false;

            DeselectAllInteractableSpaces();

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 100f, InteractableSpaceLayerMask);

            if (hit.collider != null &&
                hit.collider.gameObject.TryGetComponent<InteractableSpace>(out var interactableSpace))
            {
                interactableSpace.Selected = true;
            }
        }
    }

    private void DeselectAllInteractableSpaces()
    {
        foreach (var interactableSpace in _interactableSpaces)
        {
            interactableSpace.Selected = false;
        }
    }
}
