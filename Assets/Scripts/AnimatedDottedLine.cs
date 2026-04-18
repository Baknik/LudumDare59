using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class AnimatedDottedLine : MonoBehaviour
{
    public float ScrollSpeed = 1f;

    private LineRenderer lr;
    private Material mat;
    private float offset;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        mat = lr.material;
    }

    void Update()
    {
        offset += Time.deltaTime * ScrollSpeed;
        mat.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }
}
