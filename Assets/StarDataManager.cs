using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class StarDataManager : MonoBehaviour
{

     public GameObject[] stars;
     public GameObject[] cur_StarsPairs = new GameObject[25];
     public string[] starPairsDisplay;

    public GameObject info;
    public List<Material> newMat = new List<Material>();

    bool isChangeColor = false;

    public Text yearText;
    public float year = 0;


    public Material sky1;
    public Material sky2;
    // Define a class to hold star data
    public class StarData
    {
        public string HIP;      // Hipparcos #
        public float DIST;      // Distance from Sol in parsecs
        public float X0, Y0, Z0;   // 3D position relative to Sol at 0, 0, 0 in parsecs
        public float ABSMAG;    // Absolute magnitude (brightness) of the star
        public float MAG;       // Relative magnitude (brightness) of the star as seen from Earth
        public float VX, VY, VZ;   // Velocity of the star relative to Sol in km per second
        public string SPECT;    // Spectral class of the star
          
       
        // Constructor to initialize the StarData object
        public StarData(string hip, float dist, float x0, float y0, float z0, float absMag, float mag, float vx, float vy, float vz, string spect)
        {
            HIP = hip;
            DIST = dist;
            X0 = x0;
            Y0 = y0;
            Z0 = z0;
            ABSMAG = absMag;
            MAG = mag;
            VX = vx;
            VY = vy;
            VZ = vz;
            SPECT = spect;
        }
    }
    public class StarData2
    {
        public string plname;      // Hipparcos #
     
        public string hipname;    // Spectral class of the star


        // Constructor to initialize the StarData object
        public StarData2(string Plname, string Hipname)
        {
            plname = Plname;
            hipname = Hipname;
           
        }
    }
    // List to store StarData objects
    public List<StarData> starDataList = new List<StarData>();
    public List<StarData2> starDataList2 = new List<StarData2>();
    // Path to the CSV file containing star data
    //public string csvFilePath = "Assets/StarResources/stardata_clean_final.csv";
    //string csvFilePath = Application.dataPath + "/StarResources/stardata_clean_final.csv";
    string csvFilePath = Application.streamingAssetsPath + "/stardata_clean_final.csv";
    //public string csvFilePath2 = "Assets/StarResources/exoplanet_data.csv";
    //string csvFilePath2 = Application.dataPath + "/StarResources/exoplanet_data.csv";
    string csvFilePath2 = Application.streamingAssetsPath + "/exoplanet_data.csv";

    // Dictionary to store star positions by Hipparcos number
    private Dictionary<string, Vector3> starPositions = new Dictionary<string, Vector3>();

    // Dictionary to store star pairs for each constellation
    private Dictionary<string, List<string>> constellationStarPairs = new Dictionary<string, List<string>>();

    // Additional information about constellations
    public GameObject constellationInfoPanel;
    public Text constellationInfoText;

    // Line material for constellation lines
    public Material lineMaterial;




    // Scale for star objects
    private const float scale = 0.3f;

    // Elapsed time display
    public Text timeText;
    private float elapsedTime;

    // Time interval for updating positions
    //public float positionUpdateInterval = 10f; // Update positions every 10 seconds
    public float positionUpdateInterval = 1f; // Update positions every 1 seconds

    float starScale = 1;

    bool isPauseTime = false;

    bool isLarge = false;


    bool star1 = true;
    bool star2 = true;
    bool star3 = true;
    bool star4 = true;
    bool star5 = true;
    void Start()
    {
        // Initialize the line material
        lineMaterial = new Material(Shader.Find("Sprites/Default"));
        lineMaterial.color = Color.white;

        // Read data from CSV file and populate starDataList
        ReadStarDataFromCSV(csvFilePath);
        ReadStarDataFromCSV2(csvFilePath2);
        // Read constellationship.fab and parse star pairs
        //string constellationFilePath = "Assets/StarResources/constellationship.fab";
        string constellationFilePath = Application.streamingAssetsPath + "/constellationship.fab";
        Debug.Log(constellationFilePath);

        ReadConstellationData(constellationFilePath);


        ReadStarDataFromCSV3(csvFilePath);



        // Display constellations
        DisplayConstellations();

        // Start the time progression coroutine
        //StartCoroutine(TimeProgression());
    }

    private void Update()
    {   
        // Update elapsed time
        elapsedTime += Time.deltaTime;
        timeText.text = "Elapsed Time: " + Mathf.FloorToInt(elapsedTime) + "s";
    }

    // Coroutine for time progression
    IEnumerator TimeProgression()
    {
      
            if(isPauseTime)
            {

            }
            else
            {
                // Update star positions based on velocities
                UpdateStarPositions(1);

                // Redraw constellations
                DisplayConstellations();
            }
            // Wait for the specified interval before updating positions
            yield return new WaitForSeconds(positionUpdateInterval);

         
        
    }

    // Update star positions based on velocities
    private void UpdateStarPositions(int dir)
    {
        // Define time scale factor (e.g., 1,000 years per second)
        float timeScaleFactor = 1000f;

        foreach (var starData in starDataList)
        {
            // Store current position for debugging
            Vector3 currentPosition = new Vector3(starData.X0, starData.Y0, starData.Z0);

            // Calculate position changes based on velocity and time scale factor
            float deltaTimeYears =dir * Time.deltaTime * timeScaleFactor;
            //Debug.Log(deltaTimeYears);
            starData.X0 += starData.VX * deltaTimeYears;
            starData.Y0 += starData.VY * deltaTimeYears;
            starData.Z0 += starData.VZ * deltaTimeYears;

            
            
            // Store updated position for debugging
            Vector3 updatedPosition = new Vector3(starData.X0, starData.Y0, starData.Z0);

            // Log position changes for debugging
            //Debug.Log($"Star {starData.HIP} moved from {currentPosition} to {updatedPosition}");

            // Update starPositions dictionary
            if (starPositions.ContainsKey(starData.HIP))
            {
                starPositions[starData.HIP] = updatedPosition;
            }
            else
            {
                starPositions.Add(starData.HIP, updatedPosition);
            }
        }

        if(isLarge)
        {
            isLarge = false;
        }
        else
        {
            float TimeYears = dir * timeScaleFactor;
            year += TimeYears * 1.02269f;
            yearText.text = year.ToString() + "years";
        }
       

    }


    private void DisplayConstellations()
    {
        foreach (var pair in constellationStarPairs)
        {
            string constellationName = pair.Key;  
            
            for(int j = 0; j < starPairsDisplay.Length; j ++)           
            {
                if(starPairsDisplay[j] == constellationName)
                {
                    
                    Debug.Log(constellationName);

                    if(GameObject.Find(constellationName) != null)
                    {
                        Destroy(GameObject.Find(constellationName).gameObject);
                    }
                   
                    List<string> starPairs = pair.Value;

                    // Create a new empty GameObject to hold the constellation lines
                    GameObject constellationObj = new GameObject(constellationName);
                    
                    cur_StarsPairs[j] = constellationObj;
                    // Iterate through star pairs and draw lines between them
                    for (int i = 0; i < starPairs.Count; i += 2)
                    {  
                        
                        string star1 = starPairs[i];
                        string star2 = starPairs[i + 1];

                        //Debug.Log(starPairs[i]);
                        // Check if both stars exist in the dictionary
                        if (starPositions.ContainsKey(star1) && starPositions.ContainsKey(star2))
                        {
                            // Find the positions of the two stars
                            Vector3 star1Position = starPositions[star1];
                            Vector3 star2Position = starPositions[star2];

                            // Draw a line between the two stars
                            GameObject lineObj = new GameObject("Line");
                            LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();
                            lineRenderer.startWidth = 0.5f;
                            lineRenderer.endWidth = 0.5f;
                            lineRenderer.material = lineMaterial;
                            lineRenderer.startColor = Color.white;
                            lineRenderer.endColor = Color.white;
                            lineRenderer.SetPosition(0, star1Position);
                            lineRenderer.SetPosition(1, star2Position);

                            // Set the parent of the line object to the constellation object
                            lineObj.transform.SetParent(constellationObj.transform);
                        }
                        else
                        {
                            // Handle missing key (optional)
                            Debug.LogWarning("One or both stars not found: " + star1 + ", " + star2);
                        }
                    }
                }
            }


          
        }
    }

    // Read constellation data from the constellationship.fab file
    private void ReadConstellationData(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError("Constellationship file not found at: " + filePath);
            return;
        }

        string[] lines = File.ReadAllLines(filePath);
        foreach (string line in lines)
        {
            string[] parts = line.Trim().Split(new char[] { ' ', '\t' }, System.StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 3)
            {
                string constellation = parts[0];
                int count = int.Parse(parts[1]);
                List<string> starPairs = new List<string>();
                for (int i = 2; i < parts.Length; i++)
                {
                    starPairs.Add(parts[i]);
                }
                constellationStarPairs[constellation] = starPairs;
            }
        }
    }

    // Read star data from the CSV file
    private void ReadStarDataFromCSV(string filePath)
    {
        // Check if the file exists
        if (!File.Exists(filePath))
        {
            Debug.LogError("File not found: " + filePath);
            return;
        }

        // Read all lines from the CSV file
        string[] lines = File.ReadAllLines(filePath);
        Debug.Log("star count :" + lines.Length);
        // Skip the first line (header) and start parsing from the second line
        for (int i = 1; i < lines.Length; i+= 8)
        {
            string[] values = lines[i].Split(',');

            // Check if the line has the correct number of values
            if (values.Length == 11)
            {
                // Declare variables to hold parsed float values
                float dist, x0, y0, z0, absMag, mag, vx, vy, vz;

                // Check if the values are not empty before parsing
                if (!string.IsNullOrEmpty(values[1]) &&
                    !string.IsNullOrEmpty(values[2]) &&
                    !string.IsNullOrEmpty(values[3]) &&
                    !string.IsNullOrEmpty(values[4]) &&
                    !string.IsNullOrEmpty(values[5]) &&
                    !string.IsNullOrEmpty(values[6]) &&
                    !string.IsNullOrEmpty(values[7]) &&
                    !string.IsNullOrEmpty(values[8]) &&
                    !string.IsNullOrEmpty(values[9]))
                {
                    // Try to parse float values from the string array
                    if (float.TryParse(values[1], out dist) &&
                        float.TryParse(values[2], out x0) &&
                        float.TryParse(values[3], out y0) &&
                        float.TryParse(values[4], out z0) &&
                        float.TryParse(values[5], out absMag) &&
                        float.TryParse(values[6], out mag) &&
                        float.TryParse(values[7], out vx) &&
                        float.TryParse(values[8], out vy) &&
                        float.TryParse(values[9], out vz))
                    {
                        // Create a new StarData object and add it to the list
                        StarData starData = new StarData(
                            values[0],   // HIP
                            dist,        // DIST
                            x0,          // X0
                            y0,          // Y0
                            z0,          // Z0
                            absMag,      // ABSMAG
                            mag,         // MAG
                            vx,          // VX
                            vy,          // VY
                            vz,          // VZ
                            values[10]   // SPECT
                        );

                        // Add the StarData object to the list
                        starDataList.Add(starData);

                        // Store star positions by Hipparcos number
                        starPositions[values[0]] = new Vector3(x0, y0, z0);


                        foreach (var pair in constellationStarPairs)
                        {
                            string constellationName = pair.Key;

                            for (int j = 0; j < starPairsDisplay.Length; j++)
                            {
                                if (starPairsDisplay[j] == constellationName)
                                {

                                    //Debug.Log(constellationName);

                                    List<string> starPairs = pair.Value;

                                    // Create a new empty GameObject to hold the constellation lines
                                    GameObject constellationObj = new GameObject(constellationName);
                                    cur_StarsPairs[j] = constellationObj;
                                    // Iterate through star pairs and draw lines between them
                                    for (int m = 0; m< starPairs.Count; m += 2)
                                    {

                                        

                                        Debug.Log(starPairs[m]);
                                        if(starPairs[m] == values[0])
                                        {
                                            // Instantiate star objects at their positions
                                            //GameObject starObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                                            int index = Random.Range(0, stars.Length);
                                            GameObject starObject = Instantiate(stars[index]);
                                            starObject.name = values[0].ToString();
                                            starObject.transform.position = new Vector3(x0, y0, z0) * starScale;
                                            starObject.transform.localScale = Vector3.one * scale;

                                        }

                                    }
                                }
                            }



                        }





                      
                    }
                    else
                    {
                        // Log an error if parsing fails for any value
                        Debug.LogError("Failed to parse values from line " + i + ": " + lines[i]);
                    }
                }
                else
                {
                    // Log an error if any value is empty
                    Debug.LogError("Empty value found in line " + i + ": " + lines[i]);
                }
            }
            else
            {
                // Log an error if the line does not have the correct number of values
                Debug.LogError("Invalid number of values in line " + i + ": " + lines[i]);
            }
        }
    }


    private void ReadStarDataFromCSV3(string filePath)
    {
        // Check if the file exists
        if (!File.Exists(filePath))
        {
            Debug.LogError("File not found: " + filePath);
            return;
        }

        // Read all lines from the CSV file
        string[] lines = File.ReadAllLines(filePath);
        Debug.Log("star count :" + lines.Length);
        // Skip the first line (header) and start parsing from the second line
        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');

            // Check if the line has the correct number of values
            if (values.Length == 11)
            {
                // Declare variables to hold parsed float values
                float dist, x0, y0, z0, absMag, mag, vx, vy, vz;

                // Check if the values are not empty before parsing
                if (!string.IsNullOrEmpty(values[1]) &&
                    !string.IsNullOrEmpty(values[2]) &&
                    !string.IsNullOrEmpty(values[3]) &&
                    !string.IsNullOrEmpty(values[4]) &&
                    !string.IsNullOrEmpty(values[5]) &&
                    !string.IsNullOrEmpty(values[6]) &&
                    !string.IsNullOrEmpty(values[7]) &&
                    !string.IsNullOrEmpty(values[8]) &&
                    !string.IsNullOrEmpty(values[9]))
                {
                    // Try to parse float values from the string array
                    if (float.TryParse(values[1], out dist) &&
                        float.TryParse(values[2], out x0) &&
                        float.TryParse(values[3], out y0) &&
                        float.TryParse(values[4], out z0) &&
                        float.TryParse(values[5], out absMag) &&
                        float.TryParse(values[6], out mag) &&
                        float.TryParse(values[7], out vx) &&
                        float.TryParse(values[8], out vy) &&
                        float.TryParse(values[9], out vz))
                    {
                        // Create a new StarData object and add it to the list
                        StarData starData = new StarData(
                            values[0],   // HIP
                            dist,        // DIST
                            x0,          // X0
                            y0,          // Y0
                            z0,          // Z0
                            absMag,      // ABSMAG
                            mag,         // MAG
                            vx,          // VX
                            vy,          // VY
                            vz,          // VZ
                            values[10]   // SPECT
                        );

                        // Add the StarData object to the list
                        starDataList.Add(starData);

                        // Store star positions by Hipparcos number
                        starPositions[values[0]] = new Vector3(x0, y0, z0);


                        // Instantiate star objects at their positions
                        //GameObject starObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        int index = Random.Range(0, stars.Length);
                        GameObject starObject = Instantiate(stars[index]);
                        starObject.name = values[0].ToString();
                        starObject.transform.position = new Vector3(x0, y0, z0) * starScale;
                        starObject.transform.localScale = Vector3.one * scale;
                    }
                    else
                    {
                        // Log an error if parsing fails for any value
                        Debug.LogError("Failed to parse values from line " + i + ": " + lines[i]);
                    }
                }
                else
                {
                    // Log an error if any value is empty
                    Debug.LogError("Empty value found in line " + i + ": " + lines[i]);
                }
            }
            else
            {
                // Log an error if the line does not have the correct number of values
                Debug.LogError("Invalid number of values in line " + i + ": " + lines[i]);
            }
        }
    }
    // Method to display information about the selected constellation
    public void ShowConstellationInfo(string constellationName)
    {
        constellationInfoPanel.SetActive(true);
        constellationInfoText.text = "Constellation: " + constellationName;
    }

    // Method to hide the constellation information panel
    public void HideConstellationInfo()
    {
        constellationInfoPanel.SetActive(false);
    }

    public void StarPairsDisplayBtn1()
    {
        HideStarPairs();
          Debug.Log("111111");
        if(star1)
        {
            star1 = false;
            cur_StarsPairs[0].SetActive(false);
        }
        else
        {
            star1 = true;
            cur_StarsPairs[0].SetActive(true);
        }
    }

     public void StarPairsDisplayBtn2()
    {
        HideStarPairs();
        Debug.Log("22222");
        if (star2)
        {
            star2 = false;
            cur_StarsPairs[1].SetActive(false);
        }
        else
        {
            star2 = true;
            cur_StarsPairs[1].SetActive(true);
        }
    }
     public void StarPairsDisplayBtn3()
    {
        HideStarPairs();
        Debug.Log("33333");
        if (star3)
        {
            star3 = false;
            Debug.Log("33333--1");
            cur_StarsPairs[2].SetActive(false);
        }
        else
        {
            star3 = true;
            Debug.Log("33333--2");
            cur_StarsPairs[2].SetActive(true);
        }
    }
     public void StarPairsDisplayBtn4()
    {
        HideStarPairs();
        Debug.Log("44444");
        if (star4)
        {
            star4 = false;
            cur_StarsPairs[3].SetActive(false);
        }
        else
        {
            star4 = true;
            cur_StarsPairs[3].SetActive(true);
        }
    }
     public void StarPairsDisplayBtn5()
    {
        HideStarPairs();
        Debug.Log("55555");
        if (star5)
        {
            star5 = false;
            cur_StarsPairs[4].SetActive(false);
        }
        else
        {
            star5 = true;
            cur_StarsPairs[4].SetActive(true);
        }
    }

    public void ShowStarInfo()
    {
        Debug.Log("info");
        if (info.activeSelf)
        {
            info.SetActive(false);
        }
        else
        {
            info.SetActive(true);
            GameObject.Find("CAVE2-PlayerController").transform.position = new Vector3(6.6f, 7.7f, 15.5f);
            GameObject.Find("CAVE2-PlayerController").transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }

    public void TimeStart()
    {
        isPauseTime = false;
    }

    public void TimePause()
    {
        isPauseTime = true;
    }

    public void SetStarScaleLarge()
    {
        isLarge = true;
        starScale += 10f;
        StartCoroutine(TimeProgression());
    }
    public void SetStarScaleSmall()
    {
        isLarge = true;
        starScale -= 10f;
        StartCoroutine(TimeProgression());
    }


    private void ReadStarDataFromCSV2(string filePath)
    {
        // Check if the file exists
        if (!File.Exists(filePath))
        {
            Debug.LogError("File not found: " + filePath);
            return;
        }

        // Read all lines from the CSV file
        string[] lines = File.ReadAllLines(filePath);

        // Skip the first line (header) and start parsing from the second line
        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');

            // Check if the line has the correct number of values
            if (values.Length == 6)
            {
                // Declare variables to hold parsed float values
                string pl_name, hostname, hip_name, default_flag, sy_snum, sy_pnum;

                // Check if the values are not empty before parsing
                if (!string.IsNullOrEmpty(values[0]) &&
                    !string.IsNullOrEmpty(values[1]) &&
                    !string.IsNullOrEmpty(values[2]) &&
                    !string.IsNullOrEmpty(values[3]) &&
                    !string.IsNullOrEmpty(values[4]) &&
                    !string.IsNullOrEmpty(values[5])                 
                   )
                {
                    // Try to parse float values from the string array

                    // Create a new StarData object and add it to the list

                    string[] s = values[2].Split(' ');
                        StarData2 starData2 = new StarData2(values[0], s[1]);

                       // Debug.Log("pl" + values[0] + "..hip.." + s[1]);
                        // Add the StarData object to the list
                        starDataList2.Add(starData2);


                    if (GameObject.Find(s[1]) == null)
                        return;
                    GameObject.Find(s[1]).GetComponent<MeshRenderer>().material.color = Color.white;
                        //// Store star positions by Hipparcos number
                        //starPositions[values[0]] = new Vector3(x0, y0, z0);

                        //// Instantiate star objects at their positions
                        ////GameObject starObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        //int index = Random.Range(0, stars.Length);
                        //GameObject starObject = Instantiate(stars[index]);
                        //starObject.transform.position = new Vector3(x0, y0, z0) * starScale;
                        //starObject.transform.localScale = Vector3.one * scale;
                   
                }
                else
                {
                    // Log an error if any value is empty
                    Debug.LogError("Empty value found in line " + i + ": " + lines[i]);
                }
            }
            else
            {
                // Log an error if the line does not have the correct number of values
                Debug.LogError("Invalid number of values in line " + i + ": " + lines[i]);
            }
        }
    }
   
    public void ExoplanetColor()
    {
        if(!isChangeColor)
        {
            isChangeColor = true;

            RenderSettings.skybox = sky2;
            //foreach (var s in starDataList)
            //{




            //    //Debug.Log(s.HIP);
            //    GameObject.Find(s.HIP).GetComponent<MeshRenderer>().material = newMat[0];
            //}
            foreach (var s in starDataList2)
            {
                if (GameObject.Find(s.hipname) == null)
                {

                }
                 else
                {
                    int i = Random.Range(0, newMat.Count);

                    GameObject.Find(s.hipname).GetComponent<MeshRenderer>().material = newMat[i];
                }

               
            }

        }
        else
        {
            isChangeColor = false;


            RenderSettings.skybox = sky1;

            //foreach (var s in starDataList)
            //{

            //    //Debug.Log(s.HIP);
            //    int i = Random.Range(0, newMat.Count);
            //    GameObject.Find(s.HIP).GetComponent<MeshRenderer>().material = newMat[i];
            //}

            foreach (var s in starDataList2)
            {
                if (GameObject.Find(s.hipname) == null)
                {

                }
                else
                {
                    GameObject.Find(s.hipname).GetComponent<MeshRenderer>().material = newMat[0];

                }
            }
        }

    }


    public void UpdateStarsForward()
    {
        UpdateStarPositions(1);
        DisplayConstellations();
    }

    public void UpdateStarsBack()
    {
        UpdateStarPositions(-1);
        DisplayConstellations();
    }


    public void ShowStarPairs()
    {
        foreach(var v in cur_StarsPairs)
        {
            v.SetActive(true);
        }
    }


    public void HideStarPairs()
    {
        foreach (var v in cur_StarsPairs)
        {
            v.SetActive(false);
        }
    }
}







