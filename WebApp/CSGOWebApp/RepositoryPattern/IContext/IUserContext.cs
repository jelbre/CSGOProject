using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RepositoryPattern
{
    public interface IUserContext
    {
        User GetByID(int ID);

        int LoginWithPass(User user);

        int LoginWithSteam(User user);

        int RegisterPass(User user);

        int RegisterSteam(User user);

        bool UpdateCooldown(User user, Moneyfaucet moneyfaucet, DateTime cooldown);

        bool UpdateLoginData(User user, string type);

        string GetPassword(User user);

        bool Delete(User user);
    }
}
