//Recreation of the HttpsTest used in "[MCGalaxy] Free Op rank anarchy server" from a decompiled DLL copy (pre-compiled copy was deleted), along with some several changes like the command switch so it can be tested separately.
//DISCONTINUED! This command is now discontinued since DulmCube is now defunct (good riddance I guess?) and BetaCraft discontinuing V1 heartbeat, hence requiring a plugin.

//reference System.dll;
//reference System.Net.Http.dll
using System;
using System.IO;
using System.Net;
using MCGalaxy;
using System.Text;

namespace HttpsTest {
	public sealed class CmdHttpsTest : Command2 {
        public override string name { get { return "HttpsTest"; } }
        public override string shortcut { get { return "ht"; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Console; } }
        public override string type { get { return "Maintenance"; } }

        public override void Use(Player p, string message, CommandData data) {
                        string[] args = message.SplitSpaces();
                        string cmd    = args[0];
                        string result = null;

                        if (cmd.CaselessEq("BetaCraft")) {
                            using (WebClient client = new WebClient()) {
                            string url = "https://betacraft.uk/heartbeat.jsp";
                            result = client.DownloadString(url);
                            p.Message(result);
                            return;
                            }
                        } else if (cmd.CaselessEq("DulmCube")) {
                            using (WebClient client = new WebClient()) {
                            string url = "https://dulm.blue/cube/heartbeat.php";
                            result = client.DownloadString(url);
                            p.Message(result);
                            return;
                            }
                        }
                        p.Message("&T/HttpsTest&H: Use either BetaCraft/DulmCube switch.");
                }

		public override void Help(Player p) {
			p.Message("&T/HttpsTest BetaCraft/DulmCube &H- Test whether HTTPS is working for BetaCraft/DulmCube heartbeats. &cConsole only!");
		}
	}
}