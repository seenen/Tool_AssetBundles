using UnityEngine;
using System.Collections;
using AssetBundleEditor;

public class IOSTest : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
		//StartCoroutine(Player());
        //StartCoroutine(Monster001());
        //StartCoroutine(Scene());
        TestNavMesh();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	IEnumerator Monster001()
	{
		yield return 1;
		
#if UNITY_IPHONE
		string path = "file://" + Application.dataPath + "/Raw/";
#elif UNITY_STANDALONE_WIN
		string path = "file://" + Application.dataPath + "/StreamingAssets/";
#else
		string path = "jar:file://" + Application.dataPath + "!/assets/";
#endif
				
		Debug.Log(path + "monster001.assetBundles");
		
		ABLoad load = new ABLoad(path + "monster001.assetBundles");
		while(true)
		{
			if (load.bundle.isDone)
				break;
			
			yield return 1;
		}
		
		Debug.Log(load);
        //GameObject res = load.LoadGameObject("monster001");
        GameObject res = load.Load<GameObject>("monster001");
		
		Debug.Log(res);
		
		Instantiate(res);
		
		load.Close();

	}

    IEnumerator Scene()
    {
        yield return 1;

#if UNITY_IPHONE
		string path = "file://" + Application.dataPath + "/Raw/";
#elif UNITY_STANDALONE_WIN
        string path = "file://" + Application.dataPath + "/StreamingAssets/";
#else
		string path = "jar:file://" + Application.dataPath + "!/assets/";
#endif

        Debug.Log(path + "110101.assetBundles");

        ABLoad load = new ABLoad(path + "110101.assetBundles");
        while (true)
        {
            if (load.bundle.isDone)
                break;

            yield return 1;
        }
        
        //  加载场景
        GameObject res = load.Load<GameObject>("110101");
        Instantiate(res);

        //  加载LightMap
        ArrayList texlist = new ArrayList();// Texture2D

        Object[] objects = load.LoadAll();
        foreach (Object e in objects)
        {
            if (!e.name.StartsWith("LightmapFar-"))
                continue;

            if (e.GetType() == typeof(Texture2D))
                texlist.Add(e);
        }
        texlist.Sort();

        //  数据集数量
        int lmDataSetCount = (texlist.Count + 1) / 2;
        int lmDataIndex = 0;

        LightmapData[] lmDataSet = new LightmapData[lmDataSetCount];

        for (int i = 0; i < texlist.Count; ++i)
        {
            LightmapData lmData = new LightmapData();
            lmData.lightmapFar = (Texture2D)texlist[i];
            if ((++i) != texlist.Count) lmData.lightmapNear = (Texture2D)texlist[i];

            lmDataSet[lmDataIndex] = lmData;
        }

        LightmapSettings.lightmapsMode = LightmapsMode.Dual;
        LightmapSettings.lightmaps = lmDataSet;

        load.Close();

    }

    void TestNavMesh()
    {
        Vector3[] vertis;
        int[] ids;

        NavMesh.Triangulate(out vertis, out ids);

        Debug.Log(vertis.Length + "  " + ids.Length);

        //
        GameObject o = new GameObject();
        o.AddComponent<MeshRenderer>();
        MeshFilter mf = o.AddComponent<MeshFilter>();

        Mesh mesh = new Mesh();
        mesh.vertices = vertis;
        mesh.triangles = ids;

        mf.mesh = mesh;

        o.transform.position = Vector3.up * 20;
    }
}
