﻿using Application.Interface;
using Application.Request.WHO;
using Application.Response;
using Application.Response.WHO;
using AutoMapper;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class WHOStandardService : IWHOStandardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public WHOStandardService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ApiResponse> AddStandard(StandardRequest standardRequest)
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                var standard = _mapper.Map<WHOStandard>(standardRequest);

                var existStandard = await _unitOfWork.WHOStandards.GetAsync(s => s.PregnancyWeek == standard.PregnancyWeek);
                if (existStandard == null)
                {
                    await _unitOfWork.WHOStandards.AddAsync(standard);

                    await _unitOfWork.SaveChangeAsync();
                    return apiResponse.SetOk("Added successfully!");
                }
                return apiResponse.SetBadRequest("This gestational age standard has been entered!!!");
            }
            catch(Exception e)
            {
                return apiResponse.SetBadRequest(e.Message);
            }
        }

        public async Task<ApiResponse> DeleteStandard(int Id)
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                var standard = await _unitOfWork.WHOStandards.GetAsync(s => s.Id == Id );
                if (standard == null)
                {
                    return apiResponse.SetNotFound("Data does not exist!");
                }
                await _unitOfWork.WHOStandards.RemoveByIdAsync(Id);
                await _unitOfWork.SaveChangeAsync();
                return apiResponse.SetOk("Deleted successfully!");
            }
            catch(Exception e) 
            {
                return apiResponse.SetBadRequest(e.Message);
            }
        }

        public async Task<ApiResponse> GetAllStandard()
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                var standard = await _unitOfWork.WHOStandards.GetAllAsync(null);
                var resStandard = _mapper.Map<List<StandardResponse>>(standard);
                return new ApiResponse().SetOk(resStandard);
            }
            catch (Exception e)
            {
                return apiResponse.SetBadRequest(e.Message);
            }
        }

        public async Task<ApiResponse> UpdateStandard(int Id, StandardRequest standardRequest)
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                var standard = await _unitOfWork.WHOStandards.GetAsync(s => s.Id == Id);
                if (standard == null)
                {
                    return apiResponse.SetNotFound("Data does not exist!");
                }
                _mapper.Map(standardRequest, standard);
                await _unitOfWork.SaveChangeAsync();
                return apiResponse.SetOk("Updated successfully!");
            }
            catch (Exception e)
            {
                return apiResponse.SetBadRequest(e.Message);
            }
        }
    }
}
