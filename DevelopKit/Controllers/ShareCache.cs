using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace DevelopKit
{
    public class ShareCache
    {
        public int OpenedSceneID;
        private string Position;

        public ShareCache<Image> ShareImage;
        public ShareCache<TextBox> ShareTextBox;
        public ShareCache<TrackBar> ShareTrackBar;

        public ThirdPartDraw ThirdPartCaller;

        public ShareCache()
        {
            ShareImage = new ShareCache<Image>();
            ShareTextBox = new ShareCache<TextBox>();
            ShareTrackBar = new ShareCache<TrackBar>();

            ThirdPartCaller = new ThirdPartDraw();
        }

        public void SetPosition(string P)
        {
            Position = P;
        }

        public string GetPosition()
        {
            return Position;
        }
    }

    public class ThirdPartDraw
    {
        private Dictionary<string, SortedDictionary<int, Object>> methodToParamMapping;
        private Dictionary<string, Object> methodToResultMapping;

        public ThirdPartDraw()
        {
            methodToParamMapping = new Dictionary<string, SortedDictionary<int, Object>>();
            methodToResultMapping = new Dictionary<string, object>();
        }

        public void Draw(string method, int paramIndex, Object param)
        {
            Register(method, paramIndex, param);
            ThirdPartApiClient cli = new ThirdPartApiClient(method);

            foreach (KeyValuePair<int, Object> kv in methodToParamMapping[method])
            {
                cli.AddParam(kv.Value);
            }
            Image image = cli.Call();

            if (!methodToResultMapping.ContainsKey(method))
                methodToResultMapping.Add(method, image);
            else
                methodToResultMapping[method] = image;
        }

        public Image Get(string method)
        {
            return methodToResultMapping.ContainsKey(method)? (Image)methodToResultMapping[method]: null;
        }

        public void Register(string method, int paramIndex, Object param)
        {
            if (!methodToParamMapping.ContainsKey(method))
                methodToParamMapping[method] = new SortedDictionary<int, Object> { { paramIndex, param } };
            else
                methodToParamMapping[method][paramIndex] = param;
        }
    }

    public class ShareCache<T>
    {
        private Dictionary<int, T> cacheMap;

        public ShareCache()
        {
            cacheMap = new Dictionary<int, T>();
        }

        public void Set(int id, T t)
        {
            cacheMap[id] = t;
        }

        public void Set(int id, int refId)
        {
            cacheMap[id] = cacheMap[refId];
        }

        public T Get(int id)
        {
            Console.WriteLine("share cache get id=" + id);
            return cacheMap[id];
        }
    }


    //public class ShareCache
    //{
    //    private IdNotify IdNotify;
    //    private Dictionary<int, PictureBox> cache;

    //    public ShareCache()
    //    {
    //        IdNotify = new IdNotify(delegate (int triggerId, int Id)
    //        {
    //            cache[Id].Image = cache[triggerId].Image;
    //        });
    //        cache = new Dictionary<int, PictureBox>();
    //    }

    //    public void Reigster(int id, PictureBox obj)
    //    {

    //        if (!cache.ContainsKey(id))
    //        {
    //            IdNotify.Register(id);
    //            cache[id] = obj;
    //        }
    //        else if (IdNotify.NeedNotify(id))
    //        {
    //            IdNotify.NotifyById(id);
    //        }
    //    }
    //}


    //public class IdNotify
    //{
    //    public delegate void NotifyHandler(int triggerId, int itemId);

    //    private NotifyHandler IdNotifyHandler;
    //    private Dictionary<int, HashSet<int>> idToIDSet;

    //    public IdNotify(NotifyHandler MyIdNotifyHandler)
    //    {
    //        idToIDSet = new Dictionary<int, HashSet<int>>();
    //        IdNotifyHandler = MyIdNotifyHandler;  
    //    }

    //    public void Register(int id)
    //    {
    //        if (!idToIDSet.ContainsKey(id))
    //        {
    //            idToIDSet[id] = new HashSet<int>() { id };
    //        }
    //    }

    //    public void Register(int id, int brotherId)
    //    {
    //        if (idToIDSet.ContainsKey(brotherId))
    //        {
    //            HashSet<int> ids = idToIDSet[brotherId];
    //            ids.Add(id);
    //            idToIDSet[id] = ids;
    //        }
    //    }

    //    public void NotifyById(int id)
    //    {
    //        if (NeedNotify(id))
    //        {
    //            foreach (int brotherId in idToIDSet[id])
    //            {
    //                if (id == brotherId)
    //                    continue; 
    //                IdNotifyHandler(id, brotherId);
    //            }
    //        }
    //    }

    //    public bool NeedNotify(int id)
    //    {
    //        return idToIDSet[id].Count > 1;
    //    }
    //}
}
