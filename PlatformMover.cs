using UnityEngine;

public class PlatformMover : MonoBehaviour
{
    private Vector3 startPoint;
    public Transform endPoint;

    public float platformSpeed;

    private bool flipped = false;
    private float distance;

    public bool autoPlatform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //if its auto platform, do this
        if (autoPlatform)
        {
            //check if the platform is flipped
            FlipCheck();
        }
    }

    //pass two vector3s from any position when called
    void MovePlatform(Vector3 pos, Vector3 destination)
    {
        transform.position = Vector3.MoveTowards(pos, destination, platformSpeed * Time.deltaTime);
    }

    void FlipCheck()
    {
        //check if the platform has reached its current destination
        //if the platform has reached the destination, flip the platform, set flipped to true
        if (!flipped)
        {
            //move towards endpoint, measure distance, flip when reaching endpoint
            distance = Vector3.Distance(transform.position, endPoint.position);
            if (distance > 0.02f)
            {
                //move the platform towards target
                MovePlatform(transform.position, endPoint.position);
            }
            else
            {
                //flip the platform
                flipped = true;
            }
        }
        else
        {
            //reverse
            distance = Vector3.Distance(transform.position, startPoint);
            if (distance > 0.02f)
            {
                //move the platform towards target
                MovePlatform(transform.position, startPoint);
            }
            else
            {
                flipped = false;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !autoPlatform)
        {
            MovePlatform(transform.position, endPoint.position);
        }
    }
}