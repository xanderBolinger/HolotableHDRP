using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using WaveFunctionCollapse;
using System.Text;
using System.Linq;
using UnityEditor;

public class MapGenerator : MonoBehaviour
{
    public int testMargin;
    public float testMagnification;
    public int testDensity;
    public int testMargin2;
    public float testMagnification2;
    public int testDensity2;
    public int testMargin3;
    public float testMagnification3;
    public int testDensity3;

    public static MapGenerator instance;

    public int mapWidth = 25;
    public int mapHeight = 12;
    public int wfcOutputHeight = 30;
    public int wfcOutputWidth = 30;

    float xSpacing = 0.19f;
    float ySpacing = 0.165f;

    public string mapName;

    public GameObject heavyTreePrefab;
    public GameObject lightWoodsPrefab;
    public GameObject mediumWoodsPrefab;
    public GameObject heavyBrushPrefab;
    public GameObject brushPrefab;
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

    public ElevationDistribution elevationDistribution;
    public ElevationHeightRange elevationHeightRange;
    
    public HexFrequency.Frequency elevationFrequency;
    public HexFrequency.Frequency mountainFrequency;
    public HexFrequency.Frequency treeFrequency;
    public HexFrequency.Frequency brushFrequency;
    public HexFrequency.Frequency cityFrequency;
    public HexFrequency.Frequency townFrequency;

    InputReader inputReader;
    IValue<TileBase>[][] inputGrid;
    ValueManager<TileBase> valueManager;
    PatternManager patternManager;
    public int patternSize = 2;
    public int maximumIterations = 100;

    public int k = 4;

    public List<List<GameObject>> hexes = new List<List<GameObject>>();
    List<List<int>> cityCoordinates = new List<List<int>>();
    List<List<int>> townCoordinates = new List<List<int>>();

