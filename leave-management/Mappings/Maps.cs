using AutoMapper;
using leave_management.Data;
using leave_management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace leave_management.Mappings
{
    public class Maps:Profile
    {
        public Maps() 
        {
            CreateMap<LeaveType, LeaveTypeVm>().ReverseMap();
            //CreateMap<LeaveType, CreateLeaveTypeVm>().ReverseMap();
            CreateMap<LeaveHistory, LeaveHistoryVm>().ReverseMap();
            CreateMap<LeaveAllocation, LeaveAllocationVM>().ReverseMap();
            CreateMap<Employee, EmployeeVm>().ReverseMap();
        }
    }
}
