using UnityEngine;
using System.Collections;
using CreativeSpore.SuperTilemapEditor;

public class GridManager : MonoBehaviour
{
    // Making these larger means cave generation will take more trials; set them too high and the program will hang.
    const int CAVE_MIN_WIDTH = 30;
    const int CAVE_MIN_HEIGHT = 30;

    private static GridManager s_Instance = new GridManager();
    public GameObject tiledmap;
    public GameObject tiledmap_Overlay;
    public GameObject tiledmap_Mask;

    public GameObject [] tiledmaplevels=new GameObject[10];
    public GameObject [] tiledmaplevels_Overlay = new GameObject[10];



    public  int maxwidth = 200;
    public  int maxheight = 200;

    public int[,] ArmyMap;// = new short[maxwidth, maxheight];
    public int[,] ImportanceMap;/// = new short[maxwidth, maxheight];

    public float gridCellSize = 0.16f;
    public bool showGrid = true;
    public bool showObstacleBlocks = true;
    private Vector3 origin = new Vector3(0, 0, 0);
    public Node[,] nodes { get; set; }

    const int OVERLAY_TYPE_COUNT = 6;

    const int DCOLS = 100;
    const int DROWS = 100;

    int caveX, caveY, caveWidth, caveHeight;


    short[,] nbDirs = { { 0, -1 }, { 0, 1 }, { -1, 0 }, { 1, 0 }, { -1, -1 }, { -1, 1 }, { 1, -1 }, { 1, 1 } };
    short[,] cDirs = { { 0, 1 }, { 1, 1 }, { 1, 0 }, { 1, -1 }, { 0, -1 }, { -1, -1 }, { -1, 0 }, { -1, 1 } };


    const int HORIZONTAL_CORRIDOR_MIN_LENGTH = 5;
    const int HORIZONTAL_CORRIDOR_MAX_LENGTH= 15;
    const int VERTICAL_CORRIDOR_MIN_LENGTH= 2;
    const int VERTICAL_CORRIDOR_MAX_LENGTH = 9;

    const uint tileWater = (8 << 16);
    const uint tileWaterPlants = (24 << 16);
    const uint tileDarkGrass = 66;
    const uint tileGrass = (9 << 16);
    const uint tileFlowers = (22 << 16);
    const uint tileTrees = (23 << 16);
    const uint tileHills = 1080;
    const uint tileunderRoads = (17 << 16);

    public int NowThisID = Global_const.NONE_ID;

    public Vector3 TargetVec3 = new Vector3();

    enum directions
    {
        NO_DIRECTION = -1,
        // Cardinal directions; must be 0-3:
        UP = 0,
        DOWN = 1,
        LEFT = 2,
        RIGHT = 3,
        // Secondary directions; must be 4-7:
        UPLEFT = 4,
        DOWNLEFT = 5,
        UPRIGHT = 6,
        DOWNRIGHT = 7
    };

    public bool ISNeighbour(Vector3 _vec1, Vector3 _vec2)
    {
        if (System.Math.Abs(_vec1.x - _vec2.x) == 1
        || System.Math.Abs(_vec1.y - _vec2.y) == 1)
        {
            return true;
        }

        return false;
    }

    public void SetMap(int [,]_map,Vector3 _vec3,int _id)
    {
        int x = (int)_vec3.x;
        int y = (int)_vec3.y;

        if(x<0||x>=maxwidth||y<0||y>=maxheight)
        {
            Debug.Log("map 越界");
            return;
        }

        _map[x, y] = _id;
    }

    private GridManager()
    {
        
    }

    public void Init()
    {
        try
        {
            tiledmap = GameObject.Find("TilemapGroup/Ground").gameObject;
            tiledmap_Overlay = GameObject.Find("TilemapGroup/Ground Overlay").gameObject;
            tiledmap_Mask = GameObject.Find("TilemapGroup/mask").gameObject;

            tiledmaplevels[1] = GameObject.Find("TilemapGroup/level1").gameObject;
            tiledmaplevels_Overlay[1] = GameObject.Find("TilemapGroup/level1Overlay").gameObject;

            gridCellSize = tiledmap.GetComponent<Tilemap>().CellSize.x;
            maxwidth = tiledmap.GetComponent<Tilemap>().GridWidth;
            maxheight = tiledmap.GetComponent<Tilemap>().GridHeight;

            ArmyMap = new int[maxwidth, maxheight];
            ImportanceMap = new int[maxwidth, maxheight];

            for (int i = 0; i < maxwidth; i++)
            {
                for (int j = 0; j < maxheight; j++)
                {
                    ArmyMap[i, j] = Global_const.NONE_ID;
                    ImportanceMap[i, j] = Global_const.NONE_ID;

                }
            }

            //obstacleList = GameObject.FindGameObjectsWithTag("Obstacle");
            CalculateObstacles();

        }
        catch
        {

        }
    } 

    public static GridManager getInstance()
    {

        return s_Instance;
    }


    public Vector3 Origin
    {
        get { return origin; }
    }

    void Awake()
    {

    }
    // Find all the obstacles on the map  
    void CalculateObstacles()
    {
        nodes = new Node[maxwidth, maxheight];
        //int index = 0;
        for (int i = 0; i < maxwidth; i++)
        {
            for (int j = 0; j < maxheight; j++)
            {
                Vector3 cellPos = new Vector3(i, j, 0);
                Node node = new Node(cellPos);
                nodes[i, j] = node;
            }
        }

       
    }

   
    public bool IsInBounds(Vector3 pos)
    {
        float width = maxwidth;
        float height = maxheight;
        return (pos.x >= Origin.x && pos.x <= Origin.x + width &&
                pos.x <= Origin.y + height && pos.y >= Origin.y);
    }

