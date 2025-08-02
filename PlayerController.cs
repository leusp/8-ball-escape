using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;


public class PlayerController : MonoBehaviour
{
   // Rigidbody of the player.
   private Rigidbody rb;


   // Variable to keep track of collected "PickUp" objects.
   private int count;


   // Movement along X and Y axes.
   private float movementX;
   private float movementY;


   // Speed at which the player moves.
   public float speed = 0;


   // UI text component to display count of "PickUp" objects collected.
   public TextMeshProUGUI countText;


   // UI object to display winning text.
   public GameObject winTextObject;


   // Reference to the platform the player is currently standing on.
   private Transform activePlatform;
   private Vector3 lastPlatformPosition; // Platform's position in the previous frame


   // Start is called before the first frame update.
   void Start()
   {
       // Get and store the Rigidbody component attached to the player.
       rb = GetComponent<Rigidbody>();


       // Initialize count to zero.
       count = 0;


       // Update the count display.
       SetCountText();


       // Initially set the win text to be inactive.
       winTextObject.SetActive(false);
   }


   // This function is called when a move input is detected.
   void OnMove(InputValue movementValue)
   {
       // Convert the input value into a Vector2 for movement.
       Vector2 movementVector = movementValue.Get<Vector2>();


       // Store the X and Y components of the movement.
       movementX = movementVector.x;
       movementY = movementVector.y;
   }


   // FixedUpdate is called once per fixed frame-rate frame.
   private void FixedUpdate()
   {
       // Calculate the platform's movement delta (if standing on a platform)
       Vector3 platformMovement = Vector3.zero;
       if (activePlatform != null)
       {
           platformMovement = activePlatform.position - lastPlatformPosition;
           lastPlatformPosition = activePlatform.position;
       }


       // Create a 3D movement vector using the X and Y inputs.
       Vector3 inputMovement = new Vector3(movementX, 0.0f, movementY);


       // Combine platform movement and player input movement
       Vector3 totalMovement = platformMovement + inputMovement * speed * Time.fixedDeltaTime;


       // Apply the total movement to the player's Rigidbody
       rb.MovePosition(rb.position + totalMovement);
   }


   void OnTriggerEnter(Collider other)
   {
       // Check if the object the player collided with has the "PickUp" tag.
       if (other.gameObject.CompareTag("PickUp"))
       {
           // Deactivate the collided object (making it disappear).
           other.gameObject.SetActive(false);


           // Increment the count of "PickUp" objects collected.
           count = count + 1;


           // Update the count display.
           SetCountText();
       }
   }


   // Function to update the displayed count of "PickUp" objects collected.
   void SetCountText()
   {
       // Update the count text with the current count.
       countText.text = "Count: " + count.ToString();


       // Check if the count has reached or exceeded the win condition.
       if (count >= 25)
       {
           // Display the win text.
           winTextObject.SetActive(true);
           Destroy(GameObject.FindGameObjectWithTag("Enemy"));
       }
   }


   private void OnCollisionEnter(Collision collision)
   {
       if (collision.gameObject.CompareTag("Enemy"))
       {
           // Destroy the current object
           Destroy(gameObject);
           // Update the winText to display "You Lose!"
           winTextObject.gameObject.SetActive(true);
           winTextObject.GetComponent<TextMeshProUGUI>().text = "You Lose!";
       }


       // Check if the player is colliding with a moving platform
       if (collision.gameObject.CompareTag("MovingPlatform"))
       {
           // Set the active platform and store its initial position
           activePlatform = collision.transform;
           lastPlatformPosition = activePlatform.position;
       }
   }


   private void OnCollisionExit(Collision collision)
   {
       // Check if the player is leaving the moving platform
       if (collision.gameObject.CompareTag("MovingPlatform"))
       {
           // Unparent the player
           activePlatform = null;
       }
   }
}
