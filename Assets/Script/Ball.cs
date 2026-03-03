using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
    public int level = 1;      // 球的等級
    public GameObject nextBallPrefab; // 下一級球
    public bool isDroped = false;     // 是否已經落地

    public bool isMerging = false;   // 防止重複合成
    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Ball other = collision.gameObject.GetComponent<Ball>();
        isDroped = true;

        if (other != null &&
            other.level == level &&
            !other.isMerging &&
            !isMerging)
        {
            Merge(other);
        }
    }
    void Merge(Ball other)
    {
        if(nextBallPrefab == null) return; // 如果沒有下一級球，直接返回
        isMerging = true;
        other.isMerging = true;
        Vector2 mergePos = other.transform.position;
        if(transform.position.y < other.transform.position.y)
        {
            mergePos = transform.position;
        }

        GameObject newBallObj = Instantiate(nextBallPrefab, mergePos, Quaternion.identity);
        Ball newBall = newBallObj.GetComponent<Ball>();
        newBall.StartCoroutine(newBall.MergeDelay());

        GameManager.AddScore(level * 10); // 加分，分數為等級*10
    
        Destroy(other.gameObject, 0.02f);
        Destroy(gameObject, 0.02f);
    }
    public IEnumerator MergeDelay()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        rb.simulated = false;   // 暫停物理
        yield return new WaitForSeconds(0.15f); // 停頓 0.15 秒
        rb.simulated = true;    // 恢復物理
    }
}