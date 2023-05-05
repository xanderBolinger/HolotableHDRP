using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using WaveFunctionCollapse;
using System.Text;
using System.Linq;

public class PerlinGenerator : MonoBehaviour
{
    public static PerlinGenerator instance;

    public int mapWidth = 25;
    public int mapHeight = 12;

    float xSpacing = 0.19f;
    float ySpacing = 0.165f;

    public GameObject treePrefab;
    public GameObject mountainPrefab;
    public GameObject grassPrefab;
    public GameObject cityPrefab;
    public GameObject townPrefab;
    public GameObject highwayPrefab;
    public GameObject pathPrefab;

    /*int densityUpper = 5;
    int densityLower = 2; */

    int upperOffset = 1000;
    int lowerOffset = 0;
    /*float upperMagnification = 7.5f;
    float lowerMagnification = 6.5f;*/

    public HexFrequency.Frequency mountainFrequency;
    public HexFrequency.Frequency treeFrequency;
    public HexFrequency.Frequency cityFrequency;
    public HexFrequency.Frequency townFrequency;


    public int k = 4;

    public List<List<GameObject>> hexes = new List<List<GameObject>>();
    List<List<int>> cityCoordinates = new List<List<int>>();
    List<List<int>> townCoordinates = new List<List<int>>();

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        CreateTileMap();
        
    }

    public void ClearMap() {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        hexes.Clear();
    }

    public void CreateTileMap()
    {
        cityCoordinates.Clear();
        townCoordinates.Clear();
        var mountainStats = new HexFrequency(mountainFrequency);
        var treeStats = new HexFrequency(treeFrequency);
        var townStats = new HexFrequency(townFrequency);
        var cityStats = new HexFrequency(cityFrequency);

        Debug.Log("Mountain: ");
        var mountainMap = NoiseMap(mountainStats);
        Debug.Log("Tree Map: ");
        var treeMap = NoiseMap(treeStats);
        Debug.Log("Town Map: ");
        var townMap = NoiseMap(townStats);
        Debug.Log("City Map: ");
        var cityMap = NoiseMap(cityStats);




        for (int x = 0; x < mapWidth; x++)
        {

            var row = new List<GameObject>();

            for (int y = 0; y < mapHeight; y++)
            {
                int mountain = mountainMap[x][y];
                int tree = treeMap[x][y];
                int town = townMap[x][y];
                int city = cityMap[x][y];

                bool urbanHex = false; 

                GameObject hexPrefab = grassPrefab;
                if (city >= cityStats.margin) {
                    cityCoordinates.Add(new List<int> { x, y });
                    hexPrefab = cityPrefab;
                    urbanHex = true;
                }
                else if (town >= townStats.margin) {
                    townCoordinates.Add(new List<int> { x, y });
                    urbanHex = true;
                    hexPrefab = townPrefab;
                }
                else if(mountain >= mountainStats.margin)
                    hexPrefab = mountainPrefab;
                else if (tree >= treeStats.margin)
                    hexPrefab = treePrefab;

                GameObject hex = Instantiate(hexPrefab);
                
                if (y % 2 == 0)
                {
                    hex.transform.position = new Vector3(x * xSpacing, 0, y * ySpacing);
                }
                else
                {
                    hex.transform.position = new Vector3(x * xSpacing + xSpacing / 2, 0, y * ySpacing);
                }

                hex.name = x + ", " + y;
                if (hex.GetComponent<HexCord>() == null)
                {
                    hex.GetComponentInChildren<HexCord>().urbanHex = urbanHex;
                    hex.GetComponentInChildren<HexCord>().x = x;
                    hex.GetComponentInChildren<HexCord>().y = y;
                }
                else
                {
                    hex.GetComponent<HexCord>().urbanHex = urbanHex;
                    hex.GetComponent<HexCord>().y = y;
                    hex.GetComponent<HexCord>().y = y;
                }

                
                hex.transform.parent = gameObject.transform;
                row.Add(hex);

            }

            hexes.Add(row);
        }

        RoadGenerator.roadCords.Clear();

        if (cityCoordinates.Count > 2) {
            SetCityCoordinates();
            cityCoordinates.Insert(0, new List<int> { 0, 10 });
            cityCoordinates.Add(new List<int> { 99, 47 });
            RoadGenerator.CreateRoads(cityCoordinates, hexes, highwayPrefab);
        }

        if (townCoordinates.Count > 2) {
            SetTownCoordinates();
            
            RoadGenerator.CreateRoads(townCoordinates, hexes, pathPrefab);
        }


        GetComponent<Tilemap>().initTilemap();
        InputReader inputReader = new InputReader(GetComponent<Tilemap>());
        var grid = inputReader.ReadInputToGrid();
        ValueManager<TileBase> valueManager = new ValueManager<TileBase>(grid);
        PatternManager manager = new PatternManager(2);
        manager.ProcessGrid(valueManager, false);

        foreach (Direction dir in Enum.GetValues(typeof(Direction))) {
            Debug.Log(dir.ToString() + " " + string.Join(" ", manager.GetPossibleNeighoursForPatternInDirection(0, dir).ToArray()));
        }

        Debug.Log("---");

        WFCCore core = new WFCCore(5, 5, 50, manager);
        var result = core.CreateOutputGrid();

        /*TileBase woods = new TileBase(HexCord.HexType.WOODS);
        TileBase woods1 = new TileBase(HexCord.HexType.WOODS);
        TileBase citytb = new TileBase(HexCord.HexType.CITY);

        TileBaseValue woodsValue = new TileBaseValue(woods);
        TileBaseValue woods1Value = new TileBaseValue(woods);
        TileBaseValue cityValue = new TileBaseValue(citytb);

        if (woods == woods1) {
            Debug.Log("Comparison pass 0");
        }
        if (woods == citytb)
        {
            Debug.Log("Comparison pass 00");
        }


        if (woodsValue == woods1Value)
        {
            Debug.Log("Comparison pass 1");
        }
        else {
            Debug.Log("Comparison fail 1");
        }

        if (cityValue != woods1Value)
        {
            Debug.Log("Comparison pass 2");
        }
        else
        {
            Debug.Log("Comparison fail 2");
        }

        if (woodsValue.Equals(woods1Value))
        {
            Debug.Log("Comparison pass 3");
        }
        else
        {
            Debug.Log("Comparison fail 3");
        }

        if (!cityValue.Equals(woods1Value))
        {
            Debug.Log("Comparison pass 4");
        }
        else
        {
            Debug.Log("Comparison fail 4");
        }*/

        /* StringBuilder builder = null;
         List<string> list = new List<string>();
         for (int r = -1; r < grid.Length; r++) {
             builder = new StringBuilder();

             for (int c = -1; c < grid[0].Length; c++) {
                 builder.Append(valueManager.GetGridValuesIncludingOffset(c, r)+" ");
             }

             list.Add(builder.ToString());

         }

         list.Reverse();
         foreach (var str in list) {
             Debug.Log(str);
         } */

    }

    void SetTownCoordinates()
    {

        List<double[]> uncleanCenters = Kmeans(ConvertCoordinates(townCoordinates), k);

        List<double[]> centers = new List<double[]>();

        foreach (var c in uncleanCenters)
        {
            if (!containsCenter(c, centers))
                centers.Add(c);
        }

        townCoordinates.Clear();

        foreach (var center in centers)
        {
            Debug.Log("Center: " + center[0] + ", " + center[1]);
            townCoordinates.Add(new List<int> { (int)center[0], (int)center[1] });
        }

    }

    void SetCityCoordinates() {

        List<double[]> uncleanCenters = Kmeans(ConvertCoordinates(cityCoordinates), k);

        List<double[]> centers = new List<double[]>();

        foreach (var c in uncleanCenters) {
            if (!containsCenter(c, centers))
                centers.Add(c);
        }

        cityCoordinates.Clear();

        foreach (var center in centers) {
            Debug.Log("Center: "+center[0]+", "+center[1]);
            cityCoordinates.Add(new List<int> {(int) center[0], (int)center[1] });
        }

    }

    bool containsCenter(double[] center, List<double[]> centers) {

        foreach (var c in centers) {
            if ((center[0] == c[0] && center[1] == c[1]) || Distance(c, center) < 4)
                return true;
        }

        return false; 
    }

    double[,] ConvertCoordinates(List<List<int>> list) {
        // Assuming cityCoordinates is already initialized and filled with data
        int rows = list.Count;
        int cols = list[0].Count;

        double[,] cityCoordinatesArray = new double[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                cityCoordinatesArray[i, j] = Convert.ToDouble(list[i][j]);
            }
        }

        return cityCoordinatesArray;
    }

    public static List<double[]> Kmeans(double[,] data, int k)
    {
        List<double[]> centroids = new List<double[]>();
        System.Random rnd = new System.Random();

        // add a randomly selected data point to the centroids list
        double[] initialCentroid = new double[] { data[rnd.Next(data.GetLength(0)), 0], data[rnd.Next(data.GetLength(0)), 1] };
        centroids.Add(initialCentroid);

        // compute remaining k - 1 centroids
        for (int c_id = 0; c_id < k - 1; c_id++)
        {
            // initialize a list to store distances of data points from nearest centroid
            List<double> dist = new List<double>();
            for (int i = 0; i < data.GetLength(0); i++)
            {
                double[] point = new double[] { data[i, 0], data[i, 1] };
                double d = double.MaxValue;

                // compute distance of 'point' from each of the previously selected centroid
                // and store the minimum distance
                foreach (double[] centroid in centroids)
                {
                    double temp_dist = Distance(point, centroid);
                    d = Math.Min(d, temp_dist);
                }
                dist.Add(d);
            }

            // select data point with maximum distance as our next centroid
            double[] next_centroid = new double[] { data[dist.IndexOf(Max(dist)), 0], data[dist.IndexOf(Max(dist)), 1] };
            centroids.Add(next_centroid);
            dist.Clear();
        }
        return centroids;
    }

    public static double Max(List<double> list) {
        double max = -1;
        foreach (var val in list) { max = val > max ? val : max; }
        return max;
    } 

    public static double Distance(double[] a, double[] b)
    {

        int x0 = (int)a[0] - (int)Mathf.Floor((int)a[1] / 2);
        int y0 = (int)a[1];
        int x1 = (int)b[0] - (int)Mathf.Floor((int)b[1] / 2);
        int y1 = (int)b[0];
        int dx = x1 - x0;
        int dy = y1 - y0;
        return Mathf.Max(Mathf.Abs(dx), Mathf.Abs(dy), Mathf.Abs(dx + dy));
    }

    /*GameObject GetHex(int x, int y)
    {
        
        float rawPerlin = Mathf.PerlinNoise(
            (x - xOffset) / magnification,
            (y - yOffset) / magnification
        );
        float clampPerlin = Mathf.Clamp01(rawPerlin);
        float scaledPerlin = clampPerlin * hexPrefabs.Count;

        if (scaledPerlin == hexPrefabs.Count)
        {
            scaledPerlin = (hexPrefabs.Count - 1);
        }
        return hexPrefabs[Mathf.FloorToInt(scaledPerlin)];
    }*/

    List<List<int>> NoiseMap(HexFrequency freq)
    {
        int densityLower = freq.densityLower; 
        int densityUpper = freq.densityUpper;
        float magnificationLower = freq.magnifiactionLower;
        float magnificationUpper = freq.magnificationUpper;
        List<List<int>> map = new List<List<int>>();

        float magnification = UnityEngine.Random.Range(magnificationLower, magnificationUpper);
        System.Random rand = new System.Random();


        int density = UnityEngine.Random.Range(densityLower, densityUpper);
        int number = rand.Next(lowerOffset, upperOffset);
        int xOffset = number;
        int yOffset = number;
        Debug.Log("Offset: " + number + ", Magnification: " + magnification + ", Density: " + density);

        CalculatePerlin(xOffset, yOffset, magnification, density, map);

        return map;

    }


    /*List<List<int>> NoiseMap() {
        List<List<int>> map = new List<List<int>>();

        float magnification = UnityEngine.Random.Range(lowerMagnification, upperMagnification);
        System.Random rand = new System.Random();


        int density = UnityEngine.Random.Range(densityLower, densityUpper); 
        int number = rand.Next(lowerOffset, upperOffset);
        int xOffset = number;
        int yOffset = number;
        Debug.Log("Offset: " + number + ", Magnification: " + magnification+", Density: "+density);

        CalculatePerlin(xOffset, yOffset, magnification, density, map);

        return map;

    }*/

    void CalculatePerlin(int xOffset, int yOffset, float magnification, int density, List<List<int>> map) {
        for (int x = 0; x < mapWidth; x++)
        {

            List<int> values = new List<int>();

            for (int y = 0; y < mapHeight; y++)
            {
                float rawPerlin = Mathf.PerlinNoise(
                (x - xOffset) / magnification,
                (y - yOffset) / magnification
            );
                float clampPerlin = Mathf.Clamp01(rawPerlin);
                float scaledPerlin = clampPerlin * density;

                if (scaledPerlin == density)
                {
                    scaledPerlin = (density - 1);
                }
                //Debug.Log(Mathf.FloorToInt(scaledPerlin));
                values.Add(Mathf.FloorToInt(scaledPerlin));

            }

            map.Add(values);

        }
    }

    

}
