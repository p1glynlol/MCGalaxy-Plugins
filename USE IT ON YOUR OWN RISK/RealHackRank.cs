using System;
using MCGalaxy.Tasks;
using MCGalaxy.Events;
using MCGalaxy.Events.PlayerEvents;

namespace MCGalaxy.Commands.Info
{
	public class RealHackRank : Plugin
	{
		public override string name { get { return "Top2"; } }
		public override string MCGalaxy_Version { get { return "1.9.0.7"; } }
		public override string creator { get { return "p1glynlol"; } }

		public override void Load(bool startup)
		{
			Command.Unregister(Command.Find("HackRank"));
			Command.Register(new CmdHackRank2());
		}
        
		public override void Unload(bool shutdown)
		{
			Logger.Log(LogType.Warning, "&cRestart the server to prevent problems!");
      Command.Unregister(new CmdHackRank2());
		}
	}
	
	
	public class CmdHackRank2 : Command2 {
        public override string name { get { return "HackRank"; } }
        public override string type { get { return CommandTypes.other; } }
        public override bool SuperUseable { get { return true; } }
        
        public override void Use(Player p, string message, CommandData data) {
            if (message.Length == 0) { Help(p); return; }
            
            if (p.hackrank) {
                p.Message("&WYou have already hacked a rank!"); return;
            }
            
            Group grp = Matcher.FindRanks(p, message);
            if (grp == null) return;
            DoRealRank(p, grp);
        }

        void DoRealRank(Player p, Group newRank) {
            p.realrank = true;
            CmdRank.Donewrank(p, p, newRank);
            DoKick(p, newRank);
        }

        void DoKick(Player p, Group newRank) {
            if (!Server.Config.HackrankKicks) return;
            HackRankArgs args = new HackRankArgs();
            args.name = p.name; args.newRank = newRank;
            
            Server.MainScheduler.QueueOnce(HackRankCallback, args, 
                                           Server.Config.HackrankKickDelay);
        }
        
        void HackRankCallback(SchedulerTask task) {
            HackRankArgs args = (HackRankArgs)task.State;
            Player who = PlayerInfo.FindExact(args.name);
            if (who == null) return;

            string msg = "for hacking the rank " + args.newRank.ColoredName;
            who.Leave("kicked (" + msg + "&S)", "Kicked " + msg);
        }
        
        class HackRankArgs { public string name; public Group newRank; }
        
        public override void Help(Player p) {
            p.Message("&T/HackRank [rank] &H- Hacks a rank");
            p.Message("&HTo see available ranks, type &T/ViewRanks");
        }
    }
}
