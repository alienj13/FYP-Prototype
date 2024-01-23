using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public static List<Group> Groups = new List<Group>();
   
    byte[] buffer = new byte[1024];
    UdpClient listener;
    IPEndPoint groupEP;
    public  bool load = false;
    public  bool play = false;
    public GameObject objectPrefab;
    public Material on;
    public Material off;
    public static Main Instance { get; private set; }

    void Awake()
    {
     
        Instance = this;
    }
    public GameObject create(int pos,int i, int j,int count)
    {
       int posnew = pos *1200;
        Vector3 position;
      
        
            position = new Vector3((j * 100 + posnew), i * 100, 100);
        
        Debug.Log(pos);
        // Instantiate the object at the calculated position
        objectPrefab = Instantiate(objectPrefab, position, Quaternion.identity);

        return objectPrefab;
    }



    // Start is called before the first frame update
    void Start()
    {



    }

    // Update is called once per frame
    void Update()
    {
        
        if (load && play && Groups[0].getDataLength() > 0) {
            foreach (Group g in Groups) // Assuming Groups is a List<Group> defined elsewhere
            {

                g.Start();
            }

        }
       
           
    }

   

    public void OnButtonPress()
    {
        ReadXML();
    }

    public void OnButtonPress2()
    {
        play = true;

       
    }



    private void ReadXML()
    {
        string filePath = "C:/Users/junai/eclipse-workspace/FYP/data/Test.iqr"; // Update with the correct path

        try
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);

            XmlNodeList groupNodes = xmlDoc.GetElementsByTagName("Group");
            int pos = 0;
            foreach (XmlNode groupNode in groupNodes)
            {
                XmlElement groupElement = (XmlElement)groupNode;

                Debug.Log("Group: ");
                Debug.Log("    name: " + groupElement.GetAttribute("name"));
                Debug.Log("    ID: " + groupElement.GetAttribute("id"));
                string Gid = groupElement.GetAttribute("id");
                string Gname = groupElement.GetAttribute("name");

                XmlElement topologyRect = (XmlElement)groupElement.GetElementsByTagName("TopologyRect")[0];
                Debug.Log("    Neuron X: " + topologyRect.GetAttribute("hcount"));
                Debug.Log("    Neuron Y: " + topologyRect.GetAttribute("vcount"));
                int xCount = int.Parse(topologyRect.GetAttribute("hcount"));
                string yCount = topologyRect.GetAttribute("vcount");

                XmlElement neurons = (XmlElement)groupElement.GetElementsByTagName("Neuron")[0];
                Debug.Log("    Neuron name: " + neurons.GetAttribute("name"));

                Groups.Add(new Group(Gid, Gname, xCount, int.Parse(yCount),pos));
                pos++;
            }
            load = true;
        }
        catch (XmlException xmlEx)
        {
            Debug.LogError("XML Exception: " + xmlEx.Message);
        }
        
    }


    public void SortData(string data)
    {
        List<double> tempData = new List<double>();

        string[] pairs = data.Split(';');
      
        for(int i = 0;i < pairs.Length - 1;i++)
        {
            string[] parts = pairs[i].Split(':');
            string id = parts[0];
            
            double value = double.Parse(parts[1]);
            if (value > 1)
            {
                value = 1;
            }
            foreach (Group g in Groups) // Assuming Groups is a List<Group> defined elsewhere
            {
                if (g.getGroupId().Equals(id)) // Assuming Group class has a GroupId property
                {
                    g.addData(value); // Assuming Group class has an AddData method
                }
            }
        }
    }

    public void Turnon(GameObject o)
    {
        MeshRenderer renderer = o.GetComponent<MeshRenderer>();

        if (renderer != null)
        {
            // Create a new instance of the material
            Material instanceMaterial = new Material(on);

            // Assign the new material to the renderer's material, not the sharedMaterial
            renderer.material = instanceMaterial;
        }
    }

    public void Turnoff(GameObject[,]o,int xcount,int ycount)
    {

        for (int i = 0; i < xcount; i++)
        {
            for (int j = 0; j < ycount; j++)
            {
                MeshRenderer renderer = o[i, j].GetComponent<MeshRenderer>();
                if (renderer != null)
                {
                    // Create a new instance of the material
                    Material instanceMaterial = new Material(off);

                    // Assign the new material to the renderer's material, not the sharedMaterial
                    renderer.material = instanceMaterial;
                }
            }
        }
    }

}
