using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevelopKit
{
    public static class CollectionUtil
    {
        public static bool Contains(List<Group> groups, int gid)
        {
            if (groups == null || groups.Count == 0)
            {
                return false;
            }

            foreach (Group group in groups)
            {
                if (group.Id == gid)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
