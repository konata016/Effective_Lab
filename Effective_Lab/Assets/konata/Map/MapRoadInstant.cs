using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 道を作る
/// </summary>
public class MapRoadInstant : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 特定の位置から3方向壁に囲まれている場合、一番近い壁を壊す
    /// </summary>
    /// <param name="map">マップデータ</param>
    /// <param name="x">ターゲット</param>
    /// <param name="z">ターゲット</param>
    /// <param name="dis">見る距離</param>
    /// <param name="roadSpace">壁の幅</param>
    public static void IfWall_MakeRoad(Map.ObjType[,] map, int x, int z, int dis, int roadSpace)
    {
        int wallCount = 0;

        Dictionary<MapEvent.Dir, int> wallCheckDis =
            new Dictionary<MapEvent.Dir, int>(MapEventSearch.WallSearch(map, x, z, dis));

        //辞書内にある最も小さい値を取り出す
        int minValue = wallCheckDis.Values.Min();
        var pair = wallCheckDis.FirstOrDefault(c => c.Value == minValue);
        var key = pair.Key;

        //壁に阻まれた方向の数を数える
        foreach (var str in wallCheckDis)
        {
            //Debug.Log("key " + str.Key+" value "+str.Value);
            if (str.Value < dis) wallCount++;
            if (str.Value == MapEventSearch.Outside) wallCount++;
        }

        //3方向囲まれた場合一番近い壁を壊す
        if (wallCount >= 3)
        {
            MapEvent.WallDestroy(map, x, z, pair.Key, pair.Value, roadSpace);
        }
    }
}
