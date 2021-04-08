using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Data.ViewModels
{
    public class ResponseModel
    {
        public ResponseModel(bool succeeded, string result)
        {
            Succeeded = succeeded;
            Result = result;
        }

        public bool Succeeded { get; set; }

        public string Result { get; set; }
    }
}
