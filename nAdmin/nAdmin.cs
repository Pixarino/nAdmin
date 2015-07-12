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


        private void TellClient(Entity player, string message)
        {
            BotName = "^0[^2nEmu^0] ^7: ";
            Utilities.RawSayTo(player, BotName + "^3[PM] ^7: " + message);
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
                    Utilities.ExecuteCommand("dropclient " + byName.Call<int>("getentitynumber") + " \"" + newValue + "\"");
                    string str1 = "^2<playername> ^3has been kicked ^7for ^1<reason> ^7by ^1<kicker>";
                    ServerSay(str1.Replace("<playername>", byName.Name).Replace("<reason>", newValue).Replace("<kicker>", player.Name));
                }
                else
                {
                    if (strArray.Length > 2)
                        return;
                    Utilities.ExecuteCommand("dropclient " + byName.Call<int>("getentitynumber") + " \"^2Shoma kick shodid\"");
                    ServerSay("^2<playername> ^3has been kicked ^7by ^1<kicker>".Replace("<playername>", byName.Name).Replace("<kicker>", player.Name));
                }
            }
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
                        return;
                    Utilities.ExecuteCommand("banclient " + byName.Call<int>("getentitynumber"));
                    ServerSay("^2<playername> ^3has been banned ^7by ^1<kicker>".Replace("<playername>", byName.Name).Replace("<kicker>", player.Name));
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
                        return;
                    Utilities.ExecuteCommand("tempbanclient " + byName.Call<int>("getentitynumber") + " \"nEmu : Player TempBanned!\"");
                        ServerSay("^2<playername> ^3has been temp banned ^7by ^1<kicker>".Replace("<playername>", byName.Name).Replace("<kicker>", player.Name));
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
                        return;
                    Utilities.ExecuteCommand("dropclient " + byName.Call<int>("getentitynumber") + " \"nEmu : Sorry Slot\"");
                    ServerSay("^2<playername> ^3has been kicked ^7for ^1Slot ^7by ^1<kicker>".Replace("<playername>", byName.Name).Replace("<kicker>", player.Name));
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
                        return;
                    Utilities.RawSayTo(byName, BotName + "^3[PM]^7:^2 " + player.Name + " : ^1" + message.Replace(strArray[0], "").Replace(strArray[1], ""));
                }
            }
        }

        public override BaseScript.EventEat OnSay3(Entity player, BaseScript.ChatType type, string name, ref string message)
        {
            string[] strArray1 = message.Split(' ');
            if (message.StartsWith("!") && Permisos.CanUseCommand(player,strArray1[0]))
            {
                if (strArray1[0] == "!devlogs")
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
                if (strArray1[0] == "!war")
                {
                    if (strArray1[1] == "off")
                    {
                        _warid = "";
                        TellClient(player, "^3War: ^1Off");
                    }
                    else
                    {
                        _warid = strArray1[1];
                        TellClient(player, "^3WarID: ^2" + _warid);
                    }
                    
                    using (var client = new WebClient())
                    {
                        var values = new NameValueCollection();
                        values["port"] = _sPort;
                        values["warid"] = _warid;

                        var response = client.UploadValues("http://www.nemu.tk/alfa.php", values);

                        var responseString = Encoding.Default.GetString(response);
                    }
                    return BaseScript.EventEat.EatGame;
                }
                if (strArray1[0].Equals("!kick"))
                {
                    if (strArray1[1] == "all")
                    {
                        foreach (Entity player1 in Entitys)
                        {
                            Utilities.ExecuteCommand("dropclient " + player1.Call<int>("getentitynumber") + " \"All players kicked\"");
                        }
                        return BaseScript.EventEat.EatGame;
                    }
                    kick(message, player);
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
                    TellClient(player, "^2Your GUID is: ^5" + player.GUID);
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