    /*  public int GetRow(int index)
      {
          int row = index / numOfColumns;
          return row;
      }
      public int GetColumn(int index)
      {
          int col = index % numOfColumns;
          return col;
      }
      */
    /* public Vector2 GetVec2(int index)
     {
         return new Vector2(GetColumn(index),GetRow(index));
     }
     */
    public void GetNeighbours(Node node, ArrayList neighbors)
    {
        Vector3 neighborPos = node.position;
        //int neighborIndex = GetGridIndex(neighborPos);
        //int row = GetRow(neighborIndex);
        //int column = GetColumn(neighborIndex);
        //Bottom  
        int tempx = (int)(neighborPos.x - 1);
        int tempy = (int)neighborPos.y;
        AssignNeighbour(tempx, tempy, neighbors);
        //Top  
        tempx = (int)(neighborPos.x + 1);
        tempy = (int)neighborPos.y;
        AssignNeighbour(tempx, tempy, neighbors);
        //Right  
        tempx = (int)neighborPos.x;
        tempy = (int)neighborPos.y + 1;
        AssignNeighbour(tempx, tempy, neighbors);
        //Left  
        tempx = (int)neighborPos.x;
        tempy = (int)neighborPos.y - 1;
        AssignNeighbour(tempx, tempy, neighbors);
    }

    void AssignNeighbour(int _tempx, int _tempy, ArrayList neighbors)
    {

        

        if (_tempx > -1 && _tempy > -1 &&
            _tempx < maxwidth && _tempy < maxheight)
        {
            Node nodeToAdd = nodes[_tempx, _tempy];

            //如果是目的地直接返回
            if (TargetVec3.x == _tempx && TargetVec3.y == _tempy)
            {
                neighbors.Add(nodeToAdd);
                return;
            }

            Tile tiletemp = tiledmap.GetComponent<Tilemap>().GetTile(_tempx, _tempy);

            int targetarmyID = ArmyMap[_tempx, _tempy];
            if(targetarmyID != Global_const.NONE_ID)
            {
                herodata targetplacehero = Global_HeroData.getInstance().List_hero[targetarmyID];
                //如果对方没有国籍
                if (targetplacehero.m_relationship.belong_kindom!=Global_const.NONE_ID)
                {
                    KindomData targetkindom = Global_KindomData.getInstance().list_KindomData[targetplacehero.m_relationship.belong_kindom];

                    if(NowThisID!=Global_const.NONE_ID)
                    {
                        herodata NowThisHero = Global_HeroData.getInstance().List_hero[NowThisID];
                        if(NowThisHero.m_relationship.belong_kindom!=Global_const.NONE_ID)
                        {
                            KindomData thiskindom = Global_KindomData.getInstance().list_KindomData[NowThisHero.m_relationship.belong_kindom];

                            if(thiskindom.Kindomrelation[targetkindom.id]>0|| thiskindom.id== targetkindom.id)
                            {
                                return;
                            }

                        }

                    }


                }

            }
            /* Tilemap tilemap = GridManager.getInstance().tiledmap.GetComponent<Tilemap>();

             tilemap.SetTileData(_tempx, _tempy, (uint)(0));
             tilemap.UpdateMesh();
             */
            //Debug.Log("column" + column + " row" + row);
            if (tiletemp != null)
            {
                int walkable_INT = tiletemp.paramContainer.GetIntParam("walkable");

                //Debug.Log("walkable_INT:" + walkable_INT);

                if (walkable_INT == -1)
                {

                }
                else
                {
                    neighbors.Add(nodeToAdd);
                }
            }
            else
            {
                neighbors.Add(nodeToAdd);

            }
        }
    }


    public Vector3 FromWorldVec2TiledVec(Vector3 _Vec)
    {
        Vector3 tempvec = new Vector3(0, 0, 0);

        tempvec.x = (int)(_Vec.x / gridCellSize);
        tempvec.y = (int)(_Vec.y / gridCellSize);
        tempvec.z = 0;

        return tempvec;

    }

    public Vector3 FromTiledVec2WorldVec(Vector3 _Vec)
    {
        Vector3 tempvec = new Vector3(0, 0, 0);

        tempvec.x = _Vec.x * gridCellSize + gridCellSize / 2.0f;
        tempvec.y = _Vec.y * gridCellSize + gridCellSize / 2.0f;
        tempvec.z = 0;

        return tempvec;

    }

    public bool TestMove(Vector3 _vec3)
    {

        Tile tiletemp = tiledmap.GetComponent<Tilemap>().GetTile((int)_vec3.x, (int)_vec3.y);
        
        if (tiletemp != null)
        {
            
            int walkable_INT = tiletemp.paramContainer.GetIntParam("walkable");

            if (walkable_INT == -1)
            {
                return false;
            }
            else
            {
                return true;

            }
        }
        else
        {

            return true;

        }

        return true;
    }

    public void OpenMask(Vector3 _vec3, int fieldofvision)
    {


        for (int x = -fieldofvision; x <= fieldofvision; x++)
        {
            for (int y = -fieldofvision; y <= fieldofvision; y++)
            {
                if ((Mathf.Abs(x) + Mathf.Abs(y)) <= fieldofvision)
                {

                    //Debug.Log("x:" + (int)(_vec3.x + x) + " y:" + (int)(_vec3.y + y));

                    //tilemapmask.SetTileData((int)(_vec3.x + x), (int)(_vec3.y + y), (uint)(1));

                    tiledmap_Mask.GetComponent<Tilemap>().SetTileData((int)(_vec3.x + x), (int)(_vec3.y + y), (uint)(1));

                }

            }

        }


        /* for(int i=0;i<20;i++)
         {
             for(int j=0;j<20;j++)
             {
                 tiledmap_Overlay.GetComponent<Tilemap>().SetTileData(i, j, tileHills);

             }
         }
         */
        tiledmap_Mask.GetComponent<Tilemap>().UpdateMesh();

    }

