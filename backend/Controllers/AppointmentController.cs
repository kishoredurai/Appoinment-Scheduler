﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppoinmentScheduler.Data;
using AppoinmentScheduler.Model;
using System.Reflection;
using AppoinmentScheduler.service;
using Hangfire;

namespace AppoinmentScheduler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly AppoinmentSchedulerContext _context;
        private readonly IRecurringJobManager _recurringJobManager;


        public AppointmentController(AppoinmentSchedulerContext context, IRecurringJobManager recurringJobManager)
        {
            _context = context;
            _recurringJobManager = recurringJobManager;
        }

        // GET: api/Appointment
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppointmentModel>>> GetAppointmentModel()
        {
          if (_context.AppointmentModel == null)
          {
              return NotFound();
          }
            return await _context.AppointmentModel.Include(x=>x.Doctor).Include(x => x.Patient).ToListAsync();
        }

        // GET: api/Appointment/patient
        [HttpGet("patient/{id}")]
        public async Task<ActionResult<IEnumerable<AppointmentModel>>> GetAppointmentModelbypatient(int id)
        {
            if (_context.AppointmentModel == null)
            {
                return NotFound();
            }
            return await _context.AppointmentModel.Include(x => x.Doctor).Where(x=>x.Patient.PatientId==id).ToListAsync();
        }



        // GET: api/Appointment/doctor
        [HttpGet("doctor/{id}")]
        public async Task<ActionResult<IEnumerable<AppointmentModel>>> GetAppointmentModelbydoctor(int id)

        {


            if (_context.AppointmentModel == null)
            {
                return NotFound();
            }
            return await _context.AppointmentModel.Include(x => x.Patient).Where(x => x.Doctor.DoctorId == id).ToListAsync();
        }


        // GET: api/Appointment/doctor
        [HttpGet("doctor/today/{id}")]
        public async Task<ActionResult<IEnumerable<AppointmentModel>>> GettodayAppointmentModelbydoctor(int id)
        {
            DateTime today = DateTime.Today;


            if (_context.AppointmentModel == null)
            {
                return NotFound();
            }
            return await _context.AppointmentModel.Include(x => x.Patient).Where(x => x.Doctor.DoctorId == id && x.AppointmentDate == today).ToListAsync();
        }


        // GET: api/Appointment/doctor
        [HttpGet("today")]
        public async Task<ActionResult<IEnumerable<AppointmentModel>>> GettodayAppointmentModel()
        {
            DateTime today = DateTime.Today;


            if (_context.AppointmentModel == null)
            {
                return NotFound();
            }
            return await _context.AppointmentModel.Include(x => x.Patient).Where(x=> x.AppointmentDate == today).ToListAsync();
        }


      

        [NonAction]
        public async Task<OkResult> GettodayAppointmentModeldaily()
        {
            DateTime today = DateTime.Today;
          
            var app = await _context.AppointmentModel.Include(x=>x.Patient).Include(x=>x.Doctor).Where(x => x.AppointmentDate == today && x.AppointmentStatus !="cancelled").ToListAsync();

            foreach(var  item in app)
            {
                Console.WriteLine(item.Patient.PatientEmail);
                string message = "Today you have an appointment with Dr." + item.Doctor.DoctorName + " at " + item.AppointmentTime + "So kindly be on time";
                MailService.sendConfirmationEmail($"⏰ Reminder for Appointment on {((DateTime)item.AppointmentDate).ToString("dd/MM/yyyy")} 📆" , message, item.Patient.PatientEmail);

            }

            Console.WriteLine("hi");

            return Ok();
            
        }

        [NonAction]
        public async Task<OkResult> changeoldappointments()
        {
            DateTime yesterday = DateTime.Today.AddDays(-1);

            var app = await _context.AppointmentModel.Include(x => x.Patient).Include(x => x.Doctor).Where(x => x.AppointmentDate == yesterday && x.AppointmentStatus == "booked" || x.AppointmentStatus == "waiting").ToListAsync();

            foreach (var item in app)
            {
                Console.WriteLine(item.Patient.PatientEmail);

                AppointmentModel appointment = item;

                appointment.AppointmentStatus = "cancelled";
                appointment.AppointmentRemark = "Appointment expired !!";

                _context.Entry(appointment).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                   
                }

                string message = "Your appointment with Dr." + item.Doctor.DoctorName + " at " + item.AppointmentTime + "has been cancelled.";
                MailService.sendConfirmationEmail($"❌ Cancellation for Appointment on {((DateTime)item.AppointmentDate).ToString("dd/MM/yyyy")} 📆", message, item.Patient.PatientEmail);

            }

            Console.WriteLine("cancelled");

            return Ok();

        }







        // GET: api/Appointment/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentModel>> GetAppointmentModel(int id)
        {
          if (_context.AppointmentModel == null)
          {
              return NotFound();
          }
            var appointmentModel = await _context.AppointmentModel.Include(x=>x.Patient).Include(x=>x.Doctor).Where(x=>x.AppointmentId==id).FirstOrDefaultAsync();

            if (appointmentModel == null)
            {
                return NotFound();
            }

            return appointmentModel;
        }

        // PUT: api/Appointment/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAppointmentModel(int id, AppointmentModel appointmentModel)
        {
            if (id != appointmentModel.AppointmentId)
            {
                return BadRequest();
            }

            _context.Entry(appointmentModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }




        // PUT: api/Appointment/5
       // cancel the appointment
        [HttpPut("Reception/{id}")]
        public async Task<IActionResult> ReceptionCancelAppointmentModel(int id)
        {

            var appointmentModel = await _context.AppointmentModel.FindAsync(id);


            if (id != appointmentModel.AppointmentId)
            {
                return BadRequest();
            }

            appointmentModel.AppointmentStatus = "cancelled";
            appointmentModel.AppointmentRemark = "Cancelled by receptionist";

            _context.Entry(appointmentModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }



        // PUT: api/Appointment/5
        // cancel the appointment by patient
        [HttpPut("Doctor/{id}")]
        public async Task<IActionResult> DoctorsCancelAppointmentModel(int id)
        {

            var appointmentModel = await _context.AppointmentModel.FindAsync(id);


            if (id != appointmentModel.AppointmentId)
            {
                return BadRequest();
            }

            appointmentModel.AppointmentStatus = "cancelled";
            appointmentModel.AppointmentRemark = "Cancelled by Doctor";

            _context.Entry(appointmentModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }



        // PUT: api/Appointment/5
        // cancel the appointment by patient
        [HttpPut("Patient/{id}")]
        public async Task<IActionResult> PatientCancelAppointmentModel(int id)
        {

            var appointmentModel = await _context.AppointmentModel.FindAsync(id);


            if (id != appointmentModel.AppointmentId)
            {
                return BadRequest();
            }

            appointmentModel.AppointmentStatus = "cancelled";
            appointmentModel.AppointmentRemark = "Cancelled by Patient";

            _context.Entry(appointmentModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }





        // PUT: api/Appointment/5
        // cancel the appointment by patient
        [HttpPut("payment/{id}")]
        public async Task<IActionResult> paymentstatusupdate(int id)
        {

            var appointmentModel = await _context.AppointmentModel.FindAsync(id);


            if (id != appointmentModel.AppointmentId)
            {
                return BadRequest();
            }

            appointmentModel.PaymentStatus = "paid";

            _context.Entry(appointmentModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }







        // POST: api/Appointment
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AppointmentModel>> PostAppointmentModel(AppointmentModel appointmentModel)
        {
          if (_context.AppointmentModel == null)
          {
              return Problem("Entity set 'AppoinmentSchedulerContext.AppointmentModel'  is null.");
            }

            var app = await _context.AppointmentModel.Where(x => x.Doctor.DoctorId == appointmentModel.Doctor.DoctorId && x.AppointmentDate == appointmentModel.AppointmentDate && x.AppointmentTime == appointmentModel.AppointmentTime &&x.AppointmentStatus != "cancelled").FirstOrDefaultAsync();

            if(app != null)
            {
                return Problem("Appointment Already exsist");
            }

            var app2 = await _context.AppointmentModel.Where(x => x.Patient.PatientId==appointmentModel.Patient.PatientId&& x.Doctor.DoctorId == appointmentModel.Doctor.DoctorId && x.AppointmentDate == appointmentModel.AppointmentDate && x.AppointmentStatus != "cancelled").FirstOrDefaultAsync();

            if (app2 != null)
            {
                return Problem("Already have an appointment with doctor");
            }




            var patientModel = await _context.PatientModel.Where(x => x.PatientId == appointmentModel.Patient.PatientId).FirstOrDefaultAsync();
            var doctorModel = await _context.DoctorModel.Where(x => x.DoctorId == appointmentModel.Doctor.DoctorId).FirstOrDefaultAsync();

            appointmentModel.Patient = patientModel;
            appointmentModel.Doctor = doctorModel;

            _context.AppointmentModel.Add(appointmentModel);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAppointmentModel", new { id = appointmentModel.AppointmentId }, appointmentModel);
        }

        // DELETE: api/Appointment/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointmentModel(int id)
        {
            if (_context.AppointmentModel == null)
            {
                return NotFound();
            }
            var appointmentModel = await _context.AppointmentModel.FindAsync(id);
            if (appointmentModel == null)
            {
                return NotFound();
            }

            _context.AppointmentModel.Remove(appointmentModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }



        private bool AppointmentModelExists(int id)
        {
            return (_context.AppointmentModel?.Any(e => e.AppointmentId == id)).GetValueOrDefault();
        }
    }
}
