using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Circle : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [HideInInspector] public bool beingHeld = false;
    private MoveBetweenFrames moveBetweenFrames;

    // Start is called before the first frame update
    private Camera cam;

    private void Start()
    {
        moveBetweenFrames = GameObject.FindGameObjectWithTag("Canvas").GetComponent<MoveBetweenFrames>();
        moveBetweenFrames.AddGameobject(gameObject);
        cam = Camera.main;
    }

    // Update is called once per frame
    private void Update()
    {
        if (beingHeld)
        {
            //Debug.Log(new Vector3(Input.mousePosition.x, Input.mousePosition.y));// = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y));
            Vector3 pos = ((new Vector3(((Input.mousePosition.x / (float)Screen.width) - 0.5f), (Input.mousePosition.y / (float)Screen.height) - 0.5f)));
            //Debug.Log(pos);
            pos = new Vector3(pos.x * 1280, pos.y * 720);
            transform.localPosition = pos;
            if (Input.GetMouseButtonUp(0))
            {
                beingHeld = false;
                moveBetweenFrames.record(gameObject);
            }
            if (Input.GetButtonDown("Cancel"))
            {
                Debug.Log("CancelledREgistered");
                beingHeld = false;
                moveBetweenFrames.remove(gameObject);
                transform.localPosition = new Vector3();
            }
            if (Input.GetButtonDown("Delete"))
            {
                moveBetweenFrames.delete(gameObject);
                Destroy(gameObject);
            }
        }
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        //Output the name of the GameObject that is being clicked
        Debug.Log(name + " Game Object Click in Progress");
        beingHeld = true;
    }

    //Detect if clicks are no longer registering
    public void OnPointerUp(PointerEventData pointerEventData)
    {
        Debug.Log(name + " No longer being clicked");
        if (beingHeld)
        {
            beingHeld = false;
            moveBetweenFrames.record(gameObject);
        }
        else
        {
            transform.localPosition = new Vector3();
        }
    }
}