    /// <summary>
    /// 
    /// </summary>
    public void GenerateMap()
    {
        //Ground.ClearMap();
        //GroundOverlay.ClearMap();

        float now;
        now = Time.realtimeSinceStartup;
        float fDiv = 25f;
        float xf = Random.value * 100;
        float yf = Random.value * 100;

        //*/ Rogue Demo (280ms with 180x180)

        //*/

        // Oryx Demo
        /*/
        uint tileWater = (9 << 16);
        uint tileWaterPlants = (16 << 16);
        uint tileDarkGrass = 757;
        uint tileGrass = 756;
        uint tileFlowers = (17 << 16);
        uint tileMountains = (18 << 16);
        //*/

        for (int i = 0; i < maxwidth; i++)
        {
            for (int j = 0; j < maxheight; j++)
            {
                Tile tiletemp = tiledmap.GetComponent<Tilemap>().GetTile(i,j);


                if (tiletemp != null)
                {
                    int walkable_INT = tiletemp.paramContainer.GetIntParam("walkable");

                    if (walkable_INT == -1)
                    {
                        continue;

                    }
                    else
                    {

                    }
                }

                float fRand = Random.value;
                float noise = Mathf.PerlinNoise((i + xf) / fDiv, (j + yf) / fDiv);
                //Debug.Log( "noise: "+noise+"; i: "+i+"; j: "+j );
                if (noise < 0.1) //water
                {
                    tiledmap.GetComponent<Tilemap>().SetTileData(i, j, tileWater);
                }
                else if (noise < 0.2) // water plants
                {
                    tiledmap.GetComponent<Tilemap>().SetTileData(i, j, tileWater);
                    if (fRand < noise / 3)
                        tiledmap_Overlay.GetComponent<Tilemap>().SetTileData(i, j, tileWaterPlants);
                }
                else if (noise < 0.5 && fRand < (1 - noise / 2)) // dark grass
                {
                    tiledmap.GetComponent<Tilemap>().SetTileData(i, j, tileDarkGrass);
                }
                else if (noise < 0.6 && fRand < (1 - 1.2 * noise)) // flowers
                {
                    tiledmap.GetComponent<Tilemap>().SetTileData(i, j, tileGrass);
                    tiledmap_Overlay.GetComponent<Tilemap>().SetTileData(i, j, tileFlowers);
                }
                else if (noise < 0.7) // grass
                {
                    tiledmap.GetComponent<Tilemap>().SetTileData(i, j, tileGrass);
                }
                else // mountains
                {
                    tiledmap.GetComponent<Tilemap>().SetTileData(i, j, tileGrass);
                    tiledmap_Overlay.GetComponent<Tilemap>().SetTileData(i, j, tileTrees);
                }
            }
        }


       for(int i=0;i<Global_const.MAXIMPORTANCE;i++)
        {
            int activetemp =int.Parse(Global_ImportanceData.getInstance().List_importance[i].this_Elem.GetAttribute("Active"));
            if(activetemp==0)
            {
                continue;
            }

            int x =int.Parse(Global_ImportanceData.getInstance().List_importance[i].this_Elem.GetAttribute("x"));
            int y =int.Parse(Global_ImportanceData.getInstance().List_importance[i].this_Elem.GetAttribute("y"));


            GridManager.getInstance().carveDungeon(x,y);


            for (int tempx=x-2;tempx<x+2;tempx++)
            {
                for (int tempy =y-2; tempy <y +2; tempy++)
                {
                    if(tempx<0||tempx>maxwidth)
                    {
                        continue;
                    }

                    if (tempy < 0 || tempy > maxheight)
                    {
                        continue;
                    }

                    tiledmap.GetComponent<Tilemap>().SetTileData(tempx,tempy, tileGrass);


                }

            }

        }

        tiledmap.GetComponent<Tilemap>().UpdateMesh();
        tiledmap_Overlay.GetComponent<Tilemap>().UpdateMesh();


        tiledmaplevels_Overlay[1].GetComponent<Tilemap>().UpdateMesh();

    }

