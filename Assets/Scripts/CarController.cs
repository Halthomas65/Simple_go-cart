using UnityEngine;
using System.Collections;

public class CarController : MonoBehaviour
{
    public WheelCollider WheelFL;
    public WheelCollider WheelFR;
    public WheelCollider WheelRL;
    public WheelCollider WheelRR;
    public Transform WheelFLTrans;
    public Transform WheelFRTrans;
    public Transform WheelRLTrans;
    public Transform WheelRRTrans;
    public float steeringAngle = 45;
    public float maxTorque = 1000;
    public float maxBrakeTorque = 500;
    public Transform centerOfMass;

    float gravity = 9.8f;
    bool braked = false;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass.transform.localPosition;
    }

    void FixedUpdate()
    {
        if (!braked)
        {
            WheelFL.brakeTorque = 0;
            WheelFR.brakeTorque = 0;
            WheelRL.brakeTorque = 0;
            WheelRR.brakeTorque = 0;
        }
        //Speed of car, Car will move as you will provide the input to it.

        WheelRR.motorTorque = maxTorque * Input.GetAxis("Vertical");
        WheelRL.motorTorque = maxTorque * Input.GetAxis("Vertical");

        //Changing car direction
        //Here we are changing the steer angle of the front tyres of the car so that we can change the car direction.
        WheelFL.steerAngle = steeringAngle * Input.GetAxis("Horizontal");
        WheelFR.steerAngle = steeringAngle * Input.GetAxis("Horizontal");
    }
    void Update()
    {
        HandBrake();

        //For tyre rotate
        WheelFLTrans.Rotate(WheelFL.rpm / 60 * 360 * Time.deltaTime, 0, 0);
        WheelFRTrans.Rotate(WheelFR.rpm / 60 * 360 * Time.deltaTime, 0, 0);
        WheelRLTrans.Rotate(WheelRL.rpm / 60 * 360 * Time.deltaTime, 0, 0);
        WheelRRTrans.Rotate(WheelRL.rpm / 60 * 360 * Time.deltaTime, 0, 0);
        //Changing tyre direction
        Vector3 temp = WheelFLTrans.localEulerAngles;
        Vector3 temp1 = WheelFRTrans.localEulerAngles;
        temp.y = WheelFL.steerAngle - (WheelFLTrans.localEulerAngles.z);
        WheelFLTrans.localEulerAngles = temp;
        temp1.y = WheelFR.steerAngle - WheelFRTrans.localEulerAngles.z;
        WheelFRTrans.localEulerAngles = temp1;
    }
    void HandBrake()
    {
        //Debug.Log("brakes " + braked);
        if (Input.GetButton("Jump"))
        {
            braked = true;
        }
        else
        {
            braked = false;
        }
        if (braked)
        {

            WheelRL.brakeTorque = maxBrakeTorque * 20;//0000;
            WheelRR.brakeTorque = maxBrakeTorque * 20;//0000;
            WheelRL.motorTorque = 0;
            WheelRR.motorTorque = 0;
        }
    }
}