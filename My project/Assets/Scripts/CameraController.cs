using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    float verticalRot = 0;
    float horizontalRot = 0;

    public Transform playerBody;

    [SerializeField] float sens = 9f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (this.GetComponentInParent<Player>().Panel_Active == false)
        {
            verticalRot -= Input.GetAxis("Mouse Y") * sens;
            verticalRot = Mathf.Clamp(verticalRot, -45, 45);

            float delta = Input.GetAxis("Mouse X") * sens;
            horizontalRot = transform.localEulerAngles.y + delta;

            transform.localEulerAngles = new Vector3(verticalRot, horizontalRot, 0);
            playerBody.Rotate(Vector3.up, delta);
        }
    }
}
