using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class Float64Publisher : UnityPublisher<MessageTypes.Std.Float64>
    {
        private MessageTypes.Std.Float64 message;      
        public ControllersManager controllerInput; 
        public float[] pose = new float[2];
        
        
        private bool leftsecondarybuttonValue;
        private bool button_pressed = false;
        private int counter = 0;

        protected override void Start()
        {
            base.Start();
            InitializeMessage();
        }

        private void FixedUpdate()
        {
            leftsecondarybuttonValue = controllerInput.GetComponent<ControllersManager>().getLeftSecondaryButton();
            if(leftsecondarybuttonValue == true)
            {
                button_pressed = true;
            
            }
            if(leftsecondarybuttonValue == false && button_pressed == true)
            {
                button_pressed = false;
                counter +=1;
                switch(counter %= 2)
                {
                    case 0:
                        UpdateMessage(pose[0]);
                        break;

                    case 1:
                        UpdateMessage(pose[1]);
                        break;
            
                }
                
            }
            
        }

        private void InitializeMessage()
        {
            message = new MessageTypes.Std.Float64();
        }
        private void UpdateMessage(float data)
        {
            message.data = data;
            

            Publish(message);
        }
    }

       
}