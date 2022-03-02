// improved version of /top which extends the limit to 30 instead of 15
using System;
using System.Collections.Generic;
using System.Data;
using MCGalaxy.DB;
using MCGalaxy.SQL;

namespace MCGalaxy.Commands.Info
{
	public class Top2 : Plugin
	{
		public override string name { get { return "Top2"; } }
		public override string MCGalaxy_Version { get { return "1.9.3.8"; } } //some of the code is from icanttellyou/TomCube
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
        public override bool SuperUseable { get { return false; } }
        public override CommandAlias[] Aliases {
            get { return new [] { new CommandAlias("TopTen", "10"), new CommandAlias("TopFive", "5"),
                    new CommandAlias("Top10", "10"), new CommandAlias("Top15", "15"), new CommandAlias("Top30", "30"), }; }
        }
        
         public override void Use(Player p, string message, CommandData data) {
            string[] args = message.SplitSpaces();
            if (args.Length < 2) { Help(p); return; }
            
            int maxResults = 0, offset = 0;
            if (!CommandParser.GetInt(p, args[0], "Max results", ref maxResults, 1, 15)) return;

            TopStat stat = FindTopStat(args[1]);
            if (stat == null) {
                p.Message("&WUnrecognised type \"{0}\".", args[1]); return;
            }
            
            if (args.Length > 2) {
                if (!CommandParser.GetInt(p, args[2], "Offset", ref offset, 0)) return;
            }
            
            string limit = " LIMIT " + offset + "," + maxResults;
            List<string[]> stats = Database.GetRows(stat.Table, "DISTINCT Name, " + stat.Column,
                                                    "ORDER BY" + stat.OrderBy + limit);
            
            p.Message("&a{0}:", stat.Title());
            for (int i = 0; i < stats.Count; i++) {
                string nick  = p.FormatNick(stats[i][0]);
                string value = stat.Formatter(stats[i][1]);
                p.Message("{0}) {1} &S- {2}", offset + (i + 1), nick, value);
            }
        }
        
        static TopStat FindTopStat(string input) {
            foreach (TopStat stat in TopStat.Stats) {
                if (stat.Identifier.CaselessEq(input)) return stat;
            }
            
            int number;
            if (int.TryParse(input, out number)) {
                // Backwards compatibility where top used to take a number
                if (number >= 1 && number <= TopStat.Stats.Count)
                    return TopStat.Stats[number - 1];
            }
            return null;
        }
        
        public override void Help(Player p) {
            p.Message("&T/Top [max results] [stat] <offset>");
            p.Message("&HPrints a list of players who have the " +
                           "most/top of a particular stat. Available stats:");
            p.Message("&f" + TopStat.Stats.Join(stat => stat.Identifier));
        }
    }
}
