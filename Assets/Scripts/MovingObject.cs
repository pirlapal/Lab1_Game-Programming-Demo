using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public enum MovementType { Translate, Rotate, Both }

    public MovementType movementType = MovementType.Translate;

    public Vector3 moveAxis = Vector3.right;
    public float moveSpeed = 2f;
    public float maxDistance = 3f;

    public Vector3 rotateAxis = Vector3.up;
    public float rotateSpeed = 90f;

    private Vector3 startPosition;
    private int direction = 1;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (movementType == MovementType.Translate || movementType == MovementType.Both)
        {
            transform.position += moveAxis.normalized * moveSpeed * direction * Time.deltaTime;

            float distanceFromStart = Vector3.Distance(transform.position, startPosition);
            if (distanceFromStart >= maxDistance)
            {
                direction *= -1;
            }
        }

        if (movementType == MovementType.Rotate || movementType == MovementType.Both)
        {
            transform.Rotate(rotateAxis * rotateSpeed * Time.deltaTime);
        }
    }
}
