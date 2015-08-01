using System.IO;

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
        private int MAPS = 16;
        private string[] mapdev = new string[16]
        {
          "mp_alpha",
          "mp_bootleg",
          "mp_bravo",
          "mp_carbon",
          "mp_dome",
          "mp_exchange",
          "mp_hardhat",
          "mp_interchange",
          "mp_lambeth",
          "mp_mogadishu",
          "mp_paris",
          "mp_plaza2",
          "mp_radar",
          "mp_seatown",
          "mp_underground",
          "mp_village"  
        };

        private string[] mapuser = new string[16]
        {
          "lockdown",
          "bootleg",
          "mission",
          "carbon",
          "dome",
          "downturn",
          "hardhat",
          "interchange",
          "fallen",
          "bakaara",
          "resistance",
          "arkaden",
          "outpost",
          "seatown",
          "underground",
          "village"  
        };

        private string[] gtdev = new string[17]
        {
          "DEM",
          "SNIP",
          "CTF",
          "DOM",
          "DZ",
          "FFA",
          "GG",
          "HQ",
          "INF",
          "JUG",
          "KC",
          "OIC",
          "SAB",
          "SD",
          "TDM",
          "TDEF",
          "TJ"
        };
        private string[] gtuser = new string[17]
        {
          "Demolition",
          "iSnipe",
          "Capture_the_Flag",
          "Domination",
          "Drop_Zone",
          "Free-For-All",
          "Gun_Game",
          "Headquarters",
          "Infected",
          "Juggernaut",
          "Kill_Confirmed",
          "One_in_the_chamber",
          "Sabotage",
          "Search & Destroy",
          "Team_Deathmatch",
          "Team_Defender",
          "Team_Juggernaut"
        };
        private int gtByName = -1;
        private const int GTS = 17;

        /*
        private string[] mapdev = new string[36]
        {
        "mp_burn_ss",
        "mp_crosswalk_ss",
        "mp_six_ss",
        "mp_boardwalk",
        "mp_moab",
        "mp_nola",
        "mp_roughneck",
        "mp_shipbreaker",
        "mp_alpha",
        "mp_bootleg",
        "mp_bravo",
        "mp_carbon",
        "mp_dome",
        "mp_exchange",
        "mp_hardhat",
        "mp_interchange",
        "mp_lambeth",
        "mp_mogadishu",
        "mp_paris",
        "mp_plaza2",
        "mp_radar",
        "mp_seatown",
        "mp_underground",
        "mp_village",
        "mp_cement",
        "mp_italy",
        "mp_meteora",
        "mp_morningwood",
        "mp_overwatch",
        "mp_park",
        "mp_qadeem",
        "mp_aground_ss",
        "mp_courtyard_ss",
        "mp_hillside_ss",
        "mp_restrepo_ss",
        "mp_terminal_cls"
        

        private string[] mapuser = new string[36]
        {
        "u-turn",
        "intersection",
        "vortex",
        "boardwalk",
        "gulch",
        "parish",
        "off shore",
        "decommision",
        "lockdown",
        "bootleg",
        "mission",
        "carbon",
        "dome",
        "downturn",
        "hardhat",
        "interchange",
        "fallen",
        "bakaara",
        "resistance",
        "arkaden",
        "outpost",
        "seatown",
        "underground",
        "village",
        "Foundation",
        "Piazza",
        "Sanctuary",
        "Black Box",
        "Overwatch",
        "Liberation",
        "Oasis",
        "Aground",
        "Erosion",
        "Getaway",
        "Lookout",
        "Terminal"
        };*/

        private string _sPort;
        private string _warid;
        public string admins;
        public string BotName;
        private Dictionary<string, string> gCommands = new Dictionary<string, string>();
        private Dictionary<string, string> Groups = new Dictionary<string, string>();
        private Dictionary<string, string> UserGroup = new Dictionary<string, string>();
        private List<Entity> Entitys;
        public nAdmin()
        {
            Permisos.LoadPermisos();
            _sPort = Call<string>("getDvar", "net_port");
            PlayerConnected += (player =>
            {
                
            });
        }


        public bool CambioMapa(string mapName, Entity player)
        {
            int mapByName = GetMapByName(mapName);
            if (mapByName != -1)
             Utilities.ExecuteCommand("map " + mapdev[mapByName]);
            else 
                TellClient(player, "Nombre de Mapa Invalido");
            return false;
        }

        public bool CambioGame(string gtName, Entity player)
        {
            try
            {
                gtByName = GetGameByName(gtName);
                if (gtByName != -1)
                {
                    WriteDSPL("*", gtdev[gtByName]);
                    Utilities.ExecuteCommand("map_rotate");
                    return true;
                }
                else if (File.Exists("admin\\" + gtName + "_default.dsr"))
                {
                    WriteDSPL("*", gtName);
                    Utilities.ExecuteCommand("map_rotate");
                    return true;
                }
                else
                {
                    TellClient(player, "Nombre de Juego Invalido");
                    return false;
                }
            }
            catch(Exception e)
            {
                Log.Info("CambioGame: " + e.Message);
            }
            return false;
        }

        private int GetMapByName(string map)
        {
            MAPS = mapdev.Length;
            string[] strArray = mapuser;
            for (int index = 0; index < MAPS; ++index)
            {
                if (strArray[index].StartsWith(map, StringComparison.InvariantCultureIgnoreCase))
                    return index;
            }
            return -1;
        }

        private int GetGameByName(string map)
        {
            string[] strArray = gtuser;
            for (int index = 0; index < GTS; ++index)
            {
                if (strArray[index].ToLower().Contains(map.ToLower()))
                    return index;
            }
            return -1;
        }

        private void WriteDSPL(string map, string gt)
        {
            try
            {
                File.WriteAllText("admin\\default.dspl", map + "," + gt + "_default,1");
                File.WriteAllText("players2\\default.dspl", map + "," + gt + "_default,1");
            }
            catch (Exception ex)
            {
                Log.Info("Error al escribir DSPL!:" + ex);
            }
        }

        public static string ToHex(long value)
        {
            return String.Format("{0:X}", value);
        }

        public void OnPlayerSpawned(Entity player)
        {
            //shit here
        }


        private void TellClient(Entity player, string message)
        {
            BotName = "^0[^2nEmu^0] ^7: ";
            Utilities.RawSayTo(player, BotName + "^3[PM] ^7: " + message);
        }

        public void ServerSay(string message)
        {
            Utilities.RawSayAll(BotName + message);
        }

        private Entity FindByName(string name)
        {
            int num = 0;
            Entity entity1 = null;
            foreach (Entity entity2 in Entitys)
            {
                if (0 <= entity2.Name.IndexOf(name, StringComparison.InvariantCultureIgnoreCase))
                {
                    entity1 = entity2;
                    ++num;
                }
            }
            if (num <= 1 && num == 1)
                return entity1;
            return null;
        }

        private void kick(string message, Entity player)
        {
            string[] strArray = message.Split(' ');
            string newValue = "";
            if (strArray.Length <= 1)
            {
                TellClient(player, "^1Enter a playername");
            }
            else
            {
                Entity byName = FindByName(strArray[1]);
                if (byName == null)
                    TellClient(player, "^1That user wasn't found or multiple were found.");
                else if (strArray.Length > 2)
                {
                    for (int index = 2; index < strArray.Length; ++index)
                        newValue = newValue + " " + strArray[index];
                    Utilities.ExecuteCommand("dropclient " + byName.Call<int>("getentitynumber"));
                    string str1 = "^2<playername> ^3has been kicked ^7for ^1<reason> ^7by ^1<kicker>";
                    foreach (string str2 in File.ReadAllLines("scripts\\\\nEmu\\\\nEmu.cfg"))
                    {
                        if (str2.StartsWith("kickmessage"))
                            str1 = str2.Split('=')[1];
                    }
                    ServerSay(str1.Replace("<playername>", byName.Name).Replace("<reason>", newValue).Replace("<kicker>", player.Name));
                }
                else
                {
                    if (strArray.Length > 2)
                    {
                        Utilities.ExecuteCommand("dropclient " + byName.Call<int>("getentitynumber"));
                        ServerSay("^2<playername> ^3has been kicked ^7by ^1<kicker>".Replace("<playername>", byName.Name).Replace("<kicker>", player.Name));
                    }
                }
            }
        }

        private void ban(string message, Entity player)
        {
            string[] strArray = message.Split(' ');
            string newValue = "";
            if (strArray.Length <= 1)
            {
                TellClient(player, "^1Enter a playername");
            }
            else
            {
                Entity byName = FindByName(strArray[1]);
                if (byName == null)
                    TellClient(player, "^1That user wasn't found or multiple were found.");
                else if (strArray.Length > 2)
                {
                    for (int index = 2; index < strArray.Length; ++index)
                        newValue = newValue + " " + strArray[index];
                    Utilities.ExecuteCommand("banclient " + byName.Call<int>("getentitynumber"));
                    string str1 = "^2<playername> ^3has been banned ^7for ^1<reason> ^7by ^1<kicker>";
                    foreach (string str2 in File.ReadAllLines("scripts\\\\nEmu\\\\nEmu.cfg"))
                    {
                        if (str2.StartsWith("banmessage"))
                            str1 = str2.Split('=')[1];
                    }
                    ServerSay(str1.Replace("<playername>", byName.Name).Replace("<reason>", newValue).Replace("<kicker>", player.Name));
                }
                else
                {
                    if (strArray.Length > 2)
                    {
                        Utilities.ExecuteCommand("banclient " + byName.Call<int>("getentitynumber"));
                        ServerSay("^2<playername> ^3has been banned ^7by ^1<kicker>".Replace("<playername>", byName.Name).Replace("<kicker>", player.Name));   
                    }
                }
            }
        }

        private void tmpban(string message, Entity player)
        {
            string[] strArray = message.Split(' ');
            string newValue = "";
            if (strArray.Length <= 1)
            {
                TellClient(player, "^1Enter a playername");
            }
            else
            {
                Entity byName = FindByName(strArray[1]);
                if (byName == null)
                    TellClient(player, "^1That user wasn't found or multiple were found.");
                else if (strArray.Length > 2)
                {
                    for (int index = 2; index < strArray.Length; ++index)
                        newValue = newValue + " " + strArray[index];
                    Utilities.ExecuteCommand("tempbanclient " + byName.Call<int>("getentitynumber") + " \"" + newValue + "\"");
                    string str1 = "^2<playername> ^3has been temp banned ^7for ^1<reason> ^7by ^1<kicker>";
                    foreach (string str2 in File.ReadAllLines("scripts\\\\nEmu\\\\nEmu.cfg"))
                    {
                        if (str2.StartsWith("tempbanmessage"))
                            str1 = str2.Split('=')[1];
                    }
                        ServerSay(str1.Replace("<playername>", byName.Name).Replace("<reason>", newValue).Replace("<kicker>", player.Name));
                }
                else
                {
                    if (strArray.Length > 2)
                    {
                        Utilities.ExecuteCommand("tempbanclient " + byName.Call<int>("getentitynumber") + " \"nEmu : Player TempBanned!\"");
                        ServerSay("^2<playername> ^3has been temp banned ^7by ^1<kicker>".Replace("<playername>", byName.Name).Replace("<kicker>", player.Name));
                    }
                }
            }
        }

        private void getslot(string message, Entity player)
        {
            string[] strArray = message.Split(' ');
            string str = "";
            if (strArray.Length <= 1)
            {
                TellClient(player, "^1Enter a playername");
            }
            else
            {
                Entity byName = FindByName(strArray[1]);
                if (byName == null)
                    TellClient(player, "^1That user wasn't found or multiple were found.");
                else if (strArray.Length > 2)
                {
                    for (int index = 2; index < strArray.Length; ++index)
                        str = str + " " + strArray[index];
                    Utilities.ExecuteCommand("dropclient " + byName.Call<int>("getentitynumber") + " \"nEmu : Sorry Slot\"");
                    ServerSay("^2<playername> ^3has been kicked ^7for ^1Slot ^7by ^1<kicker>".Replace("<playername>", byName.Name).Replace("<kicker>", player.Name));
                }
                else
                {
                    if (strArray.Length > 2)
                    {
                        Utilities.ExecuteCommand("dropclient " + byName.Call<int>("getentitynumber") + " \"nEmu : Sorry Slot\"");
                        ServerSay("^2<playername> ^3has been kicked ^7for ^1Slot ^7by ^1<kicker>".Replace("<playername>",byName.Name).Replace("<kicker>", player.Name));
                    }
                }
            }
        }
        private void slot(Entity player)
        {
            int num1 = 0;
            int num2 = -1;
            foreach (Entity entity in Entitys)
            {
                if (entity.Ping > num1 && entity.Ping < 999)
                {
                    num2 = entity.Call<int>("getentitynumber");
                    num1 = entity.Ping;
                }
            }
            if (num2 > -1)
                Utilities.ExecuteCommand("dropclient " + num2 + "\"^1nEmu : your ping is too high...Fix it\"");
            else
                TellClient(player, "^1Could not find suitable client to kick.");
        }

        private void PM(Entity player, string message)
        {
            string[] strArray = message.Split(' ');
            string str = "";
            if (strArray.Length <= 1)
            {
                TellClient(player, "^1Enter a playername");
            }
            else
            {
                Entity byName = FindByName(strArray[1]);
                if (byName == null)
                    TellClient(player, "^1That user wasn't found or multiple were found.");
                else if (strArray.Length > 2)
                {
                    for (int index = 2; index < strArray.Length; ++index)
                        str = str + " " + strArray[index];
                    Utilities.RawSayTo(byName, BotName + "^3[PM]^7:^2 " + player.Name + " : ^1" + message.Replace(strArray[0], "").Replace(strArray[1], ""));
                }
                else
                {
                    if (strArray.Length > 2)
                    {
                        Utilities.RawSayTo(byName,BotName + "^3[PM]^7:^2 " + player.Name + " : ^1" + message.Replace(strArray[0], "").Replace(strArray[1], ""));
                    }
                }
            }
        }

        public override BaseScript.EventEat OnSay3(Entity player, BaseScript.ChatType type, string name, ref string message)
        {
            string[] strArray1 = message.Split(' ');
            if (message.StartsWith("!") && Permisos.CanUseCommand(player,strArray1[0]))
            {
                /*
                if (strArray1[0] == "!devlogs")
                {
                    using (var client = new WebClient())
                    {
                        var responseString = client.DownloadString("http://www.nemu.tk/nemu/warreceiver.php?port=" + _sPort + "&warid=" + _warid);
                        Log.Write(LogLevel.All, responseString);
                    }
                    Log.Write(LogLevel.All, _sPort);
                    Log.Write(LogLevel.All, _warid);
                    Log.Write(LogLevel.All, ToHex(player.GUID));
                }
                 */
                if (strArray1[0] == "!war")
                {
                    if (strArray1[1] == "off")
                    {
                        _warid = "6fbfd5e68d3306e51350bea0232f8fa5";
                        TellClient(player, "^3War: ^1Off");
                        Log.Info(BotName + "Puerto: " + _sPort);
                        Log.Info(BotName + "WarID: " + _warid);  
                    }
                    else
                    {
                        _warid = strArray1[1];
                        TellClient(player, "^3WarID: ^2" + _warid);
                        Log.Info(BotName + "Puerto: " + _sPort);
                        Log.Info(BotName + "WarID: " + _warid);
                    }
                    
                    using (var client = new WebClient())
                    {
                        var values = new NameValueCollection();
                        values["port"] = _sPort;
                        values["warid"] = _warid;

                        var response = client.UploadValues("http://www.nemu.tk/nemu/warreceiver.php", values);

                        var responseString = Encoding.Default.GetString(response);
                    }
                    return BaseScript.EventEat.EatGame;
                }
                if (strArray1[0].Equals("!kick"))
                {
                    
                    return BaseScript.EventEat.EatGame;
                }
                if (strArray1[0].Equals("!ban"))
                {
                    ban(message, player);
                    return BaseScript.EventEat.EatGame;
                }
                if (strArray1[0].Equals("!tmpban"))
                {
                    tmpban(message, player);
                    return BaseScript.EventEat.EatGame;
                }
                if (strArray1[0].Equals("!slot"))
                {
                    if (strArray1.Length > 1)
                    {
                        getslot(message, player);
                        return BaseScript.EventEat.EatGame;
                    }
                    slot(player);
                    return BaseScript.EventEat.EatGame;
                }
                if (strArray1[0] == "!mr")
                {
                    Utilities.ExecuteCommand("map_restart");
                    return BaseScript.EventEat.EatGame;
                }
                if (strArray1[0] == "!fr")
                {
                    Utilities.ExecuteCommand("fast_restart");
                    ServerSay(" ^2Fast Restarting the map...");
                    return BaseScript.EventEat.EatGame;
                }
                if (strArray1[0].Equals("!unban"))
                {
                    if (strArray1.Length <= 1)
                    {
                        TellClient(player, "^1Enter a playername");
                        return BaseScript.EventEat.EatGame;
                    }
                    Entity byName = FindByName(strArray1[1]);
                    if (byName == null)
                    {
                        TellClient(player, "^1That user wasn't found or multiple were found.");
                        return BaseScript.EventEat.EatGame;
                    }
                    Utilities.ExecuteCommand("unban " + byName.Name);
                    ServerSay(" ^1" + byName.Name + " ^3has been unbanned.");
                    return BaseScript.EventEat.EatGame;
                }
                if (strArray1[0].Equals("!guid"))
                {
                    TellClient(player, "^2Your GUID is: ^5" + player.GUID.ToString());
                    return BaseScript.EventEat.EatGame;
                }
                if (strArray1[0].Equals("!pm"))
                {
                    PM(player, message);
                    return BaseScript.EventEat.EatGame;
                }
                if (strArray1[0].Equals("!admins"))
                {
                    if (admins == "")
                    {
                        TellClient(player, "^1Online admins not in server");
                        return BaseScript.EventEat.EatGame;
                    }
                    TellClient(player, "^3Online ^2Admins ^7: " + admins);
                    return BaseScript.EventEat.EatGame;
                }
            }
            return BaseScript.EventEat.EatNone;
        }
    }
}