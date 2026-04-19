using UnityEngine;

public class Building : MonoBehaviour
{
    [Header("Settings")]
    public int SignalInterference { get; set; }
    public int ActualPeople { get; set; }

    [Header("In Game Don't Touch")]
    public int PeopleInNetwork { get; set; }
    public int Approval { get; set; }
    public int Signal { get; set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
