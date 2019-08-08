using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;


namespace DevelopKit
{
    public class ImageStore
    {
        //private IdNotify IdNotify;
        private Dictionary<string, Image> store;

        public ImageStore()
        {
            //IdNotify = new IdNotify(delegate (int srcId, int currentiId){
            //    store[srcId].Image.Clone()
            //});
            store = new Dictionary<string, Image>();
        }

        public void Set(string id, Image pb)
        {
            store[id] = pb;
            //IdNotify.Register(id);
        }

        public Image Get(string id)
        {
            return store[id];
        }
    }

    //public delegate void NotifyHandler(int srcId, int currentId);

    //public class IdNotify
    //{
    //    private NotifyHandler notifyHandler;
    //    private Dictionary<int, HashSet<int>> IdToBrotherIDs;

    //    public IdNotify(NotifyHandler handler)
    //    {
    //        notifyHandler = handler;
    //        IdToBrotherIDs = new Dictionary<int, HashSet<int>>();
    //    }

    //    public void Register(int id)
    //    {
    //        HashSet<int> idSet = new HashSet<int>();
    //        idSet.Add(id);
    //        IdToBrotherIDs[id] = idSet;
    //    }

    //    public void Register(int selfId, int brotherId)
    //    {
    //        if (IdToBrotherIDs.ContainsKey(brotherId))
    //        {
    //            HashSet<int> brothers = IdToBrotherIDs[brotherId];
    //            brothers.Add(selfId);
    //            IdToBrotherIDs[selfId] = brothers;
    //        }
    //    }

    //    public void BroadCast(int id)
    //    {
    //        if (IdToBrotherIDs[id].Count > 0)
    //        {
    //            foreach (int brotherId in IdToBrotherIDs[id])
    //            {
    //                if (id != brotherId)
    //                {
    //                    notifyHandler(id, brotherId);
    //                }
    //            }
    //        }
    //        else
    //            return;
    //    }
    //}
}
