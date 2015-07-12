using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using InfinityScript;
using System.IO;

namespace nAdmin
{
    class Permisos : BaseScript
    {
        private Dictionary<string, string> comandos;
        private Dictionary<string, string> grupos;
        private static cIniArray oIni = new cIniArray();
        private static string dirPerm = "plugins\\nAdmin\\permisos";
        private static string ciniName = "scripts\\nAdmin\\config.ini";

        static Permisos()
        {
            
        }

        public static void LoadPermisos()
        {
            if (!File.Exists(ciniName))
            {
                oIni.IniWrite(Permisos.ciniName, "Permisos", "Grupos", "Admin,Mod,User");
                oIni.IniWrite(Permisos.ciniName, "Permisos", "AdminCmd", "*");
                oIni.IniWrite(Permisos.ciniName, "Permisos", "ModCmd", "!kick,!ban");
                oIni.IniWrite(Permisos.ciniName, "Permisos", "UserCmd", "!help");
            }
        }
        public static  bool CanUseCommand(Entity player, string comando)
        {
            string coso = oIni.IniGet(Permisos.ciniName, "Permisos", GetGroup(player)+"Cmd", "");
            foreach (string cmd in coso.Split(','))
            { 
            if(cmd==comando){
                return true;
            }
            }
            return false;
        }

        public static  string GetGroup(Entity player)
        {
            string clienteMyId = ToHex(player.GUID);
           
            string str1 = oIni.IniGet(Permisos.ciniName, "Permisos", "Grupos", "Admin,Mod,User");
  
            foreach (string str2 in str1.Split(' '))
            {
                if (File.Exists(Permisos.dirPerm + "\\" + clienteMyId + "." + str2))
                    return str2;
            }
            return "User";
        }

        public static bool SetGroup(Entity player, string grupo)
        {
            try
            {
                string clienteMyId = ToHex(player.GUID);
                string path = "scripts\\nAdmin\\Permisos" + clienteMyId + "." + grupo;
                string grupoDeUsuario = GetGroup(player);
                File.Delete(Permisos.dirPerm + "\\" + clienteMyId + "." + grupoDeUsuario);
                File.Delete(Permisos.dirPerm + "\\" + clienteMyId + "." + grupo);
                FileStream fileStream = File.Create(path, 1024);
                byte[] bytes = new UTF8Encoding(true).GetBytes(player.Name);
                fileStream.Write(bytes, 0, bytes.Length);
                fileStream.Close();
            }
            catch(Exception e) {
                Log.Write((LogLevel)63,"Error al setear el permiso! %s",e.Message);
            }
            return true;
        }
        public static string ToHex(long value)
        {
            return String.Format("{0:X}", value);
        }
    }
}
