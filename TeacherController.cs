using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrudDotnetCore.Models;
using System.Data.SqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CrudDotnetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        // connection string 
        string connString= "Data Source=DESKTOP-OJJJCKJ; Initial Catalog=Rahul; User ID= sa; Password=Sa@2016";
        List<TeacherModel> teachers = new List<TeacherModel>();


        //GET: api/<TeacherController>
        [HttpGet]
        public IActionResult GetTeacher()
        {
            string query = "select * from teacher";
            using (SqlConnection con = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            teachers.Add(new TeacherModel
                            {
                                Id = Convert.ToInt32(sdr["Id"]),
                                Teacher_Name = Convert.ToString(sdr["Teacher_Name"]),
                                Teacher_Email = Convert.ToString(sdr["Teacher_Email"]),
                                Teacher_ContactNo = Convert.ToString(sdr["Teacher_ContactNo"]),
                                Teacher_Address = Convert.ToString(sdr["Teacher_Address"]),
                                Teacher_Department = Convert.ToString(sdr["Teacher_Department"])
                            });
                        }
                    }
                    con.Close();
                }
            }
            return Ok(teachers);

        }


        // GET api/<TeacherController>/5
        [HttpGet("{id}")]
        public IActionResult GetTeacherById(int id)
        {
            TeacherModel teacher = new TeacherModel();
            using (SqlConnection con = new SqlConnection(connString))
            {
                string query = "select * from teacher where Id=" + id;
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = con;
                        con.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                teacher = new TeacherModel
                                {
                                    Id = Convert.ToInt32(sdr["Id"]),
                                    Teacher_Name = Convert.ToString(sdr["Teacher_Name"]),
                                    Teacher_Email = Convert.ToString(sdr["Teacher_Email"]),
                                    Teacher_ContactNo = Convert.ToString(sdr["Teacher_ContactNo"]),
                                    Teacher_Address = Convert.ToString(sdr["Teacher_Address"]),
                                    Teacher_Department = Convert.ToString(sdr["Teacher_Department"])
                                };
                            }
                        }
                        con.Close();
                    }

                }
                if (teacher.Id == 0)
                {
                    return NotFound();
                }
                return Ok(teacher);

            }
        }

        // POST api/<TeacherController>
        [HttpPost]
        public IActionResult InsertRecord(TeacherModel teacher)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(teacher);
            }
            using(SqlConnection con=new SqlConnection(connString))
            {
                //inserting Patient data into database
                string query = "insert into Teacher values (@Teacher_Name, @Teacher_Email, @Teacher_ContactNo,@Teacher_Address,@Teacher_Department)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@Teacher_Name", teacher.Teacher_Name);
                    cmd.Parameters.AddWithValue("@Teacher_Email", teacher.Teacher_Email);
                    cmd.Parameters.AddWithValue("@Teacher_ContactNo", teacher.Teacher_ContactNo);
                    cmd.Parameters.AddWithValue("@Teacher_Address", teacher.Teacher_Address);
                    cmd.Parameters.AddWithValue("@Teacher_Department", teacher.Teacher_Department);
                    con.Open();
                    int i = cmd.ExecuteNonQuery();
                    if (i > 0)
                    {
                        return Ok();
                    }
                    con.Close();
                }
            }
            return BadRequest();

        }

           


        

        // PUT api/<TeacherController>/5
        [HttpPut("{id}")]
        public IActionResult UpdateTeacher(int id, TeacherModel teacherModel)
        {
            if (id != teacherModel.Id)
            {
                return BadRequest();
            }
            TeacherModel teacher = new TeacherModel();
            if (ModelState.IsValid)
            {
                string query = "UPDATE Teacher SET Teacher_Name = @Teacher_Name, Teacher_Email = @Teacher_Email," +
                    "Teacher_ContactNo=@Teacher_ContactNo," +
                    "Teacher_Address=@Teacher_Address,Teacher_Department=@Teacher_Department Where Id =@Id";
                using (SqlConnection con = new SqlConnection(connString))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@Teacher_Name", teacherModel.Teacher_Name);
                        cmd.Parameters.AddWithValue("@Teacher_Email", teacherModel.Teacher_Email);
                        cmd.Parameters.AddWithValue("@Teacher_ContactNo", teacherModel.Teacher_ContactNo);
                        cmd.Parameters.AddWithValue("@Teacher_Address", teacherModel.Teacher_Address);
                        cmd.Parameters.AddWithValue("@Teacher_Department", teacherModel.Teacher_Department);
                        cmd.Parameters.AddWithValue("@Id", teacherModel.Id);
                        con.Open();
                        int i = cmd.ExecuteNonQuery();
                        if (i > 0)
                        {
                            return Ok("Updated successfully ..."+id);
                        }
                        con.Close();
                    }
                }

            }
            return BadRequest(ModelState);

        }

        // DELETE api/<TeacherController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(connString))
            {
                string query = "Delete FROM Teacher where Id='" + id + "'";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    int i = cmd.ExecuteNonQuery();
                    if (i > 0)
                    {
                        return NoContent();
                    }
                    con.Close();
                }
            }
            return BadRequest();
        }
    }
}
