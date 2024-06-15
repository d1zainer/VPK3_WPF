
using piris.DomainService.DbContext;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace piris.DomainService.lpml
{
    public class DatabaseService : IDatabaseService
    {
        public bool AddPosition(store_positions position)
        {
            Console.WriteLine($"Creating position {position.positionName}");
            using (PirisDBEntities2 _dbContext = new PirisDBEntities2())
            {
                _dbContext.store_positions.Add(position);
                _dbContext.SaveChanges();
                return true;
            }
        }

        public bool CreateUser(store_users user)
        {
            Console.WriteLine($"Creating user {user.userName}");
            using (PirisDBEntities2 _dbContext = new PirisDBEntities2())
            {
                var res = _dbContext.store_users.Where(u => u.userName == user.userName).FirstOrDefault();
                if (res == null)
                {
                    store_users dbUsers = new store_users();
                    dbUsers.userName = user.userName;
                    dbUsers.userPassword = user.userPassword;
                    _dbContext.store_users.Add(dbUsers);
                    _dbContext.SaveChanges();
                    return true;
                }
                else { return false; }
            }
        }

        public bool DeletePosition(int id)
        {
            Console.WriteLine($"Trying to delete position {id}");
            using (PirisDBEntities2 _dbContext = new PirisDBEntities2())
            {
                var res = _dbContext.store_positions.Where(p => p.ID == id).FirstOrDefault();
                if (res != null)
                {
                    _dbContext.store_positions.Remove(res);
                    _dbContext.SaveChanges();
                    return true;
                }
                else { return false; }
            }
        }

        public store_positions GetPositionById(int id)
        {
            Console.WriteLine($"Trying to get position {id}");
            using (PirisDBEntities2 _dbContext = new PirisDBEntities2())
            {
                var res = _dbContext.store_positions.Where(p => p.ID == id).FirstOrDefault();
                return res;
            }
        }

        public List<store_positions> GetPositions()
        {
            Console.WriteLine("Trying to get all positions");
            using (PirisDBEntities2 _dbContext = new PirisDBEntities2())
            {
                var dbPositions = _dbContext.store_positions.ToList();
                return dbPositions;
            }
        }

        public store_users GetUserByUserName(string userName)
        {
            Console.WriteLine($"Trying to get user {userName}");
            using (PirisDBEntities2 _dbContext = new PirisDBEntities2())
            {
                var dbUser = _dbContext.store_users.Where(u => u.userName == userName).FirstOrDefault();
                return dbUser;
            }
        }

        public store_positions UpdatePosition(store_positions position)
        {
            Console.WriteLine($"Trying to update position {position.positionName}");
            using (PirisDBEntities2 _dbContext = new PirisDBEntities2())
            {
                var dbPosition = _dbContext.store_positions.Where(p => p.ID == position.ID).FirstOrDefault();
                if (dbPosition != null)
                {
                    _dbContext.Entry(dbPosition).CurrentValues.SetValues(position);
                    _dbContext.SaveChanges();
                }
                return dbPosition;
            }
        }
    }
}
