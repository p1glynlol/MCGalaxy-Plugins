//Poorly written, might need to improve this soon.
using System;
using System.Collections.Generic;
using MCGalaxy;

namespace VerifyNames {
    public class VerifyNames : Plugin {  
        public override string creator { get { return "p1glynlol"; } }
        public override string MCGalaxy_Version { get { return "1.9.2.8"; } }
        public override string name { get { return "VerifyNames"; } }

        public override void Load(bool startup) {
            Command.Register(new CmdVerifyNames());
        }
        
        public override void Unload(bool shutdown) {
        	Command.Unregister(Command.Find("VerifyNames"));
        }
    }
    
    public class CmdVerifyNames : Command2 {
        public override string name { get { return "VerifyNames"; } }
        public override string shortcut { get { return "OnlineMode"; } }
        public override string type { get { return CommandTypes.Moderation; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Owner; } }

        public override void Use(Player p, string message, CommandData data) {
            string[] args = message.SplitSpaces();
            string cmd    = args[0];
            
            if (cmd.CaselessEq("on")) {
                SetMode(true,  "&aON"); return;
            } else if (cmd.CaselessEq("off")) {
                SetMode(false, "&cOFF"); return;
            }
        }

        static void SetMode(bool enabled, string desc) {
            Server.Config.VerifyNames = enabled;
            SrvProperties.Save();
            Chat.MessageAll("Verify names is now " + desc);
            Logger.Log(LogType.SystemActivity, "Verify names is now " + desc);

        }
        
        public override void Help(Player p) {
            p.Message("&T/VerifyNames on/off");
            p.Message("&HSets whether cracked/offline mode players can join the server.");
        }
    }
}
