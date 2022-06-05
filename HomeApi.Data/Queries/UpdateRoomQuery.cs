using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeApi.Data.Queries
{
    /// <summary>
    /// Класс для передачи дополнительных параметров при обновлении комнаты
    /// </summary>
    public class UpdateRoomQuery
    {

        public DateTime? NewAddDate { get; }
        public string NewName { get; set; }
        public int? NewArea { get; set; }
        public bool? NewGasConnected { get; set; }
        public int? NewVoltage { get; set; }

        public UpdateRoomQuery(        
            DateTime? newAddDate,
            string newName,
            int? newArea,
            bool? newGasConnected,
            int? newVoltage
        )
        {
            NewAddDate = newAddDate;
            NewName = newName;
            NewArea = newArea;
            NewGasConnected = newGasConnected;
            NewVoltage = newVoltage;
        }

    }
}