    public bool createTileMapOnStart = true;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        if(createTileMapOnStart)
            CreateTileMap();
        
    }

    public void ClearMap() {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        hexes.Clear();
    }

    public void SaveTilemap()
    {
        Destroy(gameObject.GetComponent<MapGenerator>().gameObject);
        Destroy(gameObject.GetComponent<Tilemap>().gameObject);
        gameObject.AddComponent<Map>();

        foreach (Transform child in transform)
        {
            child.transform.parent = gameObject.transform;
        }
        PrefabUtility.SaveAsPrefabAsset(gameObject, "Assets/Hex Map/Saved Maps/"+mapName+".prefab");
    }
    public void InstantiateHexes(List<List<GameObject>> hexPrefabs, int mapWidth, int mapHeight) {

        hexes.Clear();

        for (int x = 0; x < mapWidth; x++)
        {

            List<GameObject> row = new List<GameObject>();

            for (int y = 0; y < mapHeight; y++)
            {
                GameObject hex = Instantiate(hexPrefabs[x][y]);

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
                    hex.GetComponentInChildren<HexCord>().x = x;
                    hex.GetComponentInChildren<HexCord>().y = y;
                }
                else
                {
                    hex.GetComponent<HexCord>().x = x;
                    hex.GetComponent<HexCord>().y = y;
                }


                hex.transform.parent = gameObject.transform;
                row.Add(hex);
            }
            hexes.Add(row);
        }

    }

    public void CreateTileMap()
    {
        ClearMap();
        cityCoordinates.Clear();
        townCoordinates.Clear();
        var mountainStats = new HexFrequency(mountainFrequency);
        var brushStats = new HexFrequency(brushFrequency);
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

        var brushMap = NoiseMap(brushStats);
        
        for (int x = 0; x < mapWidth; x++)
        {

            var row = new List<GameObject>();

            for (int y = 0; y < mapHeight; y++)
            {
                int mountain = mountainMap[x][y];
                int tree = treeMap[x][y];
                int town = townMap[x][y];
                int city = cityMap[x][y];
                int brush = brushMap[x][y];
                bool urbanHex = false; 

                GameObject hexPrefab = grassPrefab;
                if (city >= cityStats.margin)
                {
                    cityCoordinates.Add(new List<int> { x, y });
                    hexPrefab = cityPrefab;
                    urbanHex = true;
                }
                else if (town >= townStats.margin)
                {
                    townCoordinates.Add(new List<int> { x, y });
                    urbanHex = true;
                    hexPrefab = townPrefab;
                }
                else if (mountain >= mountainStats.margin)
                    hexPrefab = mountainPrefab;
                else if (tree >= treeStats.margin + 1)
                    hexPrefab = heavyTreePrefab;
                else if (tree == treeStats.margin)
                    hexPrefab = mediumWoodsPrefab;
                else if (tree == treeStats.margin - 1)
                    hexPrefab = lightWoodsPrefab;
                else if (brush >= brushStats.margin + 1)
                {
                    hexPrefab = heavyBrushPrefab;
                }
                else if (brush == brushStats.margin)
                    hexPrefab = brushPrefab;

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


        SetElevation();
        SetInputReader();
    }

    public void VerifyHexes() {

        int rowi = 0;
        int coli = 0;

        foreach (var row in hexes) {

            foreach (var hex in row) {

                string hexName = rowi + ", " + coli;

                if (hexName != hex.name) {

                    Debug.LogError("Hex name doesn't equal hex.name: "+hexName+", name 2: "+hex.name);

                }

                if (hex.GetComponent<HexCord>() == null)
                {
                    hex.GetComponentInChildren<HexCord>().x = rowi;
                    hex.GetComponentInChildren<HexCord>().y = coli;
                }
                else
                {
                    hex.GetComponent<HexCord>().x = rowi;
                    hex.GetComponent<HexCord>().y = coli;
                }

                coli++;
            }
            coli = 0;

            rowi++; 
        }

    }

    public void SetInputReader() {

        VerifyHexes();

        GetComponent<Tilemap>().initTilemap();
        inputReader = new InputReader(GetComponent<Tilemap>());
        inputGrid = inputReader.ReadInputToGrid();

        Debug.Log("---Reading Input Grid---");

        for (int row = 0; row < inputGrid.Length; row++) {
            for (int col = 0; col < inputGrid[0].Length; col++) {
                Debug.Log("Row: "+row+", Col: "+col+" tile name: "+inputGrid[row][col].Value.hexType);
            }
        }

        Debug.Log("---Reading Input Grid Finished---");

        valueManager = new ValueManager<TileBase>(inputGrid);

        Debug.Log("---Value Manager---");

        StringBuilder builder = null;
        List<string> list = new List<string>();
        for (int r = -1; r <= inputGrid.Length; r++)
        {
            builder = new StringBuilder();

            for (int c = -1; c <= inputGrid[0].Length; c++)
            {
                builder.Append(valueManager.GetGridValuesIncludingOffset(c, r) + " ");
            }

            list.Add(builder.ToString());

        }

        list.Reverse();
        foreach (var str in list)
        {
            Debug.Log(str);
        }

        Debug.Log("---Value Manager Finished---");

        patternManager = new PatternManager(patternSize);
        patternManager.ProcessGrid(valueManager, false);

        Debug.Log("---Pattern Neighbours in Direction---");

        foreach (Direction dir in Enum.GetValues(typeof(Direction)))
        {
            Debug.Log(dir.ToString() + " " + string.Join(" ", patternManager.GetPossibleNeighoursForPatternInDirection(0, dir).ToArray()));
        }

        Debug.Log("---Pattern Neighbours in Direction Finished---");

    }


    public int createAdditionalGrids = 1;

    public void RunWFC() {

        Debug.Log("Start WFC");

        var createTempWatch = new MapGeneratorBenchmark();
        var wfcWatch = new MapGeneratorBenchmark();
        var elevationWatch = new MapGeneratorBenchmark();
        createTempWatch.Start();
        
        if (createAdditionalGrids > 1) {
            ClearMap();
            AddTempHexesForWFC(createAdditionalGrids * wfcOutputWidth, 
                createAdditionalGrids * wfcOutputHeight);
        }

        createTempWatch.Stop();
        Debug.Log("Create Temp Finished, " + createTempWatch.PrintTime());

        wfcWatch.Start();

        CreateGridsWfc();

        wfcWatch.Stop();
        Debug.Log("Create Grids Finished "+wfcWatch.PrintTime());

        elevationWatch.Start();
        SetElevation();
        elevationWatch.Stop();
        Debug.Log("Set Elevation Time, "+elevationWatch.PrintTime());
    }

    void CreateGridsWfc() {
        int createdGridCount = 0;
        int gridsToCreate = createAdditionalGrids * createAdditionalGrids;
        float timeElapsed = 0f;

        Debug.Log("Create Grids: " + gridsToCreate);

        for (int x = 0; x < createAdditionalGrids; x++)
        {

            for (int y = 0; y < createAdditionalGrids; y++)
            {
                var gridWatch = new MapGeneratorBenchmark();
                gridWatch.Start();

                WFCCore core = new WFCCore(wfcOutputWidth, wfcOutputHeight, maximumIterations, patternManager);

                Tilemap outputTileMap = new Tilemap();
                outputTileMap.initTilemap();

                TileMapOutput output = new TileMapOutput(valueManager, outputTileMap);
                var result = core.CreateOutputGrid();

                output.CreateOutput(patternManager, result, wfcOutputWidth, wfcOutputHeight);

                // needs offset params 
                output.OutputImage.CreateTiles(wfcOutputHeight, wfcOutputWidth, x * wfcOutputWidth, y * wfcOutputHeight);

                gridWatch.Stop();
                timeElapsed += gridWatch.GetTimeMs();
                createdGridCount++;
                Debug.Log("Finished Grid, " + gridWatch.PrintTime() + ", Created Grids: "+createdGridCount+"/"+gridsToCreate+", EST Time Remaining: "
                    +(timeElapsed/createdGridCount));
            }

        }
    }

    void SetElevation() {
        //var elevationStats = new HexFrequency(elevationFrequency);
        var w = createAdditionalGrids * wfcOutputWidth;
        var h = createAdditionalGrids * wfcOutputHeight;
        var elevationMap = ElevationCalculator.GetElevationMap(w,h, elevationDistribution, elevationHeightRange);

        for (int x = 0; x < w; x++)
        {

            for (int y = 0; y < h; y++)
            {
                int elevation = elevationMap[x][y];
                var hex = hexes[x][y];
                var cord = HexCord.GetHexCord(hex);
                cord.elevation = elevation;
            }
        }
    }

    void AddTempHexesForWFC(int width, int height) {

        for (int x = 0; x < width; x++)
        {

            var row = new List<GameObject>();

            for (int y = 0; y < height; y++)
            {

                var obj = new GameObject();
                obj.AddComponent<HexCord>();
                GameObject hex = Instantiate(obj);

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
                    hex.GetComponentInChildren<HexCord>().urbanHex = false;
                    hex.GetComponentInChildren<HexCord>().x = x;
                    hex.GetComponentInChildren<HexCord>().y = y;
                    hex.GetComponentInChildren<HexCord>().elevation = 0;
                }
                else
                {
                    hex.GetComponent<HexCord>().urbanHex = false;
                    hex.GetComponent<HexCord>().x = x;
                    hex.GetComponent<HexCord>().y = y;
                    hex.GetComponent<HexCord>().elevation = 0;
                }


                hex.transform.parent = gameObject.transform;
                row.Add(hex);

            }

            hexes.Add(row);
        }
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

    List<List<int>> NoiseMap(HexFrequency freq, int manWidth=0, int manHeight=0)
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

        CalculatePerlin(xOffset, yOffset, magnification, density, map,
            manWidth > 0 ? manWidth : mapWidth, manHeight > 0 ? manHeight : mapHeight);

        return map;

    }
    void CalculatePerlin(int xOffset, int yOffset, float magnification, int density, List<List<int>> map,
        int width, int height) {
        for (int x = 0; x < width; x++)
        {

            List<int> values = new List<int>();

            for (int y = 0; y < height; y++)
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
