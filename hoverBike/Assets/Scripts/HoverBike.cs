using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverBike : MonoBehaviour
{

    // Andony Escudero : Adjunct Instructor for Oklahoma Christion University, for Intro to Game Technology students.
    // Contact for aditional information at Andony.escudero@gmail.com

    // UI fields

    [SerializeField]
    private Slider fuel;

    [SerializeField]
    private Image fuelColor;


    // Ridgidbody fields

    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private GameObject CM;


    // Object references for engine locations

    [SerializeField]
    private List<GameObject> engines;

    [SerializeField]
    private GameObject propulation;


    // Particle Fields

    [SerializeField]
    private GameObject engineParticle;

    [SerializeField]
    private GameObject extreemParticle;

    // Value Fields

    [SerializeField]
    private float speed = 500;

    [SerializeField]
    private float extreem = 2000;

    [SerializeField]
    private float turnspeed = 300;

    [SerializeField]
    private float hoverForce = 300;

    // Timers;

    [SerializeField]
    private float extreemTime = 5;

    private float refuelTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb.centerOfMass = CM.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {

        // Timer logic for the speed boost, speed boost UI slider values as well as colors are modified here
        if(extreemTime < 0)
        {
            refuelTime += Time.deltaTime;
            fuel.value = refuelTime;
            fuelColor.color = Color.red;
        }

        if(refuelTime >= 5f)
        {
            refuelTime = 0;
            extreemTime = 5;
            fuelColor.color = Color.green;

        }


        // Movement logic, first if is for the speed boos with the else being for normal movement based on Axis "Vertical" set up in player settings

        if (Input.GetKey(KeyCode.LeftShift) && extreemTime > 0 && Input.GetAxis("Vertical") > 0)
        {
            rb.AddForceAtPosition(Time.deltaTime * transform.TransformDirection(Vector3.forward) * Input.GetAxis("Vertical") * extreem, propulation.transform.position);
            extreemParticle.SetActive(true);
            extreemTime -= Time.deltaTime;
            fuel.value = extreemTime;
        }
        else
        {
            if(Input.GetAxis("Vertical") > 0)
            {
                rb.AddForceAtPosition(Time.deltaTime * transform.TransformDirection(Vector3.forward) * Input.GetAxis("Vertical") * speed, propulation.transform.position);
                extreemParticle.SetActive(false);
            }
            else
            {
                // go slower if going backwards

                rb.AddForceAtPosition(Time.deltaTime * transform.TransformDirection(Vector3.forward) * Input.GetAxis("Vertical") * (speed /2), propulation.transform.position);
                extreemParticle.SetActive(false);
            }  

        }

        //Show normal speed particles if going forward but not reciving input for speed boosts

        if(Input.GetAxis("Vertical") > 0 && (Input.GetKey(KeyCode.LeftShift) == false || extreemTime < 0))
        {
            engineParticle.SetActive(true);
        }
        else
        {
            engineParticle.SetActive(false);

        }

        //Engine for hovering Logic based on each engine

        foreach (GameObject engine in engines)
        {
            RaycastHit hit;

            if (Physics.Raycast(engine.transform.position, transform.TransformDirection(Vector3.down), out hit, 3f))
            {
                rb.AddForceAtPosition(Time.deltaTime * transform.TransformDirection(Vector3.up) * Mathf.Pow( 3f - hit.distance, 2) / 2f * hoverForce, engine.transform.position, ForceMode.Acceleration);
            }

        }

        // Turning logic based on local rotation 
        rb.AddTorque(Time.deltaTime * transform.TransformDirection(Vector3.up) * Input.GetAxis("Horizontal") * turnspeed);


        //rb.AddForce(-Time.deltaTime * transform.TransformVector(Vector3.right) * transform.InverseTransformVector(rb.velocity).x * 5f);
    }
}
