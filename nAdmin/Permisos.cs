﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using InfinityScript;
using System.IO;
using System.Security.Cryptography;

namespace nAdmin
{
    class Permisos : BaseScript
    {
        private Dictionary<string, string> comandos;
        private Dictionary<string, string> grupos;
        private static cIniArray oIni = new cIniArray();
        private static string dirPerm = "scripts\\nAdmin\\permisos";
        private static string ciniName = "scripts\\nAdmin\\config.ini";

        static Permisos()
        {
            
        }
     
        public static void LoadPermisos()
        {
            if (!File.Exists(ciniName))
            {
                try
                {
                    oIni.IniWrite(Permisos.ciniName, "Permisos", "Grupos", "Admin,Mod,User");
                    oIni.IniWrite(Permisos.ciniName, "Permisos", "AdminCmd", "*");
                    oIni.IniWrite(Permisos.ciniName, "Permisos", "ModCmd", "!kick,!ban");
                    oIni.IniWrite(Permisos.ciniName, "Permisos", "UserCmd", "!help");

                    MD5 md5 = System.Security.Cryptography.MD5.Create();
                    byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes((Environment.TickCount & Int32.MaxValue).ToString());
                    byte[] hash = md5.ComputeHash(inputBytes);

                    // step 2, convert byte array to hex string
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < hash.Length; i++)
                    {
                        sb.Append(hash[i].ToString("X2"));
                    }
                    oIni.IniWrite(Permisos.ciniName, "nEmu", "ServerKey", sb.ToString());
                }
                catch (Exception e)
                {
                    Log.Error(e.Message);
                }
            }
        }
        public static  bool CanUseCommand(Entity player, string comando)
        {
            string coso = oIni.IniGet(Permisos.ciniName, "Permisos", GetGroup(player)+"Cmd", "");
            Log.Info(GetGroup(player) + "Cmd");
            Log.Info(comando);
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
            Log.Info(player.Name + " " + ToHex(player.GUID));
            string str1 = oIni.IniGet(Permisos.ciniName, "Permisos", "Grupos", "Admin,Mod,User");
  
            foreach (string str2 in str1.Split(','))
            {
                Log.Info(Permisos.dirPerm + "\\" + clienteMyId + "." + str2);
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
                string path = "scripts\\nAdmin\\Permisos\\" + clienteMyId + "." + grupo;
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
