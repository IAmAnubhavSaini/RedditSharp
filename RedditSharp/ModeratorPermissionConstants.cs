using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditSharp
{
    static public class ModeratorPermissionConstants
    {
        [Flags]
        public enum ModeratorPermission
        {
            None = 0x00,
            Access = 0x01,
            Config = 0x02,
            Flair = 0x04,
            Mail = 0x08,
            Posts = 0x10,
            Wiki = 0x20,
            All = Access | Config | Flair | Mail | Posts | Wiki
        }
    }
}
