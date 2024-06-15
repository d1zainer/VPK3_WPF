using piris.DomainService.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace piris.DomainService
{
    [ServiceContract]
    public interface IDatabaseService
    {
        [OperationContract]
        List<store_positions> GetPositions();
        [OperationContract]
        bool AddPosition(store_positions position);
        [OperationContract]
        store_positions GetPositionById(int id);
        [OperationContract]
        store_users GetUserByUserName(string userName);
        [OperationContract]
        store_positions UpdatePosition(store_positions
       position);
        [OperationContract]
        bool DeletePosition(int id);
        [OperationContract]
        bool CreateUser(store_users user);
    }
}
