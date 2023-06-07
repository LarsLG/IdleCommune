using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovment : MonoBehaviour
{
    public KeyCode cameraLeft = KeyCode.Q;
    public KeyCode cameraRight = KeyCode.E;
    public KeyCode forward = KeyCode.W;
    public KeyCode backward = KeyCode.S;
    public KeyCode left = KeyCode.A;
    public KeyCode right = KeyCode.D;
    public KeyCode changeOptic = KeyCode.Tab;
    public float rotationSpeed = 200;
    public float rotationDistance = 45f;
    [SerializeField] private GameplayManager GameplayManager;
    private Quaternion currentRotation;
    private float moveSpeed = 30;
    private GameObject truecam;
    // Start is called before the first frame update
    void Start()
    {
        truecam = transform.GetChild(0).gameObject;
        currentRotation = transform.rotation;
        //Debug.Log(currentPosition);
    }

    // Update is called once per frame
    void Update()
    {
        moveSpeed = truecam.transform.position.y;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, currentRotation, rotationSpeed * Time.deltaTime);
        if (Input.GetKeyDown(cameraLeft))
        {
            currentRotation = Quaternion.AngleAxis(rotationDistance, Vector3.up) * currentRotation;
            Debug.Log(transform.eulerAngles);
        }
        if (Input.GetKeyDown(cameraRight))
        {
            currentRotation = Quaternion.AngleAxis(rotationDistance, Vector3.down) * currentRotation;
            Debug.Log(transform.eulerAngles);
        }
        if (Input.GetKeyDown(changeOptic))
        {
            if (transform.eulerAngles.x < 1)
            {
                currentRotation = Quaternion.Euler(45, transform.eulerAngles.y, transform.eulerAngles.z);
                Debug.Log(transform.eulerAngles);
            }
            if (transform.eulerAngles.x > 0)
            {
                currentRotation = Quaternion.Euler(0, transform.eulerAngles.y, transform.eulerAngles.z);
                Debug.Log(transform.eulerAngles);
            }
        }
        if (Input.GetKey(forward))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
        }
        if (Input.GetKey(backward))
        {
            transform.Translate(Vector3.back * Time.deltaTime * moveSpeed);
        }
        if (Input.GetKey(left))
        {
            transform.Translate(Vector3.left * Time.deltaTime * moveSpeed);
        }
        if (Input.GetKey(right))
        {
            transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
        }
        if (transform.position.y != (float)-1f)
        {
            transform.position = new Vector3(transform.position.x, (float)-1f, transform.position.z);
        }
        if (transform.position.x < 0)
        {
            transform.position = new Vector3(0,transform.position.y,transform.position.z);
        }
        if (transform.position.z < 0)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
        if (transform.position.x > GameplayManager.width)
        {
            transform.position = new Vector3(GameplayManager.width, transform.position.y, transform.position.z);
        }
        if (transform.position.z > GameplayManager.height)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, GameplayManager.height);
        }
    }
}
