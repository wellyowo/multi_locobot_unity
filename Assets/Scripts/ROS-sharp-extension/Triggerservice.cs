using UnityEngine;
using RosSharp.RosBridgeClient;
using std_srvs = RosSharp.RosBridgeClient.MessageTypes.Std;


namespace RosSharp.RosBridgeClient
{
    public class Triggerservice : UnityServiceClient<std_srvs.TriggerRequest, std_srvs.TriggerResponse>
    {
        

        private MessageTypes.Sensor.JointState message;    
        private float leftTriggerValue;
        private bool trigger_pressed = false, trigger_released = true;
        public ControllersManager controllerInput;
        protected override void Start()
        {
            base.Start();
        }

        private void FixedUpdate()
        {
            leftTriggerValue = controllerInput.GetComponent<ControllersManager>().getLeftTrigger();
            if(leftTriggerValue > 0.7f && trigger_released == true)
            {
                trigger_pressed = true;
                trigger_released = false;
            }
            if(leftTriggerValue < 0.7f && trigger_pressed == true)
            {
                trigger_pressed = false;
                trigger_released = true;
                CallService(ServiceCallHandler, new std_srvs.TriggerRequest());
            }
        }
        private void ServiceCallHandler(std_srvs.TriggerResponse message)
        {
            Debug.Log("Service status" + message.success);
        }


    }
}