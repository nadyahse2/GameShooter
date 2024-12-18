using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Player : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float jumpforce = 7f;

    private CharacterController charCont;
    public Transform cameraTransform;
    public GameObject PausePanel;
    public bool Panel_Active = false;

    void Start()
    {
        charCont = GetComponent<CharacterController>();
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        if (Panel_Active == false)
        {
            MovePlayer();
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (!Panel_Active)
            {
                PausePanel.SetActive(true);
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Panel_Active = true;
            }
            else
            {
                Time.timeScale = 1f;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Panel_Active = false;
                PausePanel.SetActive(false);

            }
        }

    }

    private void MovePlayer()
    {
        float deltaX = Input.GetAxis("Horizontal");
        float deltaZ = Input.GetAxis("Vertical");
        
        Vector3 moveDirection = new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.z).normalized;
        Vector3 rightDirection = cameraTransform.right.normalized;
        Vector3 movement = (moveDirection * deltaZ + rightDirection * deltaX);
        movement *= Time.deltaTime * speed;
                  

        movement.y = gravity * Time.deltaTime;

        charCont.Move(movement);
    }
}
