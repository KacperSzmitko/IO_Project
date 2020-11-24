using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServerLibrary;
using Shared;
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
            Assert.AreEqual("Response:True$$", cp.ProccesClient("Option:" + (int)Options.CREATE_USER + "$$Username:tes$$Password:1234$$", id),4);
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
    }
}
