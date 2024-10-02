using Microsoft.AspNetCore.Http;
using System;

namespace designsphere_identity.STS.Identity.CustomGrants.Models;

public class ApiResponse
{
    public string Version { get; set; }

    public int StatusCode { get; set; }

    public string Message { get; set; }

    public bool? IsError { get; set; }

    public Exception ResponseException { get; set; }

    public object Result { get; set; }

    public ApiResponse(Exception ex)
    {
        IsError = true;
        ResponseException = ex;
        Message = ex.Message;
        StatusCode = StatusCodes.Status500InternalServerError;
    }

    public ApiResponse(string message)
    {
        Message = message;
        IsError = true;
        StatusCode = StatusCodes.Status400BadRequest;
    }

    public ApiResponse(string message, object result)
    {
        Message = message;
        Result = result;
        IsError = false;
        StatusCode = StatusCodes.Status200OK;
    }

    public ApiResponse(string message, object result, bool err)
    {
        Message = message;
        Result = result;
        IsError = err;
        StatusCode = StatusCodes.Status400BadRequest;
    }
}
