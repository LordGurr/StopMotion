using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoveBetweenFrames : MonoBehaviour
{
    private int frames;
    private int frameRate;
    [SerializeField] private TextMeshProUGUI frameText;
    [SerializeField] private TextMeshProUGUI frameRateText;

    //public List<GameObject> gameObjects = new List<GameObject>();
    public bool playing = false;

    private int currentFrame = 0;
    private float wait = 1;
    private float actualWait = 0;
    [SerializeField] private Slider slider;

    private enum type
    {
        circle,
        line,
        hinge
    }

    private List<FrameDataCircle> frameDatas = new List<FrameDataCircle>();

    // Start is called before the first frame update
    private void Awake()
    {
        frames = 240;
        frameRate = 60;
        wait = 1f / (float)frameRate;
        UpdateUi();
    }

    public void AddGameobject(GameObject theGameobjcet)
    {
        frameDatas.Add(new FrameDataCircle(theGameobjcet, frames));
        frameDatas[frameDatas.Count - 1].calculatePos(frames);
        Debug.Log("Added: " + theGameobjcet.name);
    }

    private void Update()
    {
        if (playing)
        {
            actualWait += Time.deltaTime;
            if (actualWait > wait)
            {
                //Debug.Log("Frame update");
                actualWait -= wait;
                currentFrame++;
                if (currentFrame >= frames)
                {
                    currentFrame = 0;
                }
                showFrame(currentFrame);
                UpdateUi();
            }
        }
    }

    public void startPlay()
    {
        playing = !playing;
        Debug.Log(playing);
    }

    private IEnumerator play()
    {
        float timePassed = 0;

        float smoothTime = 1f / (float)frameRate;
        for (int i = 0; i < frameDatas.Count; i++)
        {
            frameDatas[i].calculatePos(frames);
        }
        for (int a = 0; a < frames && playing; a++)
        {
            for (int i = 0; i < frameDatas.Count; i++)
            {
                float inidividualSmoothTIme = (1f / (float)frameRate);

                int amountOfFramesTillMovement = frameDatas[i].framesTillMovement(true, a, frames);
                if (frameDatas[i].calculatedPositions[a] != new Vector3())
                {
                    frameDatas[i].circle.transform.localPosition = frameDatas[i].calculatedPositions[a];
                }

                //Debug.Log(Vector2.Distance(transform.position, theCurrentHooker.transform.position));
            }
            while (timePassed < smoothTime)
            {
                yield return null;
            }
            while (timePassed < smoothTime)
            {
                timePassed -= smoothTime;
            }
        }
    }

    private void UpdateUi()
    {
        frameText.text = "Current frame: " + currentFrame + " / " + frames;
        frameRateText.text = "Framerate: " + frameRate;
        slider.minValue = 0;
        slider.maxValue = frames - 1;
        slider.value = currentFrame;
    }

    public void showFrame(int frameToShow)
    {
        for (int i = 0; i < frameDatas.Count; i++)
        {
            //frameDatas[i].calculatePos(frames);
            if (frameDatas[i].positions[frameToShow] != new Vector3())
            {
                frameDatas[i].circle.transform.localPosition = new Vector3(frameDatas[i].positions[frameToShow].x, frameDatas[i].positions[frameToShow].y);
            }
            else
            {
                //float inidividualSmoothTIme = (1f / (float)frameRate);

                //int amountOfFramesTillMovement = frameDatas[i].framesTillMovement(true /*currentFrame < frameToShow ? true : false*/, frameToShow, frames);
                //Debug.Log(amountOfFramesTillMovement);
                if (frameDatas[i].calculatedPositions[frameToShow] != new Vector3())
                {
                    frameDatas[i].circle.transform.localPosition = frameDatas[i].calculatedPositions[frameToShow];
                }
            }
            frameDatas[i].setColor(frameToShow);
        }
    }

    public void NextFrame()
    {
        if (currentFrame + 1 < frames && !playing)
        {
            showFrame(currentFrame + 1);
            currentFrame++;
            UpdateUi();
        }
    }

    public void SetFrame(float frameSet)
    {
        //Debug.Log(frameSet);
        int realFrame = Convert.ToInt32(frameSet);
        if (realFrame >= 0 && realFrame < frames && !playing)
        {
            showFrame(realFrame);
            currentFrame = realFrame;
            UpdateUi();
        }
    }

    public void PreviousFrame()
    {
        if (currentFrame - 1 >= 0 && !playing)
        {
            showFrame(currentFrame - 1);
            currentFrame--;
            UpdateUi();
        }
    }

    public void record(GameObject gameObject)
    {
        //for (int a = 0; a < gameObjects.Count; a++)
        //{
        //    bool wasEqualToOne = false;
        //    for (int i = 0; i < frameDatas.Count; i++)
        //    {
        //        if (gameObjects[a] == frameDatas[i].circle)
        //        {
        //            if (currentFrame >= frameDatas[i].positions.Count)
        //            {
        //                while (currentFrame < frameDatas[i].positions.Count)
        //                {
        //                    frameDatas[i].positions.Add(new Vector3());
        //                }
        //            }
        //            frameDatas[i].positions[currentFrame] = gameObjects[a].transform.position;
        //            wasEqualToOne = true;
        //        }
        //    }
        //    if (!wasEqualToOne)
        //    {
        //        frameDatas.Add(new FrameDataCircle(gameObjects[a], frames));
        //        frameDatas[frameDatas.Count - 1].positions[currentFrame] = gameObjects[a].transform.position;
        //    }
        //}

        for (int i = 0; i < frameDatas.Count && !playing; i++)
        {
            if (gameObject == frameDatas[i].circle)
            {
                /*if (currentFrame >= frameDatas[i].positions.Count)
                {
                    while (currentFrame < frameDatas[i].positions.Count)
                    {
                        frameDatas[i].positions.Add(new Vector3());
                    }
                }*/
                frameDatas[i].positions[currentFrame] = gameObject.transform.localPosition;
                frameDatas[i].calculatePos(frames);
                frameDatas[i].setColor(currentFrame);
                Debug.Log("Saved " + gameObject.name + "'s positon " + gameObject.transform.localPosition + " to " + i);
            }
        }
    }

    public void remove(GameObject gameObject)
    {
        for (int i = 0; i < frameDatas.Count && !playing; i++)
        {
            if (gameObject == frameDatas[i].circle)
            {
                /*if (currentFrame >= frameDatas[i].positions.Count)
                {
                    while (currentFrame < frameDatas[i].positions.Count)
                    {
                        frameDatas[i].positions.Add(new Vector3());
                    }
                }*/
                frameDatas[i].positions[currentFrame] = new Vector3();
                frameDatas[i].calculatePos(frames);
                frameDatas[i].setColor(currentFrame);
                Debug.Log("removed " + gameObject.name + "'s positon " + gameObject.transform.localPosition + " from " + i);
            }
        }
    }

    public void delete(GameObject gameObject)
    {
        for (int i = 0; i < frameDatas.Count && !playing; i++)
        {
            if (gameObject == frameDatas[i].circle)
            {
                /*if (currentFrame >= frameDatas[i].positions.Count)
                {
                    while (currentFrame < frameDatas[i].positions.Count)
                    {
                        frameDatas[i].positions.Add(new Vector3());
                    }
                }*/
                frameDatas.RemoveAt(i);
                Debug.Log("deleted " + gameObject.name + "'s positon " + gameObject.transform.localPosition + " from " + i);
            }
        }
    }

    private class FrameDataCircle
    {
        public List<Vector3> positions = new List<Vector3>();

        public List<Vector3> calculatedPositions = new List<Vector3>();

        public GameObject circle;
        private float yVelocity = 0.0f;
        private float xVelocity = 0.0f;
        private Image image;

        public void setColor(int frameToShow)
        {
            bool exists = false;
            for (int i = 0; i < positions.Count; i++)
            {
                if (positions[i] != new Vector3() && i == frameToShow)
                {
                    image.color = new Color(1, 0, 0, 1);
                    exists = true;
                    break;
                }
            }
            if (!exists)
            {
                image.color = new Color(1, 1, 1, 1);
            }
        }

        public FrameDataCircle(GameObject myCircle, int frames)
        {
            circle = myCircle;
            for (int i = 0; i < frames; i++)
            {
                positions.Add(new Vector3());
            }
            image = circle.GetComponent<Image>();
        }

        public int framesTillMovement(bool checkForward, int frameFrom, int frames)
        {
            if (checkForward)
            {
                for (int i = frameFrom; i < frames; i++)
                {
                    if (positions[i] != new Vector3())
                    {
                        return i - frameFrom;
                    }
                }
            }
            else
            {
                for (int i = frameFrom; i > 0; i--)
                {
                    if (positions[i] != new Vector3())
                    {
                        return frameFrom - i;
                    }
                }
            }
            return -1;
        }

        public int nextMovementFrame(int frameFrom, int frames)
        {
            for (int i = frameFrom; i < frames; i++)
            {
                if (positions[i] != new Vector3())
                {
                    return i;
                }
            }

            return -1;
        }

        public void calculatePos(int frames)
        {
            calculatedPositions.Clear();
            xVelocity = 0.0f;
            yVelocity = 0.0f;

            Vector3 currPos = circle.transform.localPosition;

            int amountOfFramesTillMovement = framesTillMovement(true, 0, frames);
            int latestPos = -1;
            int nextPos = nextMovementFrame(0, frames);
            for (int a = 0; a < frames; a++)
            {
                if (positions[a] != new Vector3())
                {
                    calculatedPositions.Add(positions[a]);
                    currPos = positions[a];
                    amountOfFramesTillMovement = framesTillMovement(true, a + 1, frames);
                    xVelocity = 0.0f;
                    yVelocity = 0.0f;
                    latestPos = a;
                    nextPos = nextMovementFrame(a + 1, frames);
                }
                else if (amountOfFramesTillMovement > 0)
                {
                    if (nextPos < 0 || nextPos >= positions.Count)
                    {
                        calculatedPositions.Add(new Vector3());
                        Debug.LogError("too low " + (a + amountOfFramesTillMovement));
                    }
                    else
                    {
                        float temp = (((float)a - (float)latestPos) / ((float)nextPos - (float)latestPos));
                        //Debug.Log(a + "-" + latestPos + "/" + nextPos + "-" + latestPos + "=" + temp);
                        ////for (int i = 0; i < 5; i++)
                        ////{
                        ////    Debug.Log((((float)a + (float)i - (float)latestPos) / ((float)nextPos - (float)latestPos)));
                        ////}
                        //Debug.Log("");
                        float amountToMoveY = Mathf.Lerp(currPos.y, positions[nextPos].y, nextPos < 0 || latestPos < 0 ? 0 : temp);
                        float amountToMoveX = Mathf.Lerp(currPos.x, positions[nextPos].x, nextPos < 0 || latestPos < 0 ? 0 : temp);
                        calculatedPositions.Add(new Vector3(amountToMoveX, amountToMoveY));
                        //currPos = new Vector3(amountToMoveX, amountToMoveY);
                        nextPos = nextMovementFrame(a, frames);
                    }
                }
                else
                {
                    calculatedPositions.Add(new Vector3());
                }
            }
        }
    }

    private class FrameDataLine
    {
        public List<Vector3> positions = new List<Vector3>();
        public GameObject pointA;
        public GameObject pointB;

        public FrameDataLine()
        {
        }
    }

    //private void OnDrawGizmos()
    //{
    //    for (int i = 0; i < frameDatas.Count; i++)
    //    {
    //        Vector3 pointA = new Vector3();
    //        Vector3 pointB = new Vector3();
    //        int mov = frameDatas[i].nextMovementFrame(0, frames);
    //        while (mov >= 0)
    //        {
    //            if (pointA != new Vector3())
    //            {
    //                pointB = frameDatas[i].positions[mov];
    //            }
    //            else if (pointB != new Vector3())
    //            {
    //                Gizmos.DrawLine(pointA, pointB);
    //                Debug.Log("drewLine");
    //            }
    //            else
    //            {
    //                pointA = frameDatas[i].positions[mov];
    //            }
    //            mov = frameDatas[i].nextMovementFrame(mov + 1, frames);
    //        }
    //    }
    //}
}