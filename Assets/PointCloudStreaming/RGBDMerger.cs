using RosSharp.RosBridgeClient;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Linq;
using UnityEngine;
using OpenCvSharp;

public class RGBDMerger : MonoBehaviour
{
    public ImageSubscriber rgbImageSub;
    public DepthImageSubscriber depthImageSub;

    RosSharp.RosBridgeClient.MessageTypes.Sensor.Image rgbImage;
    RosSharp.RosBridgeClient.MessageTypes.Sensor.Image depthImage;

    Mat rgb_img, depth_img;
    Mat rgb_buff, depth_buff;

    public float fx, fy, cx, cy;
    public int height_start = 0, height_end = 480, width_start = 0, width_end = 640;
    public int depth_limit = 3000;
    int i, j, k;

    float x, y, z, r, g, b;

    private List<Vector3> pcl = new List<Vector3>();
    private List<Color> pcl_color = new List<Color>();

    private Vector3 tmp_vector3;
    private Color tmp_color;

    // Mat rgb_mat_3 = new Mat<Vec3b>(640, 480);

    // string rgb_path = "C:\\rgb.png";
    // string depth_path = "C:\\depth.png";

    // Start is called before the first frame update
    void Start()
    {
        rgbImage = new RosSharp.RosBridgeClient.MessageTypes.Sensor.Image();
        depthImage = new RosSharp.RosBridgeClient.MessageTypes.Sensor.Image();
    }

    // Update is called once per frame
    void Update()
    {
        // Make sure both images have been received
        if (rgbImageSub.ImageData != null && depthImageSub.ImageData != null)
        {
            DecompressImages();
        }
    }

    protected void DecompressRGB()
    {
        rgb_buff = new Mat (1, rgbImageSub.ImageData.Length, MatType.CV_8UC1, rgbImageSub.ImageData);
        rgb_img = Cv2.ImDecode(rgb_buff , ImreadModes.Color);
        // Cv2.ImWrite(rgb_path, rgb_img);
        // Vec3b color = rgb_img.Get<Vec3b>(240, 320);
    }

    protected void DecompressDepth()
    {
        depth_buff = new Mat (1, depthImageSub.ImageData.Length, MatType.CV_8UC1, depthImageSub.ImageData);
        depth_img = Cv2.ImDecode(depth_buff , ImreadModes.Unchanged);
        depth_img.ConvertTo(depth_img, MatType.CV_16UC1);
        // Cv2.ImWrite(depth_path, depth_img);
        // ushort depth = depth_img.Get<ushort>(240, 320);
    }

    protected void DecompressImages()
    {
        DecompressRGB();
        DecompressDepth();
        PointCloudRendering();
    }

    void PointCloudRendering()
    {
        pcl.Clear();
        pcl_color.Clear();

        // rgb_img /= 255.0f;
        // depth_img *= 0.001f;

        Mat<Vec3b> rgb_mat = new Mat<Vec3b>(rgb_img);
        MatIndexer<Vec3b> rgb_indexer = rgb_mat.GetIndexer();
        Mat<ushort> depth_mat = new Mat<ushort>(depth_img);
        MatIndexer<ushort> depth_indexer = depth_mat.GetIndexer();
        
        foreach (int j in Enumerable.Range(height_start, height_end).Where(number => number % 3 == 0)/*= 0; j < height; j+=2 */ /*j+=batch_size*/)
        {
            foreach (int k in Enumerable.Range(width_start, width_end).Where(number => number % 3 == 0)/*= 0; k < width; k+=2 */ /*k+=batch_size*/)
            {
                // Debug.Log("Sync Success " + depth_indexer[j, k]);
                // if(depth_indexer[j, k] < depth_limit)
                // {
                //     PointCloudComputing(depth_indexer[j, k], k, j, 
                //         (float)rgb_indexer[j, k][2]/255.0f, (float)rgb_indexer[j, k][1]/255.0f, (float)rgb_indexer[j, k][0]/255.0f);
                // }
                if(depth_indexer[j, k] < depth_limit)
                {
                    z = depth_indexer[j, k] * 0.001f;
                    x = (k - cx) / fx * z;
                    y = (j - cy) / fy * z;

                    r = (float)rgb_indexer[j, k][2]/255.0f;
                    g = (float)rgb_indexer[j, k][1]/255.0f;
                    b = (float)rgb_indexer[j, k][0]/255.0f;

                    tmp_vector3 = new Vector3(x, z, y);
                    tmp_color = new Color(r, g, b);

                    pcl.Add(tmp_vector3);
                    pcl.Add(tmp_vector3);
                    pcl.Add(tmp_vector3);
                    pcl.Add(tmp_vector3);

                    pcl_color.Add(tmp_color);
                    pcl_color.Add(tmp_color);
                    pcl_color.Add(tmp_color);
                    pcl_color.Add(tmp_color);
                }
            }
        }
    }

    public Vector3[] GetPCL(int index)
    {
        return pcl.ToArray();
    }

    public Color[] GetPCLColor(int index)
    {
        return pcl_color.ToArray();
    }

    // void PointCloudComputing(float z, int u , int v, float r, float g, float b)
    // {
    //     z = z * 0.001f;
    //     x = (u - cx) / fx * z;
    //     y = (v - cy) / fy * z;

    //     tmp_vector3 = new Vector3(x, z, y);
    //     tmp_color = new Color(r, g, b);

    //     pcl.Add(tmp_vector3);
    //     pcl.Add(tmp_vector3);
    //     pcl.Add(tmp_vector3);
    //     pcl.Add(tmp_vector3);

    //     pcl_color.Add(tmp_color);
    //     pcl_color.Add(tmp_color);
    //     pcl_color.Add(tmp_color);
    //     pcl_color.Add(tmp_color);
    // }
}
