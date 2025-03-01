using Application.Interface;
using Application.Request.Children;
using Application.Response;
using Application.Response.Children;
using AutoMapper;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ChildrenService : IChildenService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ChildrenService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ApiResponse> AddNewChildren(ChildrenRequest childrentRequest)
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                var children = _mapper.Map<Children>(childrentRequest);
                var childrenExist = await _unitOfWork.Childrens.GetAsync(x => x.Id == children.Id);
                if(childrenExist == null)
                {
                    return apiResponse.SetNotFound("Can not found Children");
                }
                await _unitOfWork.Childrens.AddAsync(children);
                await _unitOfWork.SaveChangeAsync();
                return apiResponse.SetOk("Children's details added successfully!");
            }
            catch (Exception e)
            {
                return apiResponse.SetBadRequest(e.Message);
            }
        }

        public async Task<ApiResponse> DeleteChildrenData(int Id)
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                var children = await _unitOfWork.Childrens.GetAsync(c => c.Id == Id);
                if (children == null)
                {
                    return apiResponse.SetNotFound("Can not found the Children detail");
                }
                await _unitOfWork.Childrens.RemoveByIdAsync(Id);
                await _unitOfWork.SaveChangeAsync();
                return apiResponse.SetOk("Delele successfully!");


            }
            catch (Exception e)
            {
                return apiResponse.SetBadRequest(e.Message);
            }
        }

        public async Task<ApiResponse> GetAllChildren()
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                var childrens = await _unitOfWork.Childrens.GetAllAsync(null);
                var resChildrens = _mapper.Map<List<ChildrenResponse>>(childrens);
                return new ApiResponse().SetOk(resChildrens);
            }
            catch (Exception e)
            {
                return apiResponse.SetBadRequest(e.Message);
            }
        }

        public async Task<ApiResponse> UpdateChildrenData(ChildrenUpdateRequest childrenRequest)
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                var children = await _unitOfWork.Childrens.GetAsync(c => c.Id == childrenRequest.Id);
                if (children == null)
                {
                    return apiResponse.SetNotFound("Can not found the Children detail");
                }
                _mapper.Map(childrenRequest, children);
                await _unitOfWork.SaveChangeAsync();
                return apiResponse.SetOk("Children's details updated successfully");

            }
            catch (Exception e)
            {
                return apiResponse.SetBadRequest(e.Message);
            }
        }
    }
}
