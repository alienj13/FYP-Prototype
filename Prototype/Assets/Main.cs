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
    public bool load = false;
    public bool play = false;
    public GameObject objectPrefab;
    public GameObject cylinderPrefab;
    public GameObject dendritePrefab;
    public Material on;
    public Material off;
    public static Main Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    public Vector3 GetCubePosition(int pos, float cubeSize, float cubeHeight)
{
    if (pos == 0)
        return new Vector3(0, 0, 0); // Base cube position

    int level = 1;
    int totalCubesInPreviousLevels = 1;
    int cubesInCurrentLevel = (level + 1) * (level + 1);

    // Find the level where the cube belongs
    while (pos >= totalCubesInPreviousLevels + cubesInCurrentLevel)
    {
        totalCubesInPreviousLevels += cubesInCurrentLevel;
        level++;
        cubesInCurrentLevel = (level + 1) * (level + 1);
    }

    // Calculate the position within the current level
    int posInCurrentLevel = pos - totalCubesInPreviousLevels;
    int rowInCurrentLevel = posInCurrentLevel / (level + 1);
    int colInCurrentLevel = posInCurrentLevel % (level + 1);

    // Calculate actual position
    float x = (colInCurrentLevel - level / 2f) * cubeSize;
    float y = level * cubeHeight; // Negative because it's an upside-down pyramid
    float z = (rowInCurrentLevel - level / 2f) * cubeSize;

    return new Vector3(x, y, z);
}

    public GameObject create(int pos, int i, int j, int rows, int cols)
    {
        int posnew = pos * 1500;
       
        //Convert 2D array indices to spherical coordinates
        //float theta = (i / (float)(rows - 1)) * Mathf.PI; // 0 to PI (top to bottom)
        float phi = (j / (float)(cols - 1)) * 2 * Mathf.PI; // 0 to 2PI (around the sphere)

    // Calculate a contracted theta value that avoids the poles
float poleOffset = 0.9f; // This offset will prevent theta from being 0 or PI exactly, avoiding the poles
float theta = ((i + poleOffset) / ((float)(rows - 1) + (2 * poleOffset))) * Mathf.PI;

        Vector3 v = GetCubePosition(pos,2000,2000);
        
        // Convert spherical coordinates to Cartesian coordinates for the neuron's position
        Vector3 position;

        if (pos == 0)
        {
            position = new Vector3(
                500.0f * Mathf.Sin(theta) * Mathf.Cos(phi),
                500.0f * Mathf.Cos(theta),
                500.0f * Mathf.Sin(theta) * Mathf.Sin(phi) 
            );
        }else{
            position = new Vector3(
                500.0f * Mathf.Sin(theta) * Mathf.Cos(phi),
                500.0f * Mathf.Cos(theta),
                500.0f * Mathf.Sin(theta) * Mathf.Sin(phi) 
            );
        }

         
            objectPrefab = Instantiate(objectPrefab, position+v, Quaternion.identity, transform);

            MeshRenderer renderer = objectPrefab.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                Material instanceMaterial = new Material(off);
                renderer.material = instanceMaterial;
            }

            return objectPrefab;
        }


    public GameObject CreateConnection(int sourcePos, int targetPos)
{
    Vector3 sourcePosition = GetCubePosition(sourcePos,2000,2000);
    Vector3 targetPosition = GetCubePosition(targetPos,2000,2000);
    // Calculate the midpoint
    Vector3 midpoint = (sourcePosition + targetPosition) / 2f;

    // Instantiate the cylinder at the midpoint
    GameObject cylinder = GameObject.Instantiate(cylinderPrefab, midpoint, Quaternion.identity);

    // Scale the cylinder
    float distance = Vector3.Distance(sourcePosition, targetPosition);
    cylinder.transform.localScale = new Vector3(cylinder.transform.localScale.x/2, distance / 2f, cylinder.transform.localScale.z);

    // Orient the cylinder
    cylinder.transform.up = targetPosition - sourcePosition;

    return cylinder;
}

 public GameObject Createdendrite(GameObject n, int targetPos)
{
    Vector3 sourcePosition = n.transform.position;
    Vector3 targetPosition = GetCubePosition(targetPos,2000,2000);
    // Calculate the midpoint
    Vector3 midpoint = (sourcePosition + targetPosition) / 2f;

    // Instantiate the cylinder at the midpoint
    GameObject dendrite = GameObject.Instantiate(dendritePrefab, midpoint, Quaternion.identity);

    // Scale the cylinder
    float distance = Vector3.Distance(sourcePosition, targetPosition);
    dendrite.transform.localScale = new Vector3(dendrite.transform.localScale.x/2, distance / 2f, dendrite.transform.localScale.z);

    // Orient the cylinder
    dendrite.transform.up = targetPosition - sourcePosition;

    return dendrite;
}

        void Update()
        {
            if (load && play && Groups[0].getDataLength() > 0)
            {
                foreach (Group g in Groups)
                {
                    g.Start();
                }
            }
        }

        public void OnButtonPress() { ReadXML(); }

        public void OnButtonPress2() {if(load) {play = !play;} }

        private void ReadXML()
        {
            string filePath = "C:/Users/junai/eclipse-workspace/FYP/data/Test.iqr";

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

                    Groups.Add(new Group(Gid, Gname, xCount, int.Parse(yCount), pos));
                    pos++;
                }

                groupNodes = xmlDoc.GetElementsByTagName("Connection");

                foreach (XmlNode groupNode in groupNodes)
                {
                    XmlElement groupElement = (XmlElement)groupNode;

                    Debug.Log("    ID: " + groupElement.GetAttribute("id"));
                    string id = groupElement.GetAttribute("id");

                    Debug.Log("    type: " + groupElement.GetAttribute("type"));
                    string type = groupElement.GetAttribute("type");
                    Debug.Log("    source: " + groupElement.GetAttribute("source"));
                    Debug.Log("    target: " + groupElement.GetAttribute("target"));
                    Group[] TS = findGroups(groupElement.GetAttribute("source"), groupElement.GetAttribute("target"));

                    Synapse s = new Synapse(id, type, TS[0], TS[1]);
                    TS[0].addConnection(s);
                    TS[1].addConnection(s);

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

            for (int i = 0; i < pairs.Length - 1; i++)
            {
                string[] parts = pairs[i].Split(':');
                string id = parts[0];

                double value = double.Parse(parts[1]);
                if (value > 1)
                {
                    value = 1;
                }
                foreach (Group g in Groups)
                {
                    if (g.getID().Equals(id))
                    {
                        g.addData(value);
                    }
                }
            }
        }

        public void Turnon(GameObject o)
        {
            MeshRenderer renderer = o.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                Material instanceMaterial = new Material(on);
                renderer.material = instanceMaterial;
            }
        }

        public void Turnoff(GameObject[,] o, int xcount, int ycount)
        {
            for (int i = 0; i < xcount; i++)
            {
                for (int j = 0; j < ycount; j++)
                {
                    MeshRenderer renderer = o[i, j].GetComponent<MeshRenderer>();
                    if (renderer != null)
                    {
                        Material instanceMaterial = new Material(off);
                        renderer.material = instanceMaterial;
                    }
                }
            }
        }


        public Group[] findGroups(string source, string target)
        {
            Group[] g = new Group[2];

            foreach (Group group in Groups)
            {
                if (group.getID().Equals(source))
                {
                    g[0] = group;

                }
                else if (group.getID().Equals(target))
                {
                    g[1] = group;
                }
            }
            return g;
        }
    }
