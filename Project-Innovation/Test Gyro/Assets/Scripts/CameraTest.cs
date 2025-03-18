using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    [SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private LayerMask wallMask;
    private Vector3 offset;

    [SerializeField] private float x;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        offset.x = 0;
        offset.z = x;
        var ballTransform = player.transform;
        Vector3 desiredPosition = ballTransform.position + offset;

        float wallOffset = 0.3f; // Hoe ver de camera van muren blijft

        Vector3 direction = (desiredPosition - ballTransform.position).normalized;
        float distance = offset.magnitude;

        if (Physics.Raycast(ballTransform.position, direction, out RaycastHit hit, distance, wallMask))
        {
            var oldy = transform.position.y;
            desiredPosition = ballTransform.position + direction * (hit.distance - wallOffset);
            desiredPosition.y = oldy;
        }
        
        if (Physics.Raycast(ballTransform.position, Vector3.up, out RaycastHit hit2, distance))
        {
            desiredPosition.y = (ballTransform.position + direction * (hit2.distance - wallOffset)).y;
        }

        
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        
        transform.LookAt(ballTransform.position);

    }
}