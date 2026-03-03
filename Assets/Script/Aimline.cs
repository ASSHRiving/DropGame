using UnityEngine;

public class Aimline : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public float maxDistance = 20f;
    public float fixedY = 4f;
    public LayerMask hitLayer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        MoveWithMouse();
        DrawLine();
    }
    void MoveWithMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        transform.position = new Vector3(mousePos.x, fixedY, 0f);
    }

    void DrawLine()
    {
        Vector2 startPos = transform.position;

        RaycastHit2D hit = Physics2D.Raycast(startPos, Vector2.down, maxDistance, hitLayer);

        Vector2 endPos = hit.collider != null 
            ? hit.point 
            : startPos + Vector2.down * maxDistance;

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
    }
}