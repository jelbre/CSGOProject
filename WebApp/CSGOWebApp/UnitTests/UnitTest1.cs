using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepositoryPattern;
using Models;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        CoinflipRepository coinflipRepository = new CoinflipRepository(CoinflipFactory.GetContext(ContextType.MSSQL));
        SkinRepository skinRepository = new SkinRepository(SkinFactory.GetContext(ContextType.MSSQL));
        UserRepository userRepository = new UserRepository(UserFactory.GetContext(ContextType.MSSQL));
        MoneyfaucetRepository moneyfaucetRepository = new MoneyfaucetRepository(MoneyfaucetFactory.GetContext(ContextType.MSSQL));

        User user = new User("UnitTestUsername", "UnitTestPassword");

        [TestMethod]
        public void UnitTests()
        {
            int userID = userRepository.RegisterPass(user);

            Assert.AreEqual(userID > 0, true);

            User testUser = userRepository.GetByID(userID);

            Assert.AreEqual(testUser.Username, "UnitTestUsername");

            testUser = null;
            testUser = userRepository.GetByID(userRepository.LoginWithPass(user));

            Assert.AreEqual(testUser.Username, "UnitTestUsername");
        }
    }
}
