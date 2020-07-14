using System;
using System.Collections.Generic;
using System.Text;

namespace ithaca_api
{
    public class ResponseError
    {
        public ResponseError()
        {
            version = "1.0.1";
            status = 409;
        }

        public string version { get; set; }
        public int status { get; set; }
        public string userMessage { get; set; }
    }
}
