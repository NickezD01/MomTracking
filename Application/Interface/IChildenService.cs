using Application.Request.Children;
using Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IChildenService
    {
        Task<ApiResponse> AddNewChildren(ChildrenRequest childrentRequest);
        Task<ApiResponse> GetAllChildren();
        Task<ApiResponse> DeleteChildrenData(int Id);
        Task<ApiResponse> UpdateChildrenData(ChildrenUpdateRequest childrenRequest);
    }
}
