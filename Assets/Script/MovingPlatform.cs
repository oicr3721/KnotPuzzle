using System.Collections;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Move Settings")]
    [SerializeField] private float moveDistance = 3f;
    [SerializeField] private float moveSpeed = 2f;

    private Vector3 startPos;
    private Vector3 targetPos;
    private bool movingRight = true;

    [SerializeField]
    private bool moving = false;

    private void Start()
    {
        startPos = transform.position;
        targetPos = startPos + Vector3.right * moveDistance;
    }

    private void Update()
    {
        if (!moving) return;

        MovePlatform();
    }

    public void SetMoving(bool moving)
    {
        this.moving = moving;
    }

    public void ToggleMoving()
    {
        moving = !moving;
    }

    private void MovePlatform()
    {
        Vector3 target = movingRight ? targetPos : startPos;

        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, target) < 0.01f)
        {
            movingRight = !movingRight;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Character character = collision.collider.GetComponent<Character>();

        if (character != null)
        {
            character.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Character character = collision.collider.GetComponent<Character>();

        if (character != null)
        {
            character.transform.SetParent(null);
        }
    }
}