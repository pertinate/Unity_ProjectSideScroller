using UnityEngine;
using System.Collections;

public class CompleteCameraController : MonoBehaviour
{
    public static CompleteCameraController instance;
    public GameObject player;       //Public variable to store a reference to the player game object
    public float cameraOffset = 6;
    public float xOffset = 0;
    public float yOffset = 2;

    private Vector3 start;
    private Vector3 offset;         //Private variable to store the offset distance between the player and camera

    void Awake()
    {
        instance = this;
    }
    // Use this for initialization
    void Start()
    {
        start = player.transform.position;
        start.x += xOffset;
        start.y += yOffset;
        start.z -= cameraOffset;
        this.transform.position = start;
        //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        offset = transform.position - player.transform.position;
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
        transform.position = player.transform.position + offset;
    }
}