using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMaterialization : MonoBehaviour
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
    /// マップの具現化
    /// </summary>
    /// <param name="mapData">マップデータ</param>
    /// <param name="wallObj">壁オブジェクト</param>
    /// <param name="eventObj">イベントオブジェクト</param>
    /// <param name="parent">親に</param>
    public static void ObjSet(Map.ObjType[,] mapData, GameObject wallObj,Dictionary<Map.ObjType,GameObject> eventObj, Transform parent)
    {
        int w = mapData.GetLength((int)Map.MapDataArrLength.Width);
        int d = mapData.GetLength((int)Map.MapDataArrLength.Depth);

        //オブジェクト設置
        for (int x = 1; x < w - 1; x++)
        {
            for (int z = 1; z < d - 1; z++)
            {
                if (mapData[x, z] == Map.ObjType.Wall)
                {
                    //周りをキューブで囲むように設置
                    if (isEventCheck(x, z))
                    {
                        InstantRoom(wallObj, x, z);
                    }
                }
                else
                {
                    //マップデータとイベントが一致している場合作成
                    if (eventObj.ContainsKey(mapData[x, z]))
                    {
                        InstantRoom(eventObj[mapData[x, z]], x, z);
                    }
                }

                int num = (int)mapData[x, z];
            }
        }

        bool isEventCheck(int x, int z)
        {
            bool b = false;
            for (int i = 0; i <= (int)Map.ObjType.ProhibitedArea; i++)
            {
                if (mapData[x - 1, z] == (Map.ObjType)i ||
                    mapData[x + 1, z] == (Map.ObjType)i ||
                    mapData[x, z - 1] == (Map.ObjType)i ||
                    mapData[x, z + 1] == (Map.ObjType)i)
                {
                    b = true;
                }
                if (i == 0) i = (int)Map.ObjType.Wall;
            }
            return b;
        }

        //生成
        void InstantRoom(GameObject instantObj, float x, float z)
        {
            GameObject cube = Instantiate(instantObj, new Vector3(x, 0, z), new Quaternion());
            cube.transform.SetParent(parent);
        }
    }

    /// <summary>
    /// 地面と囲いの作成
    /// </summary>
    /// <param name="w">マップの幅</param>
    /// <param name="d">マップの奥行</param>
    /// <param name="frameObj">設置するオブジェクト</param>
    /// <param name="parent">親に</param>
    public static void InstantFrame(int w,int d,GameObject frameObj, Transform parent)
    {
        //地面の生成
        ObjInstant(new Vector3(w / 2, -1, d / 2), new Vector3(w, 1, d));

        //外枠作成
        ObjInstant(new Vector3(0.5f, 0, d / 2), new Vector3(1, 1, d));
        ObjInstant(new Vector3(w - 0.5f, 0, d / 2), new Vector3(1, 1, d));
        ObjInstant(new Vector3(w / 2, 0, 0.5f), new Vector3(w, 1, 1));
        ObjInstant(new Vector3(w / 2, 0, d - 0.5f), new Vector3(w, 1, 1));

        //側の作成
        void ObjInstant(Vector3 pos, Vector3 siz)
        {
            GameObject ground = Instantiate(frameObj, parent);
            float fix = frameObj.transform.localScale.y / 2;
            ground.transform.localPosition = new Vector3(pos.x - fix, pos.y, pos.z - fix);
            ground.transform.localScale = siz;
        }
    }

    
}
