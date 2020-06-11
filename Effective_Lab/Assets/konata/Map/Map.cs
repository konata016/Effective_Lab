using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public enum ObjType
    {
        Nothing, Wall, Goal, Start, ProhibitedArea
    }

    public enum MapDataArrLength { Width, Depth }

    [Header("生成する範囲")]
    [SerializeField] int w = 200;
    [SerializeField] int d = 200;

    [SerializeField] float h = 1.5f;
    [SerializeField] float siz = 5f;

    [Header("オブジェクトが埋まった時に道を作る")]
    [SerializeField] int roadSpace = 5;
    [SerializeField] int roadLength = 30;

    [Header("ノイズの細かさ")]
    [SerializeField] float chaos = 30f;

    [Header("オブジェクト")]
    [SerializeField] GameObject frameObj;
    [SerializeField] GameObject wallObj;

    [System.Serializable]
    public class EventObj
    {
        [HideInInspector] public string name;
        [HideInInspector] public ObjType objType;
        public GameObject obj;
    }
    [Header("イベントオブジェクト"),SerializeField]
    List<EventObj> inspectorExclusive = new List<EventObj>() {
        new EventObj{name=ObjType.Start.ToString(),objType=ObjType.Start },
        new EventObj{name=ObjType.Goal.ToString(),objType=ObjType.Goal }
    };

    [Header("シード値")]
    [SerializeField] int seed;


    ObjType[,] mapData;
    Dictionary<ObjType, GameObject> eventObj = new Dictionary<ObjType, GameObject>();

    public static List<V2> RandomPutEventTable = new List<V2>();

    // Start is called before the first frame update
    void Start()
    {
        if (seed != 0) Random.seed = seed;

        mapData = TerrainDataInstant.InstantMapChip(w, d, h, chaos);
        TerrainDataInstant.InstantProhibitedArea(mapData);

        MapEvent.InstantEvent(mapData, ObjType.Start);
        MapEvent.InstantEvent(mapData, ObjType.Goal);

        int xStart = MapEvent.eventPos[ObjType.Start].x;
        int zStart = MapEvent.eventPos[ObjType.Start].z;
        int xGoal = MapEvent.eventPos[ObjType.Goal].x;
        int zGoal = MapEvent.eventPos[ObjType.Goal].z;

        MapRoadInstant.IfWall_MakeRoad(mapData, xStart, zStart, roadLength, roadSpace);
        MapRoadInstant.IfWall_MakeRoad(mapData, xGoal, xGoal, roadLength, roadSpace);

        ListToDictionary();
        MapMaterialization.InstantFrame(w, d, frameObj, transform);
        MapMaterialization.ObjSet(mapData, wallObj, eventObj, transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //インスペクタ上に出したものをディクショナリに格納しなおす
    void ListToDictionary()
    {
        foreach(EventObj obj in inspectorExclusive)
        {
            eventObj.Add(obj.objType, obj.obj);
        }
    }
}

public class V2
{
    public int x;
    public int z;

    public V2(int x = 0, int z = 0)
    {
        this.x = x;
        this.z = z;
    }
}