    public void carveDungeon(int _x,int _y)
    {
        int roomsBuilt, roomsAttempted;
        int[] roomMap;
        int[,] doorSites = new int[4,2];

        int i, x, y;
        int[] sCoord = new int[DCOLS * DROWS];
        int descentPercent = Mathf.Clamp(100, 0, 100);

        int[] grid = new int[DCOLS * DROWS];
        int dir, oppDir;
        // Room frequencies:
        //      1. Cross room
        //      2. Circular room
        //      3. Chunky room
        //      4. Cave
        //      5. Cavern (the kind that fills a level)
        //      6. Entrance room (the big upside-down T room at the start of depth 1)

        //房间出现的频率
        //十字型房间
        //环形房间
        //Chunky room
        //洞窟
        //Cavern (the kind that fills a level)
        //大厅（第一层的T型房间）

        int[] roomFrequencies = {
           0, //2 + 20 * (100 - descentPercent) / 100,
           0,// 1 + 7 * (100 - descentPercent) / 100,
           0, //7,
            100,//1 + 10 * descentPercent / 100,
            0,
            0};

        int[] firstRoomFrequencies = { 10, 3, 7, 10, 10 + 50 * descentPercent / 100, 0 };

        short corridorPercent = (short)(10 + 80 * (100 - descentPercent) / 100);


        fillSequentialList(sCoord, DCOLS * DROWS);
        shuffleList(sCoord, DCOLS * DROWS);

        //如果当前层为第一层，则制造大厅房间
       designRandomRoom(grid, false, null, firstRoomFrequencies);


        //进行35次尝试建造房间
       /* roomMap = allocGrid();

        for (roomsBuilt = roomsAttempted = 0; roomsBuilt < 35 && roomsAttempted < 35; roomsAttempted++) {
            // Build a room in hyperspace.
            fillGrid(roomMap, 0);

            //设计一个随机房间（尚未放置）
            designRandomRoom(roomMap, roomsAttempted <= 30 && rand_percent(corridorPercent), doorSites, roomFrequencies);


            // Slide hyperspace across real space, 
            //in a random but predetermined order, 
            //until the room matches up with a wall.

            //以一种随机但是预设的法则将房间在地图中进行摆放
            //直到房间和一面墙吻合

            for (i = 0; i < DCOLS * DROWS; i++) {
                x = sCoord[i] / DROWS;
                y = sCoord[i] % DROWS;

                // If the indicated tile is a wall on the room stored in grid, 
                // and it could be the site of
                //a door out of that room, 
                //then return the outbound direction that the door faces.
                // Otherwise, return NO_DIRECTION.

                //遍历所有网格，如果当前网格是房间的一面墙，且可以被设定成一扇出房间的门
                //则返回门的朝向
                //否则返回无方向

                dir = directionOfDoorSite(grid, x, y);
                oppDir = oppositeDirection(dir);

                //如果返回不是无朝向，则根据这个门放置房间
                if (dir != (int)directions.NO_DIRECTION
                    && doorSites[oppDir,0] != -1
                    && roomFitsAt(grid, roomMap, x - doorSites[oppDir,0], y - doorSites[oppDir,1])) {


                    insertRoomAt(grid, roomMap, x - doorSites[oppDir,0], y - doorSites[oppDir,1]);
                    grid[x*DROWS+y] = 2; // Door site.

                    roomsBuilt++;
                    break;
                }
            }
        }
        */

        //    colorOverDungeon(&darkGray);
        //    hiliteGrid(grid, &white, 100);
        //    temporaryMessage("How does this finished level look?", true);
        //*/ Rogue Demo (280ms with 180x180)


        //herodata nowhero = Global_HeroData.getInstance().List_hero[GlobalData.getInstance().nowheroid];


        // int[] grid = new int[DCOLS * DROWS];
        for (i=0;i<DCOLS;i++)
        {
            //string tempstr = "";
            for (int j = 0; j < DROWS; j++)
            {


                // tempstr += grid[i * DROWS + j] + " ";
                if(grid[i * DROWS + j]==1)
                {
                    for(int tempi=i-1;tempi<i+1;tempi++)
                    {
                        for (int tempj = j - 1; tempj < j + 1; tempj++)
                        {

                            tiledmap.GetComponent<Tilemap>().SetTileData(tempi + _x - DCOLS / 2, tempj + _y - DROWS / 2, tileGrass);

                        }

                    }
                    
                    tiledmap_Overlay.GetComponent<Tilemap>().SetTileData(i + _x - DCOLS / 2, j + _y - DROWS / 2, tileHills);
                    tiledmaplevels_Overlay[1].GetComponent<Tilemap>().SetTileData(i + _x - DCOLS / 2, j + _y - DROWS / 2, tileunderRoads);
                }


            }

            //Debug.Log(tempstr+"\n");

        }

       // tiledmap.GetComponent<Tilemap>().UpdateMesh();
       // tiledmap_Overlay.GetComponent<Tilemap>().UpdateMesh();

    }


    public void createRadommap()
    {


    }

    bool roomFitsAt(int [] dungeonMap, int[] roomMap, int roomToDungeonX, int roomToDungeonY)
    {
        int xRoom, yRoom, xDungeon, yDungeon, i, j;

        for (xRoom = 0; xRoom < DCOLS; xRoom++)
        {
            for (yRoom = 0; yRoom < DROWS; yRoom++)
            {
                if (roomMap[xRoom*DROWS+yRoom]>0)
                {
                    xDungeon = xRoom + roomToDungeonX;
                    yDungeon = yRoom + roomToDungeonY;

                    for (i = xDungeon - 1; i <= xDungeon + 1; i++)
                    {
                        for (j = yDungeon - 1; j <= yDungeon + 1; j++)
                        {
                            if (!coordinatesAreInMap(i, j)
                                || (dungeonMap[i*DROWS+j]>0))
                            {
                                return false;
                            }
                        }
                    }
                }
            }
        }
        return true;
    }

    void fillSequentialList(int[] list, int listLength)
    {
        int i;
        for (i = 0; i < listLength; i++)
        {
            list[i] = i;
        }
    }

    void shuffleList(int[] list, int listLength)
    {
        int i, r, buf;
        for (i = 0; i < listLength; i++)
        {
            r = Random.Range(0, listLength - 1);
            if (i != r)
            {
                buf = list[r];
                list[r] = list[i];
                list[i] = buf;
            }
        }
    }

    // Put a random room shape somewhere on the binary grid, and optionally record the coordinates of up to four door sites in doorSites.
    // If attachHallway is true, then it will bolt a perpendicular hallway onto the room at one of the four standard door sites,
    // and then relocate three of the door sites to radiate from the end of the hallway. (The fourth is defunct.)
    // RoomTypeFrequencies specifies the probability of each room type, in the following order:
    //      1. Cross room
    //      2. Circular room
    //      3. Chunky room
    //      4. Cave
    //      5. Cavern (the kind that fills a level)
    //      6. Entrance room (the big upside-down T room at the start of depth 1)

    void designRandomRoom(int[] _grid, bool attachHallway, int[,] doorSites, int[] roomTypeFrequencies) {
        int randIndex, i, sum;
        int dir;
        sum = 0;
        for (i = 0; i < OVERLAY_TYPE_COUNT; i++) {
            sum += roomTypeFrequencies[i];
        }
        randIndex = Random.Range(0, sum - 1);
        for (i = 0; i < OVERLAY_TYPE_COUNT; i++) {
            if (randIndex < roomTypeFrequencies[i]) {
                break; // "i" is our room type.
            } else {
                randIndex -= roomTypeFrequencies[i];
            }
        }
        switch (i) {
            case 0:
                designCrossRoom(_grid);
                break;
            case 1:
                designCircularRoom(_grid);
                break;
            case 2:
                designChunkyRoom(_grid);
                break;
            case 3:
                switch (Random.Range(0, 2)) {
                    case 0:
                        designCavern(_grid, 3, 12, 4, 8); // Compact cave room.
                        break;
                    case 1:
                        designCavern(_grid, 3, 12, 15, DROWS - 2); // Large north-south cave room.
                        break;
                    case 2:
                        designCavern(_grid, 20, DROWS - 2, 4, 8); // Compact cave room.
                        break;
                    default:
                        break;
                }
                break;
            case 4:
                designCavern(_grid, CAVE_MIN_WIDTH, DCOLS - 2, CAVE_MIN_HEIGHT, DROWS - 2);
                break;
            case 5:
                designEntranceRoom(_grid);
                break;
            default:
                break;
        }

        if (doorSites != null) {
            chooseRandomDoorSites(_grid, doorSites);
            if (attachHallway) {
                dir = Random.Range(0, 3);
                for (i = 0; doorSites[dir,0] == -1 && i < 3; i++) {
                    dir = (dir + 1) % 4; // Each room will have at least 2 valid directions for doors.
                }
                attachHallwayTo(_grid, doorSites);
            }
        }
    }

