/*
© CentraleSupelec, 2017
Author: Dr. Jeremy Fix (jeremy.fix@centralesupelec.fr)

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

// Adjustments to new Publication Timing and Execution Framework
// © Siemens AG, 2018, Dr. Martin Bischoff (martin.bischoff@siemens.com)

using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace RosSharp.RosBridgeClient
{
    public class Latency_TwistStampedSubscriber : UnitySubscriber<MessageTypes.Geometry.TwistStamped>
    {
        public Transform SubscribedTransform;
        public TMPro.TextMeshPro text_obj;
        public string filename ;
        public ControllersManager controllerInput;



        private double latency, latency_count = 0;
        private uint count, i = 0;
        private int counter = 0 ;
        private List<string> myList = new List<string>() {};
        private MessageTypes.Geometry.TwistStamped local_TwistStamped = new MessageTypes.Geometry.TwistStamped();
        private bool isMessageReceived;
        string formatString = "{0:G" + 5 + "}\t{1:G" + 5 + "}";
        private string time = "";

        private float leftTriggerValue;
        private bool trigger_pressed = false, trigger_released = true;
        




        protected override void Start()
        {
            time = System.DateTime.Now.ToString("HH_mm_ss");
            // filename = Application.dataPath + "/Latency/" + filename+"_"+ time+ ".csv";
            filename = System.IO.Directory.GetCurrentDirectory() + filename+"_"+ time+ ".csv";
            base.Start();

        }

        protected override void ReceiveMessage(MessageTypes.Geometry.TwistStamped message)
        {

            local_TwistStamped.header.Update();
            // Debug.Log("last " + message.header.stamp.secs + "." + message.header.stamp.nsecs + " now " + message_TwistStamped.header.stamp.secs + "." + message_TwistStamped.header.stamp.nsecs);
            if(message.header.stamp.secs == local_TwistStamped.header.stamp.secs)
            {
                latency = (local_TwistStamped.header.stamp.nsecs - message.header.stamp.nsecs) / 1000000.0D;
            }
            else
            {
                latency = (local_TwistStamped.header.stamp.secs - message.header.stamp.secs) * 1000.0D - (message.header.stamp.nsecs / 1000000.0D) + (local_TwistStamped.header.stamp.nsecs / 1000000.0D);
            }
            // Debug.Log("latency: " + latency + " " + local_TwistStamped.header.stamp.secs + " " + message.header.stamp.secs);
            
            ++count;
            latency_count += latency;
            i = message.header.seq;
            isMessageReceived = true;
            string time_now = System.DateTime.Now.ToString("HH_mm_ss.fff");
            myList.Add(time_now.Substring(0,2) + ',' + time_now.Substring(3,2) + ',' + time_now.Substring(6,6) + ',' + message.header.seq.ToString() + ',' + latency.ToString());
            
       
        }

        private void Update()
        {
            if (isMessageReceived)
                ProcessMessage();

            while(count > 20)
            {
                latency_count /= count;
                text_obj.text = "RTT : " + latency_count.ToString("f1") + " ms";
                
                //StreamWriter writer = new StreamWriter("Assets/Resources/eth_to_eth_ctl_FloatArray.txt", true);
                //writer.WriteLine(formatString, i, latency);
                //writer.Close();
                count = 0;
                latency_count = 0;
            }

            leftTriggerValue = controllerInput.GetComponent<ControllersManager>().getLeftTrigger();
            if(leftTriggerValue > 0.7f && trigger_released == true)
            {
                //Debug.Log("Trigger pressed");
                trigger_pressed = true;
                trigger_released = false;
            }
            if(leftTriggerValue < 0.7f && trigger_pressed == true)
            {
                //Debug.Log("Trigger released");
                trigger_pressed = false;
                trigger_released = true;
                counter +=1;
            }
            if(counter == 1)
            {
                counter  = 2;
                StreamWriter writer = new StreamWriter(filename, false);
                writer.WriteLine("Hr,Min,Sec,Seq,RawData");
                foreach(string number in myList){
                    writer.WriteLine(number);
                }
                Debug.Log("finish");
                Debug.Log("Application ending after " + Time.time + " seconds");
                writer.Close();
                myList.Clear();


            }
        }


        private void ProcessMessage()
        {
            isMessageReceived = false;
        }




        //only works in unity editor
        private void OnApplicationQuit()
        {
            StreamWriter writer = new StreamWriter(filename, false);
            writer.WriteLine("Hr,Min,Sec,Seq,RawData");
            foreach(string number in myList){
                writer.WriteLine(number);
            }
            Debug.Log("finish");
            Debug.Log("Application ending after " + Time.time + " seconds");
            writer.Close();
            myList.Clear();

        }
            
        

        
    }
}