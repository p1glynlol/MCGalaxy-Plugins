// improved version of /top which extends the limit to 30 instead of 15
using System;
using System.Collections.Generic;
using MCGalaxy.DB;

namespace MCGalaxy.Commands.Info
{
	public class Top2 : Plugin
	{
		public override string name { get { return "Top2"; } }
		public override string MCGalaxy_Version { get { return "1.9.4.7"; } } //some of the code is from icanttellyou/TomCube
		public override string creator { get { return "p1glynlol"; } }

		public override void Load(bool startup)
		{
			Command.Unregister(Command.Find("Top"));
			Command.Register(new CmdTop2());
		}
        
		public override void Unload(bool shutdown)
		{
			Logger.Log(LogType.Warning, "&cRestart the server to prevent problems!");
                        Command.Unregister(new CmdTop2());
		}
	}
		
	public class CmdTop2 : Command2 {
        public override string name { get { return "Top"; } }
        public override string shortcut { get { return "Most"; } }
        public override string type { get { return CommandTypes.Information; } }
        public override bool SuperUseable { get { return true; } }
        public override CommandAlias[] Aliases {
            get { return new [] { new CommandAlias("TopTen", "10"), new CommandAlias("TopFive", "5"),
                    new CommandAlias("Top10", "10"), new CommandAlias("Top15", "15"), new CommandAlias("Top30", "30"), }; }
        }
        
         public override void Use(Player p, string message, CommandData data) {
            string[] args = message.SplitSpaces();
            if (args.Length < 2) { Help(p); return; }
            
            int maxResults = 0, offset = 0;
            if (!CommandParser.GetInt(p, args[0], "Max results", ref maxResults, 1, 30)) return;

            TopStat stat = TopStat.Find(args[1]);
            if (stat == null) {
                p.Message("&WNo stat found with name \"{0}\".", args[1]); return;
            }
            
            if (args.Length > 2) {
                if (!CommandParser.GetInt(p, args[2], "Offset", ref offset, 0)) return;
            }
            List<TopResult> results = stat.GetResults(maxResults, offset);
            p.Message("&a{0}:", stat.Title);
		 
            for (int i = 0; i < stats.Count; i++) 
	    {
                p.Message("{0}) {1} &S- {2}", offset + (i + 1),
			  stat.FormatName(p, results[i].Name),
			  stat.Formatter(results[i].Value));
            }
        }
        
        public override void Help(Player p) {
            p.Message("&T/Top [max results] [stat] <offset>");
            p.Message("&HPrints a list of players who have the " +
                       "most/top of a particular stat. Available stats:");
            TopStat.List(p);
        }
    }
}
