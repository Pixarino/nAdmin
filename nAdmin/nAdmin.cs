namespace nAdmin
{
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using InfinityScript;

    public class nAdmin : BaseScript
    {
        private string _sPort;
        private string _warid;
        
        private Dictionary<string, string> gCommands = new Dictionary<string, string>();
        private Dictionary<string, string> Groups = new Dictionary<string, string>();
        private Dictionary<string, string> UserGroup = new Dictionary<string, string>();
        private List<Entity> Entitys;
        public nAdmin()
        {
            PlayerConnected += (player =>
            {
                _sPort = Call<string>("getDvar", "net_port");

                /* Conectandose al masterserver para sacar la lista de players en la war.
                using (var client = new WebClient())
                {
                    var responseString = client.DownloadString("");
                    Log.Write(LogLevel.All, responseString);
                    
                }                
                */
            });
        }

        public static string ToHex(long value)
        {
            return String.Format("{0:X}", value);
        }

        public void OnPlayerSpawned(Entity player)
        {
            //shit here
        }

        public override void OnSay(Entity player, string name, string messagetyped)
        {
            if (messagetyped.ToLower().StartsWith("!war"))
            {
                string[] msgAry = messagetyped.ToLower().Split(' ');

                if (msgAry[1] == "off")
                {
                    _warid = "";
                    Call<string>("iprintlnbold", "^3War: ^1Off");
                }
                else
                {
                    _warid = msgAry[1];
                    Call<string>("iprintlnbold", "^3WarID: ^2" + _warid);
                }

                /*
                using (List<Entity>.Enumerator enumerator = Entitys.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        Entity current = enumerator.Current;
                        if (!IsInWar(current))
                        Utilities.ExecuteCommand("dropclient " + ToHex(player.GUID) + " \"No estas en la lista de la war.\"");
                    }
                }
                */

                using (var client = new WebClient())
                {
                    var values = new NameValueCollection();
                    values["port"] = _sPort;
                    values["warid"] = _warid;

                    var response = client.UploadValues("http://www.nemu.tk/alfa.php", values);

                    var responseString = Encoding.Default.GetString(response);
                }
            }
            else if (messagetyped.ToLower().StartsWith("!devlogs"))
            {
                using (var client = new WebClient())
                {
                    var responseString = client.DownloadString("http://www.nemu.tk/alfa.php?port=" + _sPort + "&warid=" + _warid);
                    Log.Write(LogLevel.All, responseString);
                }
                Log.Write(LogLevel.All, _sPort);
                Log.Write(LogLevel.All, _warid);
                Log.Write(LogLevel.All, ToHex(player.GUID));
            }
        }

     

    

        public override BaseScript.EventEat OnSay3(Entity player, BaseScript.ChatType type, string name, ref string message)
        {
       
            string[] strArray1 = message.Split(' ');
            if (message.StartsWith("!") && Permisos.CanUseCommand(player,strArray1[0]))
            {              

                return BaseScript.EventEat.EatGame;
            }
            return BaseScript.EventEat.EatNone;
        }
    }
}