using UnityEngine;

namespace AssetBundleEditor
{
    public class ABLoad
    {
        public WWW bundle;

        public ABLoad(string root)
        {
            bundle = new WWW(root);//WWW.LoadFromCacheOrDownload (url, 1); 
        }

        public T Load<T>(string res) where T : Object
        {
            Object obj = bundle.assetBundle.Load(res, typeof(T));
            if (obj.GetType() == typeof(T))
            {
                T t = obj as T;
                return t;
            }
            else
            {
                Debug.LogError("[Bundle:] " + bundle.url + " [Res:] " + res + " [Type Is:] " + obj.GetType());
                return default(T);
            }
        }

        public Object[] LoadAll()
        {
            return bundle.assetBundle.LoadAll();
        }

        public void Close()
        {
            bundle.assetBundle.Unload(false);
        }
    }
}