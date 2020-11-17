using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform playerCamera = null;
    [SerializeField] float mouse_sens = 0.5f;
    [SerializeField] bool lock_cursor = true;   
    
    [SerializeField] float speed = 5.0f; 
    
    [SerializeField][Range(0.0f, 0.5f)] float mouse_smooting = 0.5f;   
    Vector2 current_mouse_delta = Vector2.zero;
    Vector2 current_mouse_delta_velocity = Vector2.zero;

    [SerializeField][Range(0.0f, 1.0f)] float move_smooting = 0.5f;
    Vector3 current_dir = Vector2.zero;
    Vector3 current_dir_velocity = Vector2.zero;

 

    float camera_pitch = 0.0f;
    CharacterController controller = null;

    // Start is called before the first frame update
    void Start() {
        controller = GetComponent<CharacterController>();
        if(lock_cursor) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // Update is called once per frame
    void Update() {
        UpdateMouseLook();
        UpdateMovement();
    }

    void UpdateMouseLook() {
        Vector2 target_mouse_delta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        current_mouse_delta = Vector2.SmoothDamp(current_mouse_delta, target_mouse_delta, ref current_mouse_delta_velocity, mouse_smooting);
        
        camera_pitch -= current_mouse_delta.y * mouse_sens;
        camera_pitch = Mathf.Clamp(camera_pitch, -90.0f, 90.0f);
        playerCamera.localEulerAngles = Vector3.right * camera_pitch;


        transform.Rotate(Vector3.up * current_mouse_delta.x * mouse_sens);
    }

    void UpdateMovement() {
        Vector3 target_dir = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Jump"));
        target_dir.Normalize();

        current_dir = Vector3.SmoothDamp(current_dir, target_dir, ref current_dir_velocity, move_smooting);

        Vector3 velocity = (transform.forward *  current_dir.y + transform.right * current_dir.x + transform.up * current_dir.z) * speed;
        controller.Move(velocity * Time.deltaTime);
    }
}