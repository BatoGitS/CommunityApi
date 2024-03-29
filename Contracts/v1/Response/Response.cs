﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CommunityAPI.Contracts.v1.Response
{
    public class Response<T>
    {
        public Response() { }

        public Response(T response)
        {
            Data = response;
        }

        public T Data { get; set; }
    }
}
