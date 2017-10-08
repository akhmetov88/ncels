using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PW.Ncels.Database.DataModel
{
    public partial class OBK_Ref_ContractHistoryStatus
    {
        /// <summary>
        /// Отправлен заявителем
        /// </summary>
        public const string SentByApplicant = "sentByApplicant";
        /// <summary>
        /// Распределен
        /// </summary>
        public const string SentToWork = "sentToWork";
        /// <summary>
        /// Соответствует требованиям
        /// </summary>
        public const string MeetsRequirements = "meetsRequirements";
        /// <summary>
        /// Не соответствует требованиям
        /// </summary>
        public const string DoesNotMeetRequirements = "doesNotMeetRequirements";
        /// <summary>
        /// Возвращен на доработку
        /// </summary>
        public const string Returned = "returned";
        /// <summary>
        /// Отправлен на согласование
        /// </summary>
        public const string SentToApproval = "sentToApproval";
        /// <summary>
        /// Отказано в согласовании
        /// </summary>
        public const string Refused = "refused";

    }
}
