using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManager.Dtos.Response;

public class GetPagingResponse<T>
{
    public List<T> Data { get; set; } = new List<T>();

    public int TotalPage { get; set; }
}