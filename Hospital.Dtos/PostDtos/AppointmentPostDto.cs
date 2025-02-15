﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Dtos.PostDtos
{
    public class AppointmentPostDto
    {
        public string Summary { get; set; }
        public string Symptoms { get; set; }
        public DateTimeOffset Date { get; set; }
        public Guid DoctorId { get; set; }
        public Guid PatientId { get; set; }
    }
}
