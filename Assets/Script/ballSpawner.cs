using UnityEngine;
using System.Collections;

public class ballSpawner : MonoBehaviour
{
    public GameObject[] ballPrefabs;
    public float previewY = 1f;
    public float radius;

    private GameObject currentPreview;
    private GameObject currentBallPrefab;

    public bool canSpawn = true;
    private bool previewExists = false;
    private float spawnDelay = 0.6f;
    
    private void Start() {
        CreatePreviewBall();
    }

    void Update()
    {
        MovePreview();
        if (Input.GetMouseButtonUp(0) && !GameManager.isGameOver)
        {
            SpawnBall();
        }
    }

    void CreatePreviewBall()
    {
        int index = Random.Range(0, 100);
        if(index < 30 && index >= 0)
        {
            currentBallPrefab = ballPrefabs[0];
            radius = 0.2f;
        }else if(index < 60 && index >= 30)
        {
            currentBallPrefab = ballPrefabs[1];
            radius = 0.4f;
        }else if(index < 80 && index >= 60)
        {
            currentBallPrefab = ballPrefabs[2];
            radius = 0.55f;
        }else if(index < 95 && index >= 80)
        {
            currentBallPrefab = ballPrefabs[3];
            radius = 0.7f;
        }else if(index < 100 && index >= 95)
        {
            currentBallPrefab = ballPrefabs[4];
            radius = 0.8f;
        }

        currentPreview = Instantiate(currentBallPrefab);
        previewExists = true;

        // 關閉物理
        currentPreview.GetComponent<Rigidbody2D>().simulated = false;
        // 關閉碰撞
        currentPreview.GetComponent<CircleCollider2D>().enabled = false;
    }
    void MovePreview()
    {
        if (!previewExists) return;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float clampedX = mousePos.x ;
        if(mousePos.x + radius > 3.5)
        {
            clampedX = 3.5f - radius;
        }else if(mousePos.x - radius < -3.5)
        {
            clampedX = -3.5f + radius;
        }

        currentPreview.transform.position = new Vector2(clampedX, previewY);
    }
    void SpawnBall()
    {
        if(!canSpawn) return;
        GameObject newBall = Instantiate(currentBallPrefab, currentPreview.transform.position, Quaternion.identity);
        Destroy(currentPreview);
        previewExists = false;     
        StartCoroutine(SpawnDelay());
    }
    public IEnumerator SpawnDelay()
    {
        canSpawn = false;
        yield return new WaitForSeconds(spawnDelay);
        if (!GameManager.isGameOver)
        {
            CreatePreviewBall();
            canSpawn = true;
        }
    }
}
