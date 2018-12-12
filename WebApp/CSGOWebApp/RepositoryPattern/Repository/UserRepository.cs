using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPattern
{
    public class UserRepository
    {
        IUserContext context;
        public UserRepository(IUserContext Context)
        {
            context = Context;
        }

        public int LoginWithPass(User user)
        {
            try
            {
                return context.LoginWithPass(user);
            }
            catch
            {
                return -1;
            }
        }

        public int LoginWithSteam(User user)
        {
            try
            {
                return context.LoginWithSteam(user);
            }
            catch
            {
                return -1;
            }
        }

        public int RegisterPass(User user)
        {
            try
            {
                return context.RegisterPass(user);
            }
            catch
            {
                return -1;
            }
        }

        public int Registersteam(User user)
        {
            try
            {
                return context.RegisterSteam(user);
            }
            catch
            {
                return -1;
            }
        }

        public User GetByID(int ID)
        {
            try
            {
                return context.GetByID(ID);
            }
            catch
            {
                return null;
            }
        }

        public bool UpdateCooldown(User user, Moneyfaucet moneyfaucet, DateTime cooldown)
        {
            return context.UpdateCooldown(user, moneyfaucet, cooldown);
        }

        public bool UpdateLoginData(User user, string type)
        {
            return context.UpdateLoginData(user, type);
        }

        public string GetPassword(User user)
        {
            return context.GetPassword(user); 
        }

        public bool Delete(User user)
        {
            return context.Delete(user);
        }
    }
}
