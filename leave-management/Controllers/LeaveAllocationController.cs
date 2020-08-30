using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using leave_management.Contracts;
using leave_management.Data;
using leave_management.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace leave_management.Controllers
{
    public class LeaveAllocationController : Controller
    {

        private readonly ILeaveTypeRepository _leaverepo;
        private readonly ILeaveAllocationRepository _leavealocatrepo;
        private readonly IMapper _mapper;
        private readonly UserManager<Employee> _userManager;
        public LeaveAllocationController(UserManager<Employee> userManager,ILeaveTypeRepository leaverepo,ILeaveAllocationRepository leavelocaterepo, IMapper mapper)
        {
            _userManager = userManager;
            _leaverepo = leaverepo;
            _leavealocatrepo = leavelocaterepo;
            _mapper = mapper;


        }
        // GET: LeaveAllocationController
        public ActionResult Index()
        {
            var leavetypes =_leaverepo.FindAll().ToList();
            var mappedLeaveTypes = _mapper.Map<List<LeaveType>, List<LeaveTypeVm>>(leavetypes);
            var model = new CreateLeaveAllocationVm
            {
                LeaveTypes = mappedLeaveTypes,
                NumberUpdated = 0
            };
            return View(model);
        }
        public ActionResult SetLeave(int id) 
        {
            var leavetype = _leaverepo.FindById(id);
            var employees = _userManager.GetUsersInRoleAsync("Employee").Result;
            foreach (var emp in employees) 
            {
                if (_leavealocatrepo.CheckAllocation(id, emp.Id))
                    continue;
                var allocation = new LeaveAllocationVM
                {
                    DateCreated = DateTime.Now,
                    EmployeeId = emp.Id,
                    LeaveTypeId = id,
                    NumberOfDays = leavetype.DefaultDays,
                    Period = DateTime.Now.Year
                };
                var leavelocation = _mapper.Map<LeaveAllocation>(allocation);
                _leavealocatrepo.Create(leavelocation);
            }
            return RedirectToAction(nameof(Index));
        }
        public ActionResult ListEmployees() 
        {
            var employees = _userManager.GetUsersInRoleAsync("Employee").Result;
            var model = _mapper.Map<List<EmployeeVm>>(employees);
            return View(model);

        }
        // GET: LeaveAllocationController/Details/5
        public ActionResult Details(string id)
        {
            var employee =_mapper.Map<EmployeeVm>(_userManager.FindByIdAsync(id).Result);
            var allocations=_mapper.Map<List<LeaveAllocationVM>>(_leavealocatrepo.GetLeaveAllocationByEmployee(id));
            var model = new viewAllocationVm
            {
                Employee = employee,
                leaveAllocations = allocations
            };
            return View(model);
        }

        // GET: LeaveAllocationController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LeaveAllocationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LeaveAllocationController/Edit/5
        public ActionResult Edit(int id)
        {
            var leaveallocation = _leavealocatrepo.FindById(id);
            var model = _mapper.Map<EditLeaveAllocationVm>(leaveallocation);
            return View(model);
        }

        // POST: LeaveAllocationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditLeaveAllocationVm model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var record = _leavealocatrepo.FindById(model.id);
                //var allocation = _mapper.Map<LeaveAllocation>(model);
                record.NumberOfDays = model.NumberOfDays;
                var isSuccess = _leavealocatrepo.Update(record);
                if (!isSuccess) 
                {
                    ModelState.AddModelError("","Error while saving");
                    return View(model);
                }
                return RedirectToAction(nameof(Details),new {id=model.EmployeeId });
            }
            catch
            {
                return View();
            }
        }

        // GET: LeaveAllocationController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LeaveAllocationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
