using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Common.Models;
public class PagedResponseKeyset<T>
{
    public int Reference { get; init; }
    public List<T> Data { get; init; }

    // Other properties might be added;

    public PagedResponseKeyset(List<T> data, int reference)
    {
        Data = data;
        Reference = reference;
    }
}

public class PagedResponseOffset<T>
{
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalRecords { get; init; }
    public int TotalPages { get; init; }
    public List<T> Data { get; init; }

    // Other properties might be added;

    public PagedResponseOffset(List<T> data, int pageNumber, int pageSize, int totalRecords)
    {
        Data = data;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalRecords = totalRecords;
        TotalPages = (int)Math.Ceiling((decimal)totalRecords / (decimal)pageSize);
    }
}
public interface IPaginationRequest
{
    string Filters { get; set; }
    int PageNumber { get; set; }
    int PageSize { get; }
    string Search { get; }
    string Sorts { get; }
}

public class PaginationRequest : IPaginationRequest
{
    public PaginationRequest()
    {
    }

    public PaginationRequest(IPaginationRequest request) :
        this(request.PageNumber, request.PageSize, request.Sorts, request.Filters, request.Search)
    { }

    public PaginationRequest(int pageNumber, int pageSize, string sorts = "", string filters = "", string search = "")
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        Sorts = sorts;
        Filters = filters;
        Search = search;
    }

    private int pageNumber = 1;

    [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
    public int PageNumber
    {
        get => pageNumber;
        set
        {
            if (value < 1)
                pageNumber = 1;
            else
                pageNumber = value;
        }
    }

    private int pageSize = 10;

    [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
    public int PageSize
    {
        get => pageSize;
        set
        {
            if (value < 10)
                pageSize = 10;
            else if (value > 10000)
                pageSize = 10000;
            else
                pageSize = value;
        }
    }

    public string Filters { get; set; }
    public string Sorts { get; set; }
    public string Search { get; set; }
}