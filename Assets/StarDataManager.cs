using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;

public class StarDataManager : MonoBehaviour
{
    // Define a class to hold star data
    public class StarData
    {
        public string HIP;   // Hipparcos #
        public float DIST;   // Distance from Sol in parsecs
        public float X0, Y0, Z0;   // 3D position relative to Sol at 0, 0, 0 in parsecs
        public float ABSMAG;   // Absolute magnitude (brightness) of the star
        public float MAG;   // Relative magnitude (brightness) of the star as seen from Earth
        public float VX, VY, VZ;   // Velocity of the star relative to Sol in km per second
        public string SPECT;   // Spectral class of the star

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

    // List to store StarData objects
    public List<StarData> starDataList = new List<StarData>();
    // Path to the CSV file containing star data
    public string csvFilePath = "C:\\Users\\uicel\\OneDrive\\Documents\\Shanghao\\stardata_clean_final.csv";
    // Define scale for cubes
    private const float scale = 0.3f;

    // Dictionary to store constellation names
    private Dictionary<string, string> constellationNames = new Dictionary<string, string>();
    // Dictionary to store star pairs for each constellation
    private Dictionary<string, List<string>> constellationStarPairs = new Dictionary<string, List<string>>();

    // Elapsed time display
    public Text timeText;
    private float elapsedTime;

    // Additional information about constellations
    public GameObject constellationInfoPanel;

    ///////////00000000000000000
    // Create a new material for the constellation lines
    public Material lineMaterial;
    ///////////000000000000000000

    void Start()
    {
        ///////////00000000000000000
        // Initialize the line material with the desired color
        lineMaterial = new Material(Shader.Find("Sprites/Default"));
        lineMaterial.color = Color.white;
        ///////////00000000000000000

        // Read data from CSV file and populate starDataList
        ReadStarDataFromCSV(csvFilePath);
        // Read constellationship.fab and parse star pairs
        string constellationFilePath = "C:\\Users\\uicel\\OneDrive\\Documents\\Shanghao\\constellationship.fab";
        ReadConstellationData(constellationFilePath);
        // Display constellations
        DisplayConstellations();
    }

    void Update()
    {
        // Update the positions of the stars based on their velocities
        float timeScaleFactor = 1000f; // 1000 years per second
        float deltaTime = Time.deltaTime;
        foreach (StarData starData in starDataList)
        {
            float vx = starData.VX * timeScaleFactor;
            float vy = starData.VY * timeScaleFactor;
            float vz = starData.VZ * timeScaleFactor;
            starData.X0 += vx * deltaTime;
            starData.Y0 += vy * deltaTime;
            starData.Z0 += vz * deltaTime;
        }
        // Update the visual representation of stars accordingly
        UpdateStarPositions();

        // Update elapsed time display
        elapsedTime += Time.deltaTime;
        UpdateTimeDisplay(elapsedTime);
    }

    // Update the positions of the visual representations of stars in the scene
    void UpdateStarPositions()
    {
        // Update the positions of the visual representations of stars based on the updated positions of the stars in starDataList
    }

    // Update the elapsed time display
    void UpdateTimeDisplay(float elapsedSeconds)
    {
        // Debug log to check if timeText is null
        if (timeText == null)
        {
            Debug.LogError("timeText is not assigned!");
            return;
        }
        int years = (int)(elapsedSeconds / 31536000); // 1 year = 31,536,000 seconds
        int days = (int)((elapsedSeconds % 31536000) / 86400); // 1 day = 86,400 seconds
        timeText.text = "Elapsed Time: " + years + " years, " + days + " days";
    }

    // Display constellations
    private void DisplayConstellations()
    {
        foreach (var pair in constellationStarPairs)
        {
            string constellationName = pair.Key;
            List<string> starPairs = pair.Value;
            // Create a new empty GameObject to hold the constellation lines and text
            GameObject constellationObj = new GameObject(constellationName);

            // Create text object for constellation name
            GameObject textObj = new GameObject("Text");
            Text textComponent = textObj.AddComponent<Text>();
            textComponent.text = constellationName;
            textComponent.font = Resources.GetBuiltinResource<Font>("Arial.ttf"); // You can change the font if needed
            textComponent.fontSize = 12; // Set the font size
            textComponent.alignment = TextAnchor.MiddleCenter; // Center the text
            textObj.transform.SetParent(constellationObj.transform);

            // Iterate through star pairs and draw lines between them
            for (int i = 0; i < starPairs.Count; i += 2)
            {
                string star1 = starPairs[i];
                string star2 = starPairs[i + 1];
                // Find the positions of the two stars
                Vector3 star1Position = FindStarPosition(star1);
                Vector3 star2Position = FindStarPosition(star2);
                // Draw a line between the two stars
                GameObject lineObj = new GameObject("Line");
                LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();
                lineRenderer.startWidth = 0.1f;
                lineRenderer.endWidth = 0.1f;

                ///////////00000000000000000
                // Assign the line material to the LineRenderer
                lineRenderer.material = lineMaterial;
                ///////////00000000000000000
                
                // Set the color of the line
                lineRenderer.startColor = new Color(174f / 255f, 243f / 255f, 89f / 255f); // Color code #AEF359
                lineRenderer.endColor = new Color(0f, 1f, 1f); // Cyan color

                lineRenderer.SetPosition(0, star1Position);
                lineRenderer.SetPosition(1, star2Position);
                // Set the parent of the line object to the constellation object
                lineObj.transform.SetParent(constellationObj.transform);
            }
        }
    }


    // Find the position of a star by its Hipparcos number
    private Vector3 FindStarPosition(string hipparcos)
    {
        foreach (var starData in starDataList)
        {
            if (starData.HIP == hipparcos)
            {
                return new Vector3(starData.X0, starData.Y0, starData.Z0);
            }
        }
        return Vector3.zero;
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
            string[] parts = line.Trim().Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
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
                            dist,   // DIST
                            x0,   // X0
                            y0,   // Y0
                            z0,   // Z0
                            absMag,   // ABSMAG
                            mag,   // MAG
                            vx,   // VX
                            vy,   // VY
                            vz,   // VZ
                            values[10]   // SPECT
                        );

                        // Add the StarData object to the list
                        starDataList.Add(starData);
                         GameObject assetObject = Resources.Load<GameObject>("HIP");
                        if (assetObject == null){
                            assetObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        }
                        assetObject.transform.position = new Vector3(x0, y0, z0);
                        assetObject.transform.localScale = Vector3.one * scale;
                        Instantiate(assetObject);
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
        // Activate the info panel and populate it with information about the selected constellation
        constellationInfoPanel.SetActive(true);
        // You can set text, images, and other UI elements to provide information about the selected constellation
    }

    // Method to hide the constellation information panel
    public void HideConstellationInfo()
    {
        // Deactivate the info panel
        constellationInfoPanel.SetActive(false);
    }
}
