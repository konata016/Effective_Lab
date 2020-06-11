using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// マップに出すイベント
/// </summary>
public class MapEvent : MonoBehaviour
{
    public enum Dir { Up, Down, Right, Left }

    /// <summary>
    /// イベントの名前から位置を取得できる
    /// </summary>
    public static Dictionary<Map.ObjType, V2> eventPos = new Dictionary<Map.ObjType, V2>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// イベントのポジションをマップデータ内に記録
    /// </summary>
    /// <param name="map">マップデータ</param>
    /// <param name="objType">イベントの種類</param>
    public static void InstantEvent(Map.ObjType[,] map, Map.ObjType objType)
    {
        int ran = Random.Range(0, Map.RandomPutEventTable.Count - 1);

        //マップデータにマークする
        map[Map.RandomPutEventTable[ran].x, Map.RandomPutEventTable[ran].z] = objType;
        
        //座標を格納
        eventPos.Add(objType, new V2(Map.RandomPutEventTable[ran].x, Map.RandomPutEventTable[ran].z));
        
        //イベントを配置したのでその場所を消す
        Map.RandomPutEventTable.RemoveAt(ran);

    }


    /// <summary>
    /// 1方向の壁を壊す
    /// </summary>
    /// <param name="map">マップデータ</param>
    /// <param name="x">壊す座標</param>
    /// <param name="z">壊す座標</param>
    /// <param name="dir">壊す方向</param>
    /// <param name="length">壊す距離</param>
    /// <param name="roadSpace">壊す幅</param>
    public static void WallDestroy(Map.ObjType[,] map, int x, int z, Dir dir, int length, int roadSpace)
    {
        int w = map.GetLength((int)Map.MapDataArrLength.Width);
        int d = map.GetLength((int)Map.MapDataArrLength.Depth);

        V2 roadPos = new V2(x, z);
        int loopCount = 0;
        bool isRoadInstant = false;

        for (; ; )
        {
            //どの方向か見る
            switch (dir)
            {
                case Dir.Up: roadPos.z += 1; break;
                case Dir.Down: roadPos.z -= 1; break;
                case Dir.Right: roadPos.x += 1; break;
                case Dir.Left: roadPos.x -= 1; break;
                default: break;
            }
            //Debug.Log(roadPos.x + "  " + roadPos.z);

            //配列外の場合ループから抜け出す
            if (roadPos.z == d - 1 || roadPos.z == 1) break;
            if (roadPos.x == w - 1 || roadPos.x == 1) break;

            //壁または禁止エリアの場合、壁を壊し道を作る
            if (map[roadPos.x, roadPos.z] == Map.ObjType.Wall ||
                map[roadPos.x, roadPos.z] == Map.ObjType.ProhibitedArea)
            {
                int num = roadSpace / 2;
                for (int i = -num; i <= num; i++)
                {
                    if (dir == Dir.Up || dir == Dir.Down)
                    {
                        map[x + i, roadPos.z] = Map.ObjType.Nothing;
                    }
                    if (dir == Dir.Left || dir == Dir.Right)
                    {
                        map[roadPos.x, z + i] = Map.ObjType.Nothing;
                    }
                }
                isRoadInstant = true;
            }
            else if (isRoadInstant) //壁から出るまで回す
            {
                break;
            }
            //Debug.Log(loopCount);
            loopCount++;
        }

    }
}
