using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [SerializeField] private Transform followAt;
    [SerializeField] private Vector3 velocity = Vector3.zero;
    [SerializeField] private Vector3 offset = new(-6.0f, -1.0f, -10f);
    [SerializeField] private float dumpingTime = 0.3f;

    [SerializeField] private Vector3 originalOffset;

    private void Awake()
    {
        // intentaremos ir a 60 fps
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        originalOffset = offset;
    }

    private void Update()
    {
        MoveCamera(true);
    }

    public void ResetCameraPosition()
    {
        MoveCamera(false);
    }

    private void MoveCamera(bool smooth)
    {
        
        if (GameManager.sharedInstance.currentGameState != GameState.inGame) return;

        Vector3 destination = new Vector3(followAt.position.x - offset.x, offset.y, offset.z);

        if (smooth)
        {
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dumpingTime);
        }
        else transform.position = destination;

    }

    public void ChangeCameraOffset(Vector3 newOffset)
    {
        offset = newOffset;
    }

    public void ResetOffest()
    {
        offset = originalOffset;
    }
}
