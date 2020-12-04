

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mapbox.Unity.Map;
using Mapbox.Examples;


public class CarManipulator : MonoBehaviour
{
    private GameObject newcar, newcam;
    public GameObject car, cam;
    public Text currentCar;
    public GameObject[] cars, cams;
    public Transform parent;
    public int i = 0;
    public AbstractMap map;


    [SerializeField]
    private Transform player;
    [SerializeField]
    private float max_distance;
    [SerializeField]
    private float maxAngel;

    Vector3 distance_vector;

    [HideInInspector]
    public bool carInRange = false;

    bool carInStopZone = false;

    [SerializeField]
    private WaitForSeconds delay;

    [SerializeField]
    private float stop_radius;
   

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Car").transform;
        delay = new WaitForSeconds(0.05f);

        StartCoroutine("stop");
    }

    // Update is called once per frame
    void Update()
    {
        cars = GameObject.FindGameObjectsWithTag("Car");//to make the script knows  car's gameobject
        cams = GameObject.FindGameObjectsWithTag("MainCamera");//to make the script knows cam's gameobject
        currentCar.text = "Car: " + (i + 0).ToString();

        Disable(i);



        CheckForCar();

        if (carInRange)
        {
            if (Vector3.Distance(player.position, transform.position) < stop_radius)
            {
                carInStopZone = true;
                
            }

        }
    }


    void CheckForCar()
    {
        distance_vector = transform.position - player.position;
        float distance = distance_vector.magnitude;


        if (distance < max_distance)
        {


            float angel = Vector3.Angle(this.transform.forward, distance_vector);

            if (angel < maxAngel)
            {
                RaycastHit hit;
                Ray ray = new Ray(transform.position, new Vector3(distance_vector.x, .2f, distance_vector.z));

                if (Physics.Raycast(ray, out hit))
                {

                    Debug.DrawRay(ray.origin, ray.direction);
                    {
                        if (hit.transform.gameObject.tag != "Car")
                        {
                            carInRange = true;

                        }
                    }

                }


            }

        }
        else
        {
            carInRange = false;
        }

        if (carInRange)
        {
            Stop();
        }

    }

    void Stop()
    {

        if (carInStopZone)
        {
            Debug.Log("Stop");
           this.GetComponent<AstronautMouseController>().characterSpeed = 0;
           // WaitForSeconds(delay) ;
            this.GetComponent<AstronautMouseController>().characterSpeed = 5;
            

        }

    }




    public void AddCar()
    {

        newcar = Instantiate(car, new Vector3(0, 0, 0), Quaternion.identity);
        newcar.transform.parent = this.transform;// place all added cars in the cars' empty object while adding 
        newcar.name = "Car" + cars.Length.ToString();
        newcam = Instantiate(cam, cam.transform.position, cam.transform.rotation);
        newcam.transform.parent = parent;
        newcam.name = "Cam" + cams.Length.ToString();
        newcam.GetComponent<Camera>().enabled = false;
        newcar.GetComponent<AstronautMouseController>().enabled = false; ;
        newcar.GetComponent<AstronautMouseController>().map = map;
        newcam.GetComponent<AstronautMouseController>().cam = newcam.GetComponent<Camera>();
        newcar.GetComponent<CharacterMovement>().enabled = false;
        newcar.GetComponent<AstronautDirections>().enabled = false;

    }
    public void Right()
    {
        if (i < cars.Length - 1)
        {
            i++;
        }
        else
        {
            i = 0;
        }
    }
    public void Left()
    {
        if (i > 0)
        {
            i--;
        }
        else
        {
            i = cars.Length - 1;
        }
    }
    public void Disable(int x)
    {
        cams[x].GetComponent<Camera>().enabled = true;
        cars[x].GetComponent<AstronautMouseController>().enabled = true;
        newcar.GetComponent<CharacterMovement>().enabled = true;
        newcar.GetComponent<AstronautDirections>().enabled = true;
        cars[x].GetComponent<AstronautMouseController>().cam = cams[x].GetComponent<Camera>();//to assign cam to the current car 

        for (int j = 0; j < cars.Length; j++)
        {

            if (x != j)
            {
                cams[j].GetComponent<Camera>().enabled = false;
                cars[j].GetComponent<AstronautMouseController>().enabled = false;
                cars[j].GetComponent<CharacterMovement>().enabled = false;
                cars[j].GetComponent<AstronautDirections>().enabled = false;

            }
        }
    }
   
}