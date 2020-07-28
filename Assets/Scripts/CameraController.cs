using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public float normalSpeed;
    public float fastSpeed;
    private float movementSpeed;
    public float movementTime;
    public float rotationAmount;
    public Vector3 zoomAmount;

    public Transform camTransform;
    public Transform followTransform;
    private Quaternion newRotation;
    private Vector3 newPosition;
    private Vector3 newZoom;

    private Vector3 dragStartPosition;
    private Vector3 dragCurrentPosition;
    private Vector3 rotateStartPosition;
    private Vector3 rotateCurrentPosition;

    public event EventHandler OnDelelect; 
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = camTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (followTransform != null)
        {
            transform.position = followTransform.position;
        }


        HandleMovementInput();
        HandleMouseInput();


        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (OnDelelect != null)
                OnDelelect(this, EventArgs.Empty);

            if(followTransform != null)
                newPosition = followTransform.transform.position;

            followTransform = null;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

    }

    void HandleMouseInput()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            newZoom -= Input.mouseScrollDelta.y * zoomAmount;
        }

        // No moving when locked
        if (followTransform == null)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Plane plane = new Plane(Vector3.forward, Vector3.zero);

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                float entry;

                if (plane.Raycast(ray, out entry))
                {
                    dragStartPosition = ray.GetPoint(entry);
                }
            }
            if (Input.GetMouseButton(1))
            {
                Plane plane = new Plane(Vector3.forward, Vector3.zero);

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                float entry;

                if (plane.Raycast(ray, out entry))
                {
                    dragCurrentPosition = ray.GetPoint(entry);

                    newPosition = transform.position + (dragStartPosition - dragCurrentPosition);
                }
            }
        }

        //if (Input.GetMouseButtonDown(2))
        //{
        //    rotateStartPosition = Input.mousePosition;
        //}
        //if (Input.GetMouseButton(2))
        //{
        //    rotateCurrentPosition = Input.mousePosition;

        //    Vector3 diff = rotateStartPosition - rotateCurrentPosition;
        //    rotateStartPosition = rotateCurrentPosition;

        //    newRotation *= Quaternion.Euler(Vector3.up * (-diff.x / 5));
        //}

    }

    void HandleMovementInput()
    {

        if (followTransform == null)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                movementSpeed = fastSpeed;
            }
            else
            {
                movementSpeed = normalSpeed;
            }


            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                newPosition += transform.up * movementSpeed;
            }

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                newPosition += transform.up * -movementSpeed;
            }

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                newPosition += transform.right * movementSpeed;
            }

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                newPosition += transform.right * -movementSpeed;
            }

            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
        }

        //if (Input.GetKey(KeyCode.Q))
        //{
        //    newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        //}

        //if (Input.GetKey(KeyCode.E))
        //{
        //    newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
        //}


        if(Input.GetKey(KeyCode.R))
        {
            newZoom += zoomAmount;
        }

        if (Input.GetKey(KeyCode.F))
        {
            newZoom -= zoomAmount;
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);       
        camTransform.localPosition = Vector3.Lerp(camTransform.localPosition, newZoom, Time.deltaTime * movementTime);
    }
}
