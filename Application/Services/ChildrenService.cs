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
        private IClaimService _claim;

        public ChildrenService(IUnitOfWork unitOfWork, IMapper mapper, IClaimService claim)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _claim = claim;
        }
        public async Task<ApiResponse> AddNewChildren(ChildrenRequest childrentRequest)
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                var claim = _claim.GetUserClaim();
                var children = _mapper.Map<Children>(childrentRequest);
                children.AccountId = claim.Id;
                var childrenExist = await _unitOfWork.Childrens.GetAsync(x => x.Id == children.Id);
                if(childrenExist == null)
                {
                    await _unitOfWork.Childrens.AddAsync(children);
                    await _unitOfWork.SaveChangeAsync();
                    int childrenId = children.Id;
                    return apiResponse.SetOk(childrenId);
                }
                return apiResponse.SetBadRequest("Children already exist!!!");
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
                return apiResponse.SetOk("Deleled successfully!");


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

        public async Task<ApiResponse> UpdateChildrenData(int Id,ChildrenUpdateRequest childrenUpdateRequest)
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                var children = await _unitOfWork.Childrens.GetAsync(c => c.Id == Id);
                if (children == null)
                {
                    return apiResponse.SetNotFound("Can not found the Children detail");
                }
                _mapper.Map(childrenUpdateRequest, children);
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
