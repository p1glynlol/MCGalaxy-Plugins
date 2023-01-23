namespace MCGalaxy.Commands.Moderation {   
    public sealed class CmdUnXban : Command2 {       
        public override string name { get { return "UnXBan"; } }
        public override string shortcut { get { return "uxb"; } }
        public override string type { get { return CommandTypes.Moderation; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public override CommandAlias[] Aliases {
            get { return new [] { new CommandAlias("-noip") }; }
        }
        
        public override void Use(Player p, string message, CommandData data) {
            bool UnbanIP = true;
            if (message.CaselessStarts("-noip ")) {
                message = message.Substring("-noip ".Length);
                UnbanIP = false;
            }            
            if (message.Length == 0) { Help(p); return; }

            string name = message.SplitSpaces()[0];
            if (UnbanIP) Command.Find("UnbanIP").Use(p, "@" + name, data);
            Command.Find("Unban").Use(p, message, data);    
        }

        public override void Help(Player p) {
            p.Message("&T/UnXBan [player] <reason>");
            p.Message("&HUnBans, un IP bans the given player.");
        }
    }
}