using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class line : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Image image;
    private bool beingHeld = false;
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float divider = 100;
    private MoveBetweenFrames moveBetweenFrames;
    private GameObject canvas;

    // Start is called before the first frame update
    private void Start()
    {
        image = GetComponent<Image>();
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        moveBetweenFrames = canvas.GetComponent<MoveBetweenFrames>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!beingHeld)
        {
            if (pointA != null && pointB != null)
            {
                Stretch(gameObject, pointA.localPosition, pointB.localPosition, false);
            }
            else if (pointA != null)
            {
                Vector3 pos = ((new Vector3(((Input.mousePosition.x / (float)Screen.width) - 0.5f), (Input.mousePosition.y / (float)Screen.height) - 0.5f)));
                //Debug.Log(pos);
                pos = new Vector3(pos.x * 1280, pos.y * 720);
                Stretch(gameObject, pointA.localPosition, pos, false);
                if (Input.GetMouseButtonUp(0))
                {
                    pointB = ClosestCircle();
                    if (pointA == pointB)
                    {
                        pointB = null;
                    }
                }
            }
        }
        else
        {
            Vector3 pos = ((new Vector3(((Input.mousePosition.x / (float)Screen.width) - 0.5f), (Input.mousePosition.y / (float)Screen.height) - 0.5f)));
            //Debug.Log(pos);
            pos = new Vector3(pos.x * 1280, pos.y * 720);
            transform.localPosition = pos;
            if (Input.GetMouseButtonUp(0))
            {
                if (!moveBetweenFrames.playing)
                {
                    if (pointB != null || pointB == null && pointA == null)
                    {
                        Hold();
                    }
                }
            }
            if (Input.GetButtonDown("Delete"))
            {
                Destroy(gameObject);
            }
        }
    }

    public void Stretch(GameObject _sprite, Vector3 _initialPosition, Vector3 _finalPosition, bool _mirrorZ)
    {
        Vector3 centerPos = (_initialPosition + _finalPosition) / 2f;
        _sprite.transform.localPosition = centerPos;
        Vector3 direction = _finalPosition - _initialPosition;
        direction = Vector3.Normalize(direction);
        _sprite.transform.right = direction;
        if (_mirrorZ) _sprite.transform.right *= -1f;
        Vector3 scale = new Vector3(1, 0.4f, 1);
        scale.x = Vector3.Distance(_initialPosition, _finalPosition);
        //_sprite.transform.localScale = scale;
        scale.x /= (divider * (1280f / Screen.width) * (0.078125f * Screen.width));
        gameObject.transform.localScale = scale;
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        //Output the name of the GameObject that is being clicked
        Debug.Log(name + " Game Object Click in Progress");
        if (!moveBetweenFrames.playing)
        {
            if (pointB != null || pointB == null && pointA == null)
            {
                Hold();
            }
        }
    }

    public void Hold()
    {
        beingHeld = true;
        transform.rotation = Quaternion.identity;
        transform.localScale = new Vector3(0.5f, 0.5f, 1);
        pointA = null;
        pointB = null;
    }

    //Detect if clicks are no longer registering
    public void OnPointerUp(PointerEventData pointerEventData)
    {
        Debug.Log(name + " No longer being clicked");
        if (!moveBetweenFrames.playing)
        {
            if (pointB == null && pointA == null)
            {
                beingHeld = false;
                pointA = ClosestCircle();
                pointB = null;
            }
            //else if (pointB == null && pointA != null)
            //{
            //    beingHeld = false;
            //    pointB = ClosestCircle();
            //}
        }
    }

    private Transform ClosestCircle()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll((Input.mousePosition), 100);
        Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        Debug.Log(transform.position);
        List<StoreGameObjectWithDistance> gameObjects = new List<StoreGameObjectWithDistance>();
        for (int i = 0; i < collider2Ds.Length; i++)
        {
            gameObjects.Add(new StoreGameObjectWithDistance(collider2Ds[i].gameObject, Vector3.Distance(transform.position, collider2Ds[i].transform.position)));
            Debug.Log(gameObjects[i].myGameobject.name);
        }
        if (gameObjects.Count > 0)
        {
            if (gameObjects.Count > 1)
            {
                gameObjects = gameObjects.OrderByDescending(o => o.myDistance).ToList();
                gameObjects.Reverse();
            }
            return gameObjects[0].myGameobject.transform;
        }
        return null;
    }

    private class StoreGameObjectWithDistance
    {
        public GameObject myGameobject;
        public float myDistance;

        public StoreGameObjectWithDistance(GameObject theGameobject, float theDistance)
        {
            myGameobject = theGameobject;
            myDistance = theDistance;
        }
    }
}