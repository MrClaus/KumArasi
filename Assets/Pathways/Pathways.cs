using UnityEngine;
using Random = System.Random;
using System;

public class Pathways : MonoBehaviour
{
    private static Random rnd = new Random();
    private static Pathways _instance;

    [Header("Map Set")]
    public int StartMapSize = 10;
    public Path[] pathList;

    [Header("Map Movement")]
    [SerializeField] private float updateMapSpeed = 7f;
    [SerializeField] private float updateHorizontalSpeed = 4f;    

    private int lastType;
    private Path lastPath;
    private double currentTime;
    private float deltaTime = 1f;
    private bool isStopMoving, isStartMoving, isMoving;
    private float[] positionsPlayer = { -2f, 0, 2f };
    private int idPosition = 1, idToPosition;
    private float currentMapSpeed, currentXPosition;


    private void Awake()
    {
        _instance = this;        

        for (int i = 0; i < StartMapSize; i++)
            CreatePath(true);        
    }


    void Update()
    {
        double time = DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
        int delta = (int) (time - currentTime);
        float k = (delta / 1000f) / deltaTime;

        if (isStopMoving)
        {
            if (k >= 1)
            {                
                isStopMoving = false;
                currentMapSpeed = 0;
            }
            else
                currentMapSpeed = updateMapSpeed * (1 - k);
        }
        if (isStartMoving)
        {
            if (k >= 1)
            {
                isStartMoving = false;
                currentMapSpeed = updateMapSpeed;
            }
            else
                currentMapSpeed = updateMapSpeed * k;
        }
        if (isMoving)
        {
            currentXPosition += updateHorizontalSpeed * (idToPosition - idPosition) * Time.deltaTime;
            if ((idToPosition - idPosition > 0) && (currentXPosition >= positionsPlayer[idToPosition]) ||
                (idToPosition - idPosition < 0) && (currentXPosition <= positionsPlayer[idToPosition]))
            {
                isMoving = false;
                currentXPosition = positionsPlayer[idToPosition];
                idPosition = idToPosition;
            }
        }
    }


    // Create new path in map
    public void CreatePath(bool isFirst)
    {
        int id = rnd.Next(0, pathList.Length);
        while (id == lastType || id == 2 && lastType != 3 || isFirst && id == 5 || id == 3 && lastType == 5)
            id = rnd.Next(0, pathList.Length);
        id = (lastType == 3) ? 2 : id;
        lastType = id;

        lastPath = Instantiate(pathList[id], new Vector3(0, 0, (lastPath != null) ? (lastPath.transform.position.z + 20) : 0), Quaternion.identity);
    }


    public void Run()
    {
        currentMapSpeed = updateMapSpeed;        
        idToPosition = 0;
        idPosition = 1;
    }


    // Return speed of movement map
    public float Speed()
    {
        return currentMapSpeed;
    }


    // Return X position map
    public float HorizontalPosition()
    {
        return currentXPosition;
    }


    // Player stops immediately (moving map)
    public void StopMoving()
    {
        currentMapSpeed = 0;
        isStopMoving = false;
        isStartMoving = false;
        isMoving = false;
    }


    // Player stops gradually (moving map)
    public void StopMoving(float deltaTime)
    {
        currentTime = DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
        this.deltaTime = deltaTime;
        isStopMoving = true;
    }


    // Player starts moving immediately (moving map)
    public void StartMoving()
    {
        currentMapSpeed = updateMapSpeed;
    }


    // Player starts moving gradually (moving map)
    public void StartMoving(float deltaTime)
    {
        currentTime = DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
        this.deltaTime = deltaTime;
        isStartMoving = true;
    }


    // Player moves to right (moving map)
    public void RightMoving()
    {
        if (idPosition > 0)
        {
            idToPosition = idPosition - 1;
            isMoving = true;
        }
    }


    // Player moves to left (moving map)
    public void LeftMoving()
    {
        if (idPosition < positionsPlayer.Length - 1)
        {
            idToPosition = idPosition + 1;
            isMoving = true;
        }
    }


    // Return instance object of class
    public static Pathways Instance()
    {
        return _instance;
    }
}