using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kai_Stranka.Models
{
    public static class IpAddress
    {
        public static string GetIpAddress(this HttpRequest request)
        {
            var emptyValues = default(StringValues);
            StringValues header = request.Headers.FirstOrDefault(h => h.Key == "CF-Connecting-IP").Value;
            if (header == emptyValues) header = request.Headers.FirstOrDefault(h => h.Key == "X-Forwarded-For").Value;
            if (header != emptyValues)
                return header.First();
            else
                return request.HttpContext.Connection.RemoteIpAddress.ToString();
        }
    }
}
