using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class HeadtiltPublisher : UnityPublisher<MessageTypes.Std.Float64>
    {
        private MessageTypes.Std.Float64 message;      
        public ControllersManager controllerInput; 
        public float[] pose = new float[2];
        
        
        private bool leftsecondarybuttonValue;
        private bool button_pressed = false;
        private int counter = 0;

        public GameObject Mid_pc;
        private Vector3 init_top_pc_position;
        private Quaternion init_top_pc_rotation;

        public RGBDMerger RGBD_Merger;
        private int init_depth_limit;
        
        public int navi_mode_depth_limit = 3000; 

        protected override void Start()
        {
            base.Start();
            InitializeMessage();
            init_top_pc_position = Mid_pc.transform.position;
            init_top_pc_rotation = Mid_pc.transform.rotation;
            init_depth_limit = RGBD_Merger.depth_limit;
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
                        Mid_pc.transform.rotation = init_top_pc_rotation;
                        RGBD_Merger.depth_limit = init_depth_limit;
                        Debug.Log( RGBD_Merger.depth_limit);
                        break;

                    case 1:
                        UpdateMessage(pose[1]);
                        Debug.Log(init_top_pc_rotation.eulerAngles);
                        Mid_pc.transform.rotation = Quaternion.AngleAxis(90f, Vector3.right);
                        RGBD_Merger.depth_limit = navi_mode_depth_limit;
                        Debug.Log( RGBD_Merger.depth_limit);
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