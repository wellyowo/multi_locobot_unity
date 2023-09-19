/*
© Siemens AG, 2017-2018
Author: Dr. Martin Bischoff (martin.bischoff@siemens.com)

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at
<http://www.apache.org/licenses/LICENSE-2.0>.
Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class TwistPublisher_basecontrol : UnityPublisher<MessageTypes.Geometry.Twist>
    {

        private MessageTypes.Geometry.Twist message;
        private float previousRealTime;        
        private Vector3 previousPosition = Vector3.zero;
        private Quaternion previousRotation = Quaternion.identity;

        public float linear_scale = 1;
        public float angular_scale = 1;
        public ControllersManager controllerInput;
        private Vector2 Primary2DAxis;
        protected override void Start()
        {
            base.Start();
            InitializeMessage();
        }

        private void FixedUpdate()
        {
            UpdateMessage();
        }

        private void InitializeMessage()
        {
            message = new MessageTypes.Geometry.Twist();
            message.linear = new MessageTypes.Geometry.Vector3();
            message.angular = new MessageTypes.Geometry.Vector3();
        }
        private void UpdateMessage()
        {
           
            Primary2DAxis = controllerInput.GetComponent<ControllersManager>().getRightPrimary2DAxis();
            message.linear.x = Primary2DAxis.y * linear_scale;
            message.angular.z = -Primary2DAxis.x * angular_scale;

            Publish(message);
        }

    }
}
