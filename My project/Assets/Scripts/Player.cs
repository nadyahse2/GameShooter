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

    void Start()
    {
        charCont = GetComponent<CharacterController>();
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        MovePlayer();
       

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
