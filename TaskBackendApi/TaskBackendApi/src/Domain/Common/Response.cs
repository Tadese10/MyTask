using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common;
public class Response<T>
{
    public Response()
    {
    }
    public Response(T data, string message)
    {
        Successful = true;
        Message = message;
        Data = data;
        Code = (int)HttpStatusCode.OK;
    }
    public Response(string message)
    {
        Successful = false;
        Message = message;
        Code = (int)HttpStatusCode.BadRequest;
    }
    public bool Successful { get; set; }
    public string Message { get; set; }
    public List<string> Errors { get; set; }
    public T Data { get; set; }
    public int Code { get; set; }
}
