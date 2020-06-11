using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainDataInstant : MonoBehaviour
{
    static float seedX, seedZ;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// マップデータの作成
    /// </summary>
    /// <param name="w">幅</param>
    /// <param name="d">奥行き</param>
    /// <param name="h">ノイズの高さから壁作成</param>
    /// <param name="chaos">ノイズの細かさ</param>
    /// <returns></returns>
    public static Map.ObjType[,] InstantMapChip(int w, int d, float h, float chaos)
    {
        Map.ObjType[,] mapData = new Map.ObjType[w, d];

        //シード値の生成
        seedX = Random.value * 100f;
        seedZ = Random.value * 100f;

        //パーリンノイズからマップ作成
        for (int x = 0; x < w; x++)
        {
            for (int z = 0; z < d; z++)
            {
                //壁でない場所を格納する
                if (x < w - 1 && w != 0 && z < d - 1 && z != 0)
                {
                    Map.RandomPutEventTable.Add(new V2());
                    Map.RandomPutEventTable[Map.RandomPutEventTable.Count - 1].x = x;
                    Map.RandomPutEventTable[Map.RandomPutEventTable.Count - 1].z = z;
                }

                //ノイズ発生
                float xSample = (x + seedX) / chaos;
                float zSample = (z + seedZ) / chaos;
                float noise = Mathf.PerlinNoise(xSample, zSample);
                float y = h * noise;

                //四捨五入
                y = Mathf.Round(y);

                if (y <= h * 0.1f)
                {
                    //壁であることを記録
                    mapData[x, z] = Map.ObjType.Wall;

                    //イベントを配置できる場所の記録用
                    if (x < w - 1 && w != 0 && z < d - 1 && z != 0)
                    {
                        Map.RandomPutEventTable.RemoveAt(Map.RandomPutEventTable.Count - 1);
                    }
                }
            }
        }

        return mapData;
    }

    /// <summary>
    /// イベント設置禁止エリア作成
    /// </summary>
    /// <param name="w"></param>
    /// <param name="d"></param>
    /// <param name="mapData"></param>
    public static void InstantProhibitedArea(Map.ObjType[,] mapData)
    {
        int w = mapData.GetLength((int)Map.MapDataArrLength.Width);
        int d = mapData.GetLength((int)Map.MapDataArrLength.Depth);

        for (int x = 1; x < w - 1; x++)
        {
            for (int z = 1; z < d - 1; z++)
            {
                if (mapData[x, z] == Map.ObjType.Wall)
                {
                    if (mapData[x - 1, z] == Map.ObjType.Nothing ||
                        mapData[x + 1, z] == Map.ObjType.Nothing ||
                        mapData[x, z - 1] == Map.ObjType.Nothing ||
                        mapData[x, z + 1] == Map.ObjType.Nothing)
                    {
                        mapData[x, z] = Map.ObjType.ProhibitedArea;
                    }
                }

            }
        }
    }
}
