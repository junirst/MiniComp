using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Snake : MonoBehaviour
{
    public Transform segmentPrefab;
    public Vector2Int direction = Vector2Int.right;
    public float speed = 20f;
    public float speedMultiplier = 1f;
    public int initialSize = 4;
    public float moveThroughWalls = 0f;
    public float verticalBound = 0f;

    private readonly List<Transform> segments = new List<Transform>();
    private Vector2Int input;
    private float nextUpdate;

    private void Start()
    {
        ResetState();
    }

    private void Update()
    {
        if (direction.x != 0f)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
                input = Vector2Int.up;
            } else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
                input = Vector2Int.down;
            }
        }
        else if (direction.y != 0f)
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
                input = Vector2Int.right;
            } else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
                input = Vector2Int.left;
            }
        }
    }

    private void FixedUpdate()
    {
        if (Time.time < nextUpdate) {
            return;
        }

        if (input != Vector2Int.zero) {
            direction = input;
        }

        for (int i = segments.Count - 1; i > 0; i--) {
            segments[i].position = segments[i - 1].position;
        }

        int x = Mathf.RoundToInt(transform.position.x) + direction.x;
        int y = Mathf.RoundToInt(transform.position.y) + direction.y;

        if (moveThroughWalls > 0f)
        {
            float boundX = moveThroughWalls;
            float boundY = verticalBound > 0f ? verticalBound : moveThroughWalls * 0.5f;

            if (x > boundX) x = Mathf.RoundToInt(-boundX);
            else if (x < -boundX) x = Mathf.RoundToInt(boundX);
            if (y > boundY) y = Mathf.RoundToInt(-boundY);
            else if (y < -boundY) y = Mathf.RoundToInt(boundY);
        }

        transform.position = new Vector2(x, y);

        nextUpdate = Time.time + (1f / (speed * speedMultiplier));
    }

    public void Grow()
    {
        Transform segment = Instantiate(segmentPrefab);
        segment.position = segments[segments.Count - 1].position;
        segments.Add(segment);
    }

    public void ResetState()
    {
        direction = Vector2Int.right;
        transform.position = Vector3.zero;

        for (int i = 1; i < segments.Count; i++) {
            Destroy(segments[i].gameObject);
        }

        segments.Clear();
        segments.Add(transform);

        for (int i = 0; i < initialSize - 1; i++) {
            Grow();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Food"))
        {
            Grow();
            ScoreManager.Instance.OnFoodEaten();
            SnakeAudioManager.Instance?.PlayEatSfx();
        }
        else if (other.gameObject.CompareTag("Obstacle"))
        {
            GameManager.Instance.GameOver();
        }
        else if (other.gameObject.CompareTag("Wall"))
        {
            if (moveThroughWalls > 0f) {
                return;
            }
            ResetState();
        }
    }
}
