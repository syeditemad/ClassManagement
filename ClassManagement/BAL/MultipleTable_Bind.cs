using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassManagement_ModelLibrary.Class_Model;
namespace ClassManagement.BAL
{
    public class MultipleTable_Bind
    {
        public IEnumerable<EmployeePersonelDetails> GetEmployeeDetailsList { get; set; }
        public IEnumerable<NomineeDetails> NomineeDetailsList { get; set; }
        public IEnumerable<Country> GetCountryListView { get; set; }

        public IEnumerable<CountryState> GetCountryStateViewList { get; set; }
    }

   

}