    void designCrossRoom(int[] _grid)
    {
        int roomWidth, roomHeight, roomWidth2, roomHeight2, roomX, roomY, roomX2, roomY2;

        fillGrid(_grid, 0);

        roomWidth = Random.Range(3, 12);
        roomX = Random.Range(Mathf.Max(0, DCOLS / 2 - (roomWidth - 1)), Mathf.Min(DCOLS, DCOLS / 2));
        roomWidth2 = Random.Range(4, 20);
        roomX2 = (roomX + (roomWidth / 2) + Random.Range(0, 2) + Random.Range(0, 2) - 3) - (roomWidth2 / 2);

        roomHeight = Random.Range(3, 7);
        roomY = (DROWS / 2 - roomHeight);

        roomHeight2 = Random.Range(2, 5);
        roomY2 = (DROWS / 2 - roomHeight2 - (Random.Range(0, 2) + Random.Range(0, 1)));

        drawRectangleOnGrid(_grid, roomX - 5, roomY + 5, roomWidth, roomHeight, 1);
        drawRectangleOnGrid(_grid, roomX2 - 5, roomY2 + 5, roomWidth2, roomHeight2, 1);
    }

    void designCircularRoom(int[] _grid)
    {
        int radius;

        if (rand_percent(5))
        {
            radius = Random.Range(4, 10);
        }
        else
        {
            radius = Random.Range(2, 4);
        }

        fillGrid(_grid, 0);
        drawCircleOnGrid(_grid, DCOLS / 2, DROWS / 2, radius, 1);
    }

    void designChunkyRoom(int[] _grid)
    {
        int i, x, y;
        int minX, maxX, minY, maxY;
        int chunkCount = Random.Range(2, 8);

        fillGrid(_grid, 0);
        drawCircleOnGrid(_grid, DCOLS / 2, DROWS / 2, 2, 1);
        minX = DCOLS / 2 - 3;
        maxX = DCOLS / 2 + 3;
        minY = DROWS / 2 - 3;
        maxY = DROWS / 2 + 3;

        for (i = 0; i < chunkCount;)
        {
            x = Random.Range(minX, maxX);
            y = Random.Range(minY, maxY);
            if (_grid[x * DROWS + y] != 0)
            {
                //            colorOverDungeon(&darkGray);
                //            hiliteGrid(grid, &white, 100);

                drawCircleOnGrid(_grid, x, y, 2, 1);
                i++;
                minX = Mathf.Max(1, Mathf.Min(x - 3, minX));
                maxX = Mathf.Min(DCOLS - 2, Mathf.Max(x + 3, maxX));
                minY = Mathf.Max(1, Mathf.Min(y - 3, minY));
                maxY = Mathf.Min(DROWS - 2, Mathf.Max(y + 3, maxY));

                //            hiliteGrid(grid, &green, 50);
                //            temporaryMessage("Added a chunk:", true);
            }
        }
    }

    void designCavern(int[] _grid, int minWidth, int maxWidth, int minHeight, int maxHeight)
    {
        int destX, destY;
        int[] blobGrid;
        blobGrid = new int[DCOLS * DROWS];

        fillGrid(blobGrid, 0);

        createBlobOnGrid(blobGrid, 5, minWidth, minHeight, maxWidth, maxHeight, 55, "ffffffttt".ToCharArray(), "ffffttttt".ToCharArray());

        //    colorOverDungeon(&darkGray);
        //    hiliteGrid(blobGrid, &tanColor, 80);
        //    temporaryMessage("Here's the cave:", true);
        // Position the new cave in the middle of the grid...

        destX = (DCOLS - caveWidth) / 2;
        destY = (DROWS - caveHeight) / 2;
        // ...and copy it to the master grid.
        insertRoomAt(_grid, blobGrid, destX - caveX, destY - caveY);
    }

    void fillGrid(int[] _grid, int fillValue)
    {
        short i, j;

        for (i = 0; i < DCOLS; i++)
        {
            for (j = 0; j < DROWS; j++)
            {
                _grid[i * DROWS + j] = fillValue;
            }
        }
    }

    void drawRectangleOnGrid(int[] _grid, int x, int y, int width, int height, int value)
    {
        int i, j;

        for (i = x; i < x + width; i++)
        {
            for (j = y; j < y + height; j++)
            {
                _grid[i * height + j] = value;
            }
        }
    }

    void drawCircleOnGrid(int[] _grid, int x, int y, int radius, int value)
    {
        int i, j;

        for (i = Mathf.Max(0, x - radius - 1); i < Mathf.Max(DCOLS, x + radius); i++)
        {
            for (j = Mathf.Max(0, y - radius - 1); j < Mathf.Max(DROWS, y + radius); j++)
            {
                if ((i - x) * (i - x) + (j - y) * (j - y) < radius * radius + radius)
                {
                    _grid[i * DCOLS + j] = value;
                }
            }
        }
    }

    // Get a random int between lowerBound and upperBound, inclusive
    bool rand_percent(short percent)
    {
        return (Random.Range(0, 99) < Mathf.Clamp(percent, 0, 100));
    }

