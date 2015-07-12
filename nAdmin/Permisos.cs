using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using InfinityScript;

namespace nAdmin
{
    class Permisos : BaseScript
    {
        static Permisos()
        {
        }
        
        public static  bool CanUseCommand(Entity player, string comando)
        {
            return true;
        }

        public static  string GetGroup(string xuid)
        {
            return "a";
        }

        public static  bool SetGroup(Entity player, string grupo)
        {
            return true;
        } 
    }
}
