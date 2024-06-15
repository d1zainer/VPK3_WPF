using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace piris.DomainService
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени интерфейса "IPositionService" в коде и файле конфигурации.
    [ServiceContract]
    public interface IPositionService
    {
        [OperationContract]
        void DoWork();
    }
}
