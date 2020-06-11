using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// マップ上にあるイベントを探す
/// </summary>
public class MapEventSearch : MonoBehaviour
{
    public static int Outside { get { return 1000; } }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 4方向を見て壁の距離を返す
    /// </summary>
    /// <param name="map">マップデータ</param>
    /// <param name="x">見る座標</param>
    /// <param name="z">見る座標</param>
    /// <param name="dis">見る距離</param>
    /// <returns></returns>
    public static Dictionary<MapEvent.Dir, int> WallSearch(Map.ObjType[,] map,int x,int z, int dis)
    {
        int w = map.GetLength((int)Map.MapDataArrLength.Width);
        int d = map.GetLength((int)Map.MapDataArrLength.Depth);

        Dictionary<MapEvent.Dir, int> wallCheckDis = new Dictionary<MapEvent.Dir, int>()
        {
            { MapEvent.Dir.Up, 0 }, { MapEvent.Dir.Down, 0 },
            { MapEvent.Dir.Right, 0 }, { MapEvent.Dir.Left, 0 }
        };

        //壁までの距離を求める
        if (x - dis > 0)
        {
            wallCheckDis[MapEvent.Dir.Left] = CheckX(-1);
        }
        else wallCheckDis[MapEvent.Dir.Left] = Outside;

        if (x + dis < w)
        {
            wallCheckDis[MapEvent.Dir.Right] = CheckX(1);
        }
        else wallCheckDis[MapEvent.Dir.Right] = Outside;

        if (z - dis > 0)
        {
            wallCheckDis[MapEvent.Dir.Down] = CheckZ(-1);
        }
        else wallCheckDis[MapEvent.Dir.Down] = Outside;

        if (z + dis < d)
        {
            wallCheckDis[MapEvent.Dir.Up] = CheckZ(1);
        }
        else wallCheckDis[MapEvent.Dir.Up] = Outside;

        return wallCheckDis;

        //X方向の探査
        int CheckX(int dir)
        {
            int loopCount = 0;
            int pos = x;
            bool check = false;

            for (int i = 0; i < dis; i++)
            {
                pos += dir;
                if (map[pos, z] == Map.ObjType.Wall)
                {
                    check = true;
                    break;
                }
                loopCount++;
            }

            //Debug.Log("X" + loopCount);

            if (check) return loopCount;
            else return dis;
        }

        //Z方向の探査
        int CheckZ(int dir)
        {
            int loopCount = 0;
            int pos = z;
            bool check = false;

            for (int i = 0; i < dis; i++)
            {
                pos += dir;
                if (map[x, pos] == Map.ObjType.Wall)
                {
                    check = true;
                    break;
                }
                loopCount++;
            }

            //Debug.Log("Z" + loopCount);

            if (check) return loopCount;
            else return dis;
        }
    }
}
