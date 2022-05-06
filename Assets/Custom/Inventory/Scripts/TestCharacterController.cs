using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZInventory;

namespace EZInventory
{
    //This is only a very quickly made character controller that should not be used beyond basic testing
    [RequireComponent(typeof(Rigidbody))]
    public class TestCharacterController : MonoBehaviour
    {
        [SerializeField]
        private float moveSpeed;
        [SerializeField]
        private float lookSpeed;

        float xInput;
        float zInput;
        Vector3 camRotation;

        [SerializeField]
        Transform cameraTransform;
        Rigidbody rb;

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            xInput = Input.GetAxis("Horizontal");
            zInput = Input.GetAxis("Vertical");

            camRotation.y += Input.GetAxis("Mouse X") * lookSpeed * Time.timeScale;
            camRotation.x += -Input.GetAxis("Mouse Y") * lookSpeed * Time.timeScale;
            camRotation.x = Mathf.Clamp(camRotation.x, -55, 65);
            cameraTransform.localRotation = Quaternion.Euler(camRotation.x, 0, 0);
            transform.eulerAngles = new Vector2(0, camRotation.y);

        }

        private void FixedUpdate()
        {
            Vector3 cameraForward = cameraTransform.forward;
            cameraForward.y = 0;
            cameraForward.Normalize();
            Vector3 cameraRight = cameraTransform.right;
            cameraRight.y = 0;
            cameraRight.Normalize();

            Vector3 moveDirection = (cameraForward * zInput + cameraRight * xInput).normalized;

            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.deltaTime);
        }
    }
}