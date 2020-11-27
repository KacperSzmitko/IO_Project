using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServerLibrary;
using Shared;
using System;
using System.Collections.Generic;

namespace TestServer
{
    [TestClass]
    public class UnitTest1
    {
        ClientProcesing cp;
        int id;

        [TestMethod]
        public void TestUserCreateMessageGeneration()
        {
             cp = new ClientProcesing();
             id = cp.AddPlayer(new ServerLibrary.Player());
             Assert.AreEqual("Response:True$$", cp.ProccesClient("Option:" + (int)Options.CREATE_USER + "$$Username:tes$$Password:1234$$", id));
/*            List<string> l = new List<string>
            {
                "Option:5$$Username:test$$Password:1234$$",
                "Option:2$$",
                "Option:0$$",
                "Option:2$$",
            };*/
                                  
        }

        [TestMethod]
        public void TestLoginFields()
        {
            try
            {
                //Try to create new user
                TestUserCreateMessageGeneration();
                Assert.AreEqual(4, cp.Login("Username:tes$$Password:1234$$", id).Split("$$").Length);
            }
            catch
            {
                //Try login
                Assert.AreEqual(4, cp.Login("Username:tes$$Password:1234$$", id).Split("$$").Length);
            }
            
        }

        [TestMethod]
        public void TestSendMove()
        {
            cp = new ClientProcesing();
            ServerLibrary.Player p = new ServerLibrary.Player();
            p.matchID = 0;
            
            id = cp.AddPlayer(p);
            cp.ProccesClient("$$5$$Username:tes$$Password:1234$$", id);
            string response = cp.ProccesClient("$$5$$Username:tes$$Password:1234$$", id);
            string[] r = response.Split("$$");
            string sessionID = r[1].Split(":")[1];
            p.sessionId = sessionID;
            cp.games.Add(new Gameplay(p, new Player(), 9, 5));
            response = cp.ProccesClient(String.Format("Option:7$$SessionID:{0}$$Move:1$$", Int32.Parse(sessionID)),id);
            Console.WriteLine(response);
            
        }
    }
}
