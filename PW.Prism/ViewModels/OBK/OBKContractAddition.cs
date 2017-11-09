using PW.Ncels.Database.Models.OBK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PW.Prism.ViewModels.OBK
{
    public class OBKContractAddition
    {
        public OBKContractAddition(OBKContractViewModel contract, OBKContractAdditionViewModel contractAddition)
        {
            Contract = contract;
            ContractAddition = contractAddition;
        }
        public OBKContractViewModel Contract { get; set; }
        public OBKContractAdditionViewModel ContractAddition { get; set; } 
    }
}