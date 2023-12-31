﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppoinmentScheduler.Data;
using AppoinmentScheduler.Model;

namespace AppoinmentScheduler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppoinmentSchedulerContext _context;

        public UserController(AppoinmentSchedulerContext context)
        {
            _context = context;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserModel>>> GetUserModel()
        {
          if (_context.UserModel == null)
          {
              return NotFound();
          }
            return await _context.UserModel.Include(x=>x.Doctor).ToListAsync();
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserModel>> GetUserModel(int id)
        {
          if (_context.UserModel == null)
          {
              return NotFound();
          }
            var userModel = await _context.UserModel.FindAsync(id);

            if (userModel == null)
            {
                return NotFound();
            }

            return userModel;
        }

        // PUT: api/User/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserModel(int id, UserModel userModel)
        {
            if (id != userModel.UserId)
            {
                return BadRequest();
            }

            _context.Entry(userModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserModelExists(id))
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

        // POST: api/User
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserModel>> PostUserModel(UserModel userModel)
        {
            //Console.WriteLine("Doctor ID: " + userModel.Doctor.DoctorId);

            if(userModel.UserRole == "admin")
            {

             _context.UserModel.Add(userModel);
             await _context.SaveChangesAsync();

            }
            else
            {

                DoctorModel dm = new DoctorModel();

                dm.DoctorName = userModel.Doctor.DoctorName;
                dm.DoctorQualification = userModel.Doctor.DoctorQualification;
                dm.DoctorDesignation = userModel.Doctor.DoctorDesignation;
                dm.DoctorEmail = userModel.Doctor.DoctorEmail;
                dm.DoctorFees = userModel.Doctor.DoctorFees;
                dm.DoctorMobile = userModel.Doctor.DoctorMobile;
                dm.DoctorAddress = userModel.Doctor.DoctorAddress;
                dm.DoctorStatus = userModel.Doctor.DoctorStatus;




                userModel.Doctor = dm;


                _context.UserModel.Add(userModel);
            await _context.SaveChangesAsync();
            }


            return CreatedAtAction("GetUserModel", new { id = userModel.UserId }, userModel);

            //return NoContent();
        }





        // POST: api/User/login
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("login")]
        public async Task<ActionResult<UserModel>> PostUserLogin(UserModel userModel)
        {
          
            var UserModel = await _context.UserModel.Include(x=>x.Doctor).Where(x=>x.UserEmail==userModel.UserEmail).Where(x=>x.UserPassword==userModel.UserPassword).FirstOrDefaultAsync();

            if (userModel == null)
            {
                return NotFound();
            }

            return UserModel;

            //return NoContent();
        }







        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserModel(int id)
        {
            if (_context.UserModel == null)
            {
                return NotFound();
            }
            var userModel = await _context.UserModel.FindAsync(id);
            if (userModel == null)
            {
                return NotFound();
            }

            _context.UserModel.Remove(userModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserModelExists(int id)
        {
            return (_context.UserModel?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}
