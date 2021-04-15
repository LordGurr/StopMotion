using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class circleSpawner : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private GameObject circle;
    private GameObject canvas;
    private MoveBetweenFrames moveBetweenFrames;

    // Start is called before the first frame update
    private void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        moveBetweenFrames = canvas.GetComponent<MoveBetweenFrames>();
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        //Output the name of the GameObject that is being clicked
        if (!moveBetweenFrames.playing)
        {
            Debug.Log(name + " Game Object Click in Progress");
            GameObject temp = Instantiate(circle, new Vector3(), Quaternion.identity);
            temp.transform.parent = canvas.transform;
            temp.transform.localPosition = transform.localPosition;
            temp.transform.localScale = new Vector3(1, 1, 1);
            temp.GetComponent<Circle>().beingHeld = true;
        }
    }

    //Detect if clicks are no longer registering
    public void OnPointerUp(PointerEventData pointerEventData)
    {
        Debug.Log(name + " No longer being clicked");
    }
}