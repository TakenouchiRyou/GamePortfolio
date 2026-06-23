using UnityEngine;
using Pathfinding;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 3f;
    private Seeker seeker;
    private Rigidbody2D rb;
    private Path path;
    private int currentWaypoint = 0;
    private Transform player;

    void OnEnable()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    void OnDisable()
    {
        CancelInvoke("UpdatePath");
    }

    void UpdatePath()
    {
        if (player == null) return;
        seeker.StartPath(transform.position, player.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void Update()
    {
        if (Time.timeScale == 0f)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if (path == null) return;
        if (currentWaypoint >= path.vectorPath.Count) return;

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        rb.velocity = direction * moveSpeed;

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < 0.5f)
            currentWaypoint++;
    }
}