    // Loads up **grid with the results of a cellular automata simulation.
    void createBlobOnGrid(int[] _blobGrid,
                          int roundCount,
                          int minBlobWidth, int minBlobHeight,
                          int maxBlobWidth, int maxBlobHeight, short percentSeeded,
                          char[] birthParameters, char[] survivalParameters)
    {

        int i, j, k;
        int blobNumber, blobSize, topBlobNumber, topBlobSize;

        int topBlobMinX, topBlobMinY, topBlobMaxX, topBlobMaxY, blobWidth, blobHeight;
        //short buffer2[maxBlobWidth][maxBlobHeight]; // buffer[][] is already a global short array
        bool foundACellThisLine;

        // Generate blobs until they satisfy the minBlobWidth and minBlobHeight restraints
        // Clear buffer.
        fillGrid(_blobGrid, 0);

        do
        {
            

            // Fill relevant portion with noise based on the percentSeeded argument.
            for (i = 0; i < maxBlobWidth; i++)
            {
                for (j = 0; j < maxBlobHeight; j++)
                {
                    _blobGrid[i * DROWS + j] = (rand_percent(percentSeeded) ? 1 : 0);
                }
            }

            //        colorOverDungeon(&darkGray);
            //        hiliteGrid(grid, &white, 100);
            //        temporaryMessage("Random starting noise:", true);

            // Some iterations of cellular automata
            for (k = 0; k < roundCount; k++)
            {
                cellularAutomataRound(_blobGrid, birthParameters, survivalParameters);

                //            colorOverDungeon(&darkGray);
                //            hiliteGrid(grid, &white, 100);
                //            temporaryMessage("Cellular automata progress:", true);
            }

            //        colorOverDungeon(&darkGray);
            //        hiliteGrid(grid, &white, 100);
            //        temporaryMessage("Cellular automata result:", true);

            // Now to measure the result. These are best-of variables; start them out at worst-case values.
            topBlobSize = 0;
            topBlobNumber = 0;
            topBlobMinX = maxBlobWidth;
            topBlobMaxX = 0;
            topBlobMinY = maxBlobHeight;
            topBlobMaxY = 0;

            // Fill each blob with its own number, starting with 2 (since 1 means floor), and keeping track of the biggest:
            blobNumber = 2;

            for (i = 0; i < DCOLS; i++)
            {
                for (j = 0; j < DROWS; j++)
                {
                    if (_blobGrid[i * DROWS + j] == 1)
                    { // an unmarked blob
                      // Mark all the cells and returns the total size:
                        blobSize = fillContiguousRegion(_blobGrid, i, j, blobNumber);
                        if (blobSize > topBlobSize)
                        { // if this blob is a new record
                            topBlobSize = blobSize;
                            topBlobNumber = blobNumber;
                        }
                        blobNumber++;
                    }
                }
            }

            // Figure out the top blob's height and width:
            // First find the max & min x:
            for (i = 0; i < DCOLS; i++)
            {
                foundACellThisLine = false;
                for (j = 0; j < DROWS; j++)
                {
                    if (_blobGrid[i * DROWS + j] == topBlobNumber)
                    {
                        foundACellThisLine = true;
                        break;
                    }
                }
                if (foundACellThisLine)
                {
                    if (i < topBlobMinX)
                    {
                        topBlobMinX = i;
                    }
                    if (i > topBlobMaxX)
                    {
                        topBlobMaxX = i;
                    }
                }
            }

            // Then the max & min y:
            for (j = 0; j < DROWS; j++)
            {
                foundACellThisLine = false;
                for (i = 0; i < DCOLS; i++)
                {
                    if (_blobGrid[i * DROWS + j] == topBlobNumber)
                    {
                        foundACellThisLine = true;
                        break;
                    }
                }
                if (foundACellThisLine)
                {
                    if (j < topBlobMinY)
                    {
                        topBlobMinY = j;
                    }
                    if (j > topBlobMaxY)
                    {
                        topBlobMaxY = j;
                    }
                }
            }

            blobWidth = (topBlobMaxX - topBlobMinX) + 1;
            blobHeight = (topBlobMaxY - topBlobMinY) + 1;

        } while (blobWidth < minBlobWidth
                 || blobHeight < minBlobHeight
                 || topBlobNumber == 0);

        // Replace the winning blob with 1's, and everything else with 0's:
        for (i = 0; i < DCOLS; i++)
        {
            for (j = 0; j < DROWS; j++)
            {
                if (_blobGrid[i * DROWS + j] == topBlobNumber)
                {
                    _blobGrid[i * DROWS + j] = 1;
                }
                else
                {
                    _blobGrid[i * DROWS + j] = 0;
                }
            }
        }

        // Populate the returned variables.
        caveX = topBlobMinX;
        caveY = topBlobMinY;
        caveWidth = blobWidth;
        caveHeight = blobHeight;
    }

    void cellularAutomataRound(int[] _grid, char[] birthParameters, char[] survivalParameters)
    {
        int i, j, nbCount, newX, newY;

        int[] buffer2;

        buffer2 = new int[DCOLS * DROWS];

        copyGrid(buffer2, _grid); // Make a backup of grid in buffer2, so that each generation is isolated.

        for (i = 0; i < DCOLS; i++) {
            for (j = 0; j < DROWS; j++) {
                nbCount = 0;
                for (int dir = 0; dir < 8; dir++) {
                    newX = i + nbDirs[dir, 0];
                    newY = j + nbDirs[dir, 1];
                    if ((coordinatesAreInMap(newX, newY) == true)
                        && (buffer2[newX * DROWS + newY] > 0)) {

                        nbCount++;
                    }
                }
                if ((buffer2[i * DROWS + j] != 0) && birthParameters[nbCount] == 't') {
                    _grid[i * DROWS + j] = 1;	// birth
                } else if ((buffer2[i * DROWS + j] > 0) && survivalParameters[nbCount] == 't') {
                    // survival
                } else {
                    _grid[i * DROWS + j] = 0;	// death
                }
            }
        }

    }

