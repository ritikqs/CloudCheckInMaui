using CCIMIGRATION.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CCIMIGRATION.ConstantHelper
{
    public static class StaticHelpers
    {
        public static bool IsGeofencingMonitoring;
        public static List<ContractorModel> ContractorList = new List<ContractorModel>();
        public static List<EmployeeTypeModel> EmployerList = new List<EmployeeTypeModel>();
        public static ObservableCollection<TradeModel> TradeList = new ObservableCollection<TradeModel>();
        public enum EmployeeType : int
        {
            Employee = 1,
            Supervisor = 2,
            Subcontractor = 3,
            SiteManager = 4,
            Visitor = 5,
            Customer = 6,
            Contractor = 7,
            CollectionDelivery = 8,
            Apprentice = 9,
            Agency = 10,
            Admin = 11
        }
    }
    
}
