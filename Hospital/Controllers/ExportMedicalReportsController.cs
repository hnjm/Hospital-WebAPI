﻿using Hospital.Services.DataServices.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Hospital.Controllers
{
    /// <summary>
    ///
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Doctor")]
    public class ExportMedicalReportsController : ControllerBase
    {
        private readonly IMedicalReportsExportService _medicalReportsExportService;
        private readonly IDoctorsService _doctorService;
        private readonly IPatientsService _patientService;

        /// <summary>
        /// Export MedicalReports controller constructor
        /// </summary>
        public ExportMedicalReportsController(IMedicalReportsExportService medicalReportsExportService, IDoctorsService doctorService, IPatientsService patientService)
        {
            _medicalReportsExportService = medicalReportsExportService;
            _doctorService = doctorService;
            _patientService = patientService;
        }
        /// <summary>
        /// Export medical reports of doctor
        /// </summary>
        [HttpPost("/Doctor/{id}")]
        public async Task<ActionResult> ExportDoctorMedicalReports(Guid id, DateTimeOffset? startDate, DateTimeOffset? endDate)
        {
            if (ModelState.IsValid)
            {
                var doctorName = await _doctorService.GetByIdAsync(id);
                var fileName = doctorName.Name;
                if(startDate.HasValue)
                {
                    fileName += $" {startDate.Value} - ";
                }
                if(endDate.HasValue)
                {
                    fileName += endDate.Value;
                }
                return File(_medicalReportsExportService.ExportDoctorMedicalReports(id, startDate, endDate), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Export medical reports of patient
        /// </summary>
        [HttpPost("/Patient/{id}")]
        [Authorize(Roles = "Doctor, Patient")]
        public async Task<ActionResult> ExportPatientMedicalReports(Guid id, DateTimeOffset? startDate, DateTimeOffset? endDate)
        {
            if (ModelState.IsValid)
            {
                var patientName = await _patientService.GetByIdAsync(id);
                var fileName = patientName.Name;
                if (startDate.HasValue)
                {
                    fileName += $" {startDate.Value} - ";
                }
                if (endDate.HasValue)
                {
                    fileName += endDate.Value;
                }
                return File(_medicalReportsExportService.ExportPatientMedicalReports(id, startDate, endDate), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }

            return BadRequest(ModelState);
        }
    }
}