    void copyGrid(int[] to, int[] from)
    {
        short i, j;

        for (i = 0; i < DCOLS; i++)
        {
            for (j = 0; j < DROWS; j++)
            {
                to[i * DROWS + j] = from[i * DROWS + j];
            }
        }
    }

    // mallocing two-dimensional arrays! dun dun DUN!
    int[] allocGrid()
    {
        int i;
        int[] array = new int[DCOLS * DROWS];
        return array;
    }

    bool coordinatesAreInMap(int _x, int _y)
    {

        return ((_x) >= 0 && (_x) < DCOLS && (_y) >= 0 && (_y) < DROWS);
    }

    // Marks a cell as being a member of blobNumber, then recursively iterates through the rest of the blob
    int fillContiguousRegion(int[] grid, int x, int y, int fillValue)
    {
        int newX, newY, numberOfCells = 1;

        grid[x * DROWS + y] = fillValue;

        // Iterate through the four cardinal neighbors.
        for (int dir = 0; dir < 4; dir++) {
            newX = x + nbDirs[dir, 0];
            newY = y + nbDirs[dir, 1];
            if (!coordinatesAreInMap(newX, newY)) {
                break;
            }
            if (grid[newX * DROWS + newY] == 1) { // If the neighbor is an unmarked region cell,
                numberOfCells += fillContiguousRegion(grid, newX, newY, fillValue); // then recurse.
            }
        }
        return numberOfCells;
    }

    void insertRoomAt(int[] dungeonMap, int[] roomMap, int roomToDungeonX, int roomToDungeonY)
    {
        int xRoom, yRoom;

        for (xRoom = 0; xRoom < DCOLS; xRoom++)
        {
            for (yRoom = 0; yRoom < DROWS; yRoom++)
            {
                if ((roomMap[xRoom * DROWS + yRoom] > 0)
                    && coordinatesAreInMap(xRoom + roomToDungeonX, yRoom + roomToDungeonY))
                {

                    dungeonMap[(xRoom + roomToDungeonX) * DROWS + (yRoom + roomToDungeonY)] = 1;
                }
            }
        }
    }

    // This is a special room that appears at the entrance to the dungeon on depth 1.
    void designEntranceRoom(int[] _grid)
    {
        int roomWidth, roomHeight, roomWidth2, roomHeight2, roomX, roomY, roomX2, roomY2;

        fillGrid(_grid, 0);

        roomWidth = 8;
        roomHeight = 10;
        roomWidth2 = 20;
        roomHeight2 = 5;
        roomX = DCOLS / 2 - roomWidth / 2 - 1;
        roomY = DROWS - roomHeight - 2;
        roomX2 = DCOLS / 2 - roomWidth2 / 2 - 1;
        roomY2 = DROWS - roomHeight2 - 2;

        drawRectangleOnGrid(_grid, roomX, roomY, roomWidth, roomHeight, 1);
        drawRectangleOnGrid(_grid, roomX2, roomY2, roomWidth2, roomHeight2, 1);
    }

    void chooseRandomDoorSites(int[] roomMap, int[,] doorSites)
    {
        int i, j, k, newX, newY;
        int dir;
        int[] grid;
        bool doorSiteFailed;

        grid = allocGrid();
        copyGrid(grid, roomMap);

        //    colorOverDungeon(&darkGray);
        //    hiliteGrid(grid, &blue, 100);
        //    temporaryMessage("Generating this room:", true);
        //    const char dirChars[] = "^v<>";

        for (i = 0; i < DCOLS; i++) {
            for (j = 0; j < DROWS; j++) {
                if (grid[i * DROWS + j] > 0) {
                    dir = directionOfDoorSite(roomMap, i, j);
                    if (dir != (int)directions.NO_DIRECTION) {
                        // Trace a ray 10 spaces outward from the door site to make sure it doesn't intersect the room.
                        // If it does, it's not a valid door site.
                        newX = i + nbDirs[dir, 0];
                        newY = j + nbDirs[dir, 1];
                        doorSiteFailed = false;
                        for (k = 0; k < 10 && coordinatesAreInMap(newX, newY) && !doorSiteFailed; k++) {
                            if (grid[newX * DROWS + newY] > 0) {
                                doorSiteFailed = true;
                            }
                            newX += nbDirs[dir, 0];
                            newY += nbDirs[dir, 1];
                        }
                        if (!doorSiteFailed) {
                            //                        plotCharWithColor(dirChars[dir], mapToWindowX(i), mapToWindowY(j), &black, &green);
                            grid[i * DROWS + j] = dir + 2; // So as not to conflict with 0 or 1, which are used to indicate exterior/interior.
                        }
                    }
                }
            }
        }

        //    temporaryMessage("Door candidates:", true);

        // Pick four doors, one in each direction, and store them in doorSites[dir].
        for (dir = 0; dir < 4; dir++)
        {
            //  randomLocationInGrid(grid,out doorSites[dir, 0]),out (doorSites[dir][1]), dir + 2);
            randomLocationInGrid(grid, out (doorSites[dir,0]),out (doorSites[dir,1]), dir + 2);
        }

    }


    int directionOfDoorSite(int[] _grid, int x, int y)
    {
        int dir, solutionDir;
        int newX, newY, oppX, oppY;
    
        if (_grid[x*DROWS+y]>0) { // Already interior
            return (int)directions.NO_DIRECTION;
        }

    solutionDir = (int)directions.NO_DIRECTION;
        for (dir=0; dir<4; dir++) {
            newX = x + nbDirs[dir,0];
            newY = y + nbDirs[dir,1];
            oppX = x - nbDirs[dir,0];
            oppY = y - nbDirs[dir,1];
            if (coordinatesAreInMap(oppX, oppY)
                && coordinatesAreInMap(newX, newY)
                && (_grid[oppX*DROWS+oppY]>0)) {
            
                // This grid cell would be a valid tile on which to place a door that, facing outward, points dir.
                if (solutionDir != (int)directions.NO_DIRECTION) {
                    // Already claimed by another direction; no doors here!
                    return (int)directions.NO_DIRECTION;
                }
                solutionDir = dir;
            }
        }
        return solutionDir;
    }

