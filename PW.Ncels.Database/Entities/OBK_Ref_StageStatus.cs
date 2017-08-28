using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PW.Ncels.Database.DataModel
{
    public partial class OBK_Ref_StageStatus
    {
        /// <summary>
        /// Статус "На распределение"
        /// </summary>
        public const string New = "inQueue";
        /// <summary>
        /// Статус "В работе"
        /// </summary>
        public const string InWork = "inWork";
        /// <summary>
        /// Статус "На даработке"
        /// </summary>
        public const string InReWork = "inReWork";
        /// <summary>
        /// Статус "Выполнен"
        /// </summary>
        public const string Completed = "completed";
    }
}
