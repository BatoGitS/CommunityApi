using System;
using System.Collections.Generic;
using System.Text;

namespace CommunityAPI.Contracts.v1.Response
{
    public class ErrorModel
    {
        public string FieldName { get; set; }

        public string Message { get; set; }
    }
}