    // Takes a grid as a mask of valid locations, chooses one randomly and returns it as (x, y).
    // If there are no valid locations, returns (-1, -1).
    void randomLocationInGrid(int [] _grid,out int x, out int y, int validValue)
    {
        x = y = -1;
        int locationCount = validLocationCount(_grid, validValue);
        short i, j;

        if (locationCount <= 0)
        {
            x = y= -1;
            return;
        }
        int index = Random.Range(0, locationCount - 1);
        for (i = 0; i < DCOLS && index >= 0; i++)
        {
            for (j = 0; j < DROWS && index >= 0; j++)
            {
                if (_grid[i*DROWS+j] == validValue)
                {
                    if (index == 0)
                    {
                        x = i;
                        y = j;
                    }
                    index--;
                }
            }
        }
        return;
    }

    int validLocationCount(int[] grid, int validValue)
    {
        int i, j, count;
        count = 0;
        for (i = 0; i < DCOLS; i++)
        {
            for (j = 0; j < DROWS; j++)
            {
                if (grid[i* DROWS+j] == validValue)
                {
                    count++;
                }
            }
        }
        return count;
    }

    // If the indicated tile is a wall on the room stored in grid, and it could be the site of
    // a door out of that room, then return the outbound direction that the door faces.
    // Otherwise, return NO_DIRECTION.
    int directionOfDoorSite(int [] _grid, short x, short y)
    {
        int dir, solutionDir;
        int newX, newY, oppX, oppY;
    
        if (_grid[x*DROWS+y]>0) { // Already interior
            return (int)directions.NO_DIRECTION;
        }
    
        solutionDir = (int)directions.NO_DIRECTION;
        for (dir=0; dir<4; dir++) {
            newX = x + nbDirs[dir,0];
            newY = y + nbDirs[dir,1];
            oppX = x - nbDirs[dir,0];
            oppY = y - nbDirs[dir,1];
            if (coordinatesAreInMap(oppX, oppY)
                && coordinatesAreInMap(newX, newY)
                && (_grid[oppX*DROWS+oppY]>0)) {
            
                // This grid cell would be a valid tile on which to place a door that, facing outward, points dir.
                if (solutionDir != (int)directions.NO_DIRECTION) {
                    // Already claimed by another direction; no doors here!
                    return (int)directions.NO_DIRECTION;
                }
                solutionDir = dir;
            }
        }
        return solutionDir;
    }

    void attachHallwayTo(int [] _grid, int [,] doorSites)
    {
        int i, x, y, newX, newY;
        int []dirs=new int[4];
        int length;
        int dir=0, dir2=0;
        bool allowObliqueHallwayExit;

        // Pick a direction.
        fillSequentialList(dirs, 4);
        shuffleList(dirs, 4);
        for (i=0; i<4; i++) {
            dir = dirs[i];
            if (doorSites[dir,0] != -1
                && doorSites[dir,1] != -1
                && coordinatesAreInMap(doorSites[dir,0] + nbDirs[dir,0] * HORIZONTAL_CORRIDOR_MAX_LENGTH,
                                       doorSites[dir,1] + nbDirs[dir,1] * VERTICAL_CORRIDOR_MAX_LENGTH)) {
                    break; // That's our direction!
            }
        }
        if (i==4) {
            return; // No valid direction for hallways.
        }
    
        if (dir == (int)directions.UP || dir == (int)directions.DOWN) {
            length = Random.Range(VERTICAL_CORRIDOR_MIN_LENGTH, VERTICAL_CORRIDOR_MAX_LENGTH);
        } else {
            length = Random.Range(HORIZONTAL_CORRIDOR_MIN_LENGTH, HORIZONTAL_CORRIDOR_MAX_LENGTH);
        }
    
        x = doorSites[dir,0];
        y = doorSites[dir,1];
        for (i = 0; i<length; i++) {
            if (coordinatesAreInMap(x, y)) {
                _grid[x*DROWS+y] = 1;
            }
            x += nbDirs[dir,0];
            y += nbDirs[dir,1];
        }
        x = Mathf.Clamp(x - nbDirs[dir,0], 0, DCOLS - 1);
    y = Mathf.Clamp(y - nbDirs[dir,1], 0, DROWS - 1); // Now (x, y) points at the last interior cell of the hallway.
    allowObliqueHallwayExit = rand_percent(15);
        for (dir2 = 0; dir2< 4; dir2++) {
            newX = x + nbDirs[dir2,0];
            newY = y + nbDirs[dir2,1];
        
            if ((dir2 != dir && !allowObliqueHallwayExit)
                || !coordinatesAreInMap(newX, newY)
                || (_grid[newX*DROWS+newY]>0)) {
            
                doorSites[dir2,0] = -1;
                doorSites[dir2,1] = -1;
            } else {
                doorSites[dir2,0] = newX;
                doorSites[dir2,1] = newY;
            }
        }
    }

    int oppositeDirection(int theDir)
    {
        switch (theDir)
        {
            case (int)directions.UP:
                return (int)directions.DOWN;
            case (int)directions.DOWN:
                return (int)directions.UP;
            case (int)directions.LEFT:
                return (int)directions.RIGHT;
            case (int)directions.RIGHT:
                return (int)directions.LEFT;
            case (int)directions.UPRIGHT:
                return (int)directions.DOWNLEFT;
            case (int)directions.DOWNLEFT:
                return (int)directions.UPRIGHT;
            case (int)directions.UPLEFT:
                return (int)directions.DOWNRIGHT;
            case (int)directions.DOWNRIGHT:
                return (int)directions.UPLEFT;
            case (int)directions.NO_DIRECTION:
                return (int)directions.NO_DIRECTION;
            default:
                return -1;
        }
    }
}