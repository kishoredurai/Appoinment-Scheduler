import React from "react";
import { useState } from "react";
import { AdminSidebar } from "../../layout/sidebar/adminsidebar";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import {  toast } from "react-toastify";
import { Backend_Url } from "../../config/connection";
export const AddAdminPatient = () => {
  const navigate = useNavigate();

  const [formData, setFormData] = useState({
    patientAccountStatus:true
  });

  // formsubmit

  function SubmitPatient(e) {
    e.preventDefault();

    axios.post(Backend_Url+'/api/Patient', formData, {
      headers: {
        'Content-Type': 'application/json'
      }
    })
      .then((response) => {
        if (!response.ok) {
          if (response.status === 404) {
            throw new Error('Resource not found');
          }
          if (response.status === 201) {
            return response.data;
          }
          else {
            throw new Error('Network response was not ok');
          }
        }
        return response.data;
      })
      .then((data) => {
        // Handle the response from the server
     
        toast.success('Patient Created ✅', {
          position: "top-right",
          autoClose: 5000,
          hideProgressBar: false,
          closeOnClick: false,
          pauseOnHover: true,
          draggable: true,
          progress: undefined,
          theme: "light",
          });
  
          setTimeout(() => {
            navigate('/admin/patient');
          }, 3000);
    
      })
      .catch((error) => {
        // Handle any errors that occurred during the request
        console.error('Error:', error.message);
      });


  }

  return (
    <>
      <AdminSidebar />

      <div class="p-2 md:p-4 min-h-screen bg-gray-200 sm:ml-64">
        <div class=" p-2 md:p-4 border-2 border-gray-200 border-dashed rounded-lg dark:border-gray-700 mt-14">
          {/* bread crumbs  */}
          <div class="flex w-full mb-4 rounded bg-white dark:bg-gray-800">
            <nav
              class="flex w-full px-5 py-3 text-gray-700 border border-white rounded-lg bg-white dark:bg-gray-800 dark:border-gray-700"
              aria-label="Breadcrumb"
            >
              <ol class="inline-flex items-center space-x-1 md:space-x-3">
                <li class="inline-flex items-center">
                  <a
                    href="#"
                    class="inline-flex items-center text-sm font-medium text-gray-700 hover:text-blue-600 dark:text-gray-400 dark:hover:text-white"
                  >
                    <svg
                      aria-hidden="true"
                      class="w-4 h-4 mr-2"
                      fill="currentColor"
                      viewBox="0 0 20 20"
                      xmlns="http://www.w3.org/2000/svg"
                    >
                      <path d="M10.707 2.293a1 1 0 00-1.414 0l-7 7a1 1 0 001.414 1.414L4 10.414V17a1 1 0 001 1h2a1 1 0 001-1v-2a1 1 0 011-1h2a1 1 0 011 1v2a1 1 0 001 1h2a1 1 0 001-1v-6.586l.293.293a1 1 0 001.414-1.414l-7-7z"></path>
                    </svg>
                    Dashboard
                  </a>
                </li>
                <li>
                  <div class="flex items-center">
                    <svg
                      aria-hidden="true"
                      class="w-6 h-6 text-gray-400"
                      fill="currentColor"
                      viewBox="0 0 20 20"
                      xmlns="http://www.w3.org/2000/svg"
                    >
                      <path
                        fill-rule="evenodd"
                        d="M7.293 14.707a1 1 0 010-1.414L10.586 10 7.293 6.707a1 1 0 011.414-1.414l4 4a1 1 0 010 1.414l-4 4a1 1 0 01-1.414 0z"
                        clip-rule="evenodd"
                      ></path>
                    </svg>
                    <a
                      href="#"
                      class="ml-1 text-sm font-medium text-gray-700 hover:text-blue-600 md:ml-2 dark:text-gray-400 dark:hover:text-white"
                    >
                      Patient
                    </a>
                  </div>
                </li>
                <li aria-current="page">
                  <div class="flex items-center">
                    <svg
                      aria-hidden="true"
                      class="w-6 h-6 text-gray-400"
                      fill="currentColor"
                      viewBox="0 0 20 20"
                      xmlns="http://www.w3.org/2000/svg"
                    >
                      <path
                        fill-rule="evenodd"
                        d="M7.293 14.707a1 1 0 010-1.414L10.586 10 7.293 6.707a1 1 0 011.414-1.414l4 4a1 1 0 010 1.414l-4 4a1 1 0 01-1.414 0z"
                        clip-rule="evenodd"
                      ></path>
                    </svg>
                    <span class="ml-1 text-sm font-medium text-gray-500 md:ml-2 dark:text-gray-400">
                      New
                    </span>
                  </div>
                </li>
              </ol>
            </nav>
          </div>

          {/* form */}
          <div class="flex mb-4 rounded bg-white dark:bg-gray-800">
            <div className="w-full p-3 border border-white rounded-lg">
              <h1 className="text-2xl font-bold mb-6">New Patient</h1>

              <form onSubmit={(e)=>SubmitPatient(e)}>
                <div className="flex flex-wrap mb-4">
                  <label htmlFor="date" className="w-full md:w-1/4">
                    Full Name :
                  </label>
                  <input
                    type="text"
                    onChange={(e) =>
                      setFormData({
                        ...formData,
                        patientName: e.target.value,
                      })
                    }
                    required
                    className="w-full md:w-3/4 px-2 py-1 border border-gray-300 rounded"
                  />
                </div>
                <div className="flex flex-wrap mb-4">
                  <label htmlFor="date" className="w-full md:w-1/4">
                    Age :
                  </label>
                  <input
                    type="text"
                    onChange={(e) =>
                      setFormData({
                        ...formData,
                        patientAge: e.target.value,
                      })
                    }
                    required
                    className="w-full md:w-3/4 px-2 py-1 border border-gray-300 rounded"
                  />
                </div>
                <div className="flex flex-wrap mb-4">
                  <label htmlFor="date" className="w-full md:w-1/4">
                    Date of Birth :
                  </label>
                  <input
                    type="date"
                    onChange={(e) =>
                      setFormData({
                        ...formData,
                        patientMobile: e.target.value,
                      })
                    }
                    required
                    className="w-full md:w-3/4 px-2 py-1 border border-gray-300 rounded"
                  />
                </div>
                <div className="flex flex-wrap mb-4">
                  <label htmlFor="date" className="w-full md:w-1/4">
                    Gender :
                  </label>
                  <select
                    id="date"
                    onChange={(e) =>
                      setFormData({
                        ...formData,
                        patientGender: e.target.value,
                      })
                    }
                    required
                    className="w-full md:w-3/4 px-2 py-1 border border-gray-300 rounded"
                  >
                    <option defaultChecked>select option</option>

                    <option value="Male">Male</option>
                    <option value="Female">Female</option>
                    <option value="others">Others</option>
                  </select>
                </div>
                <div className="flex flex-wrap mb-4">
                  <label htmlFor="date" className="w-full md:w-1/4">
                    Blood Group :
                  </label>
                  <input
                    type="text"
                    onChange={(e) =>
                      setFormData({
                        ...formData,
                        patientBloodGroup: e.target.value,
                      })
                    }
                    required
                    className="w-full md:w-3/4 px-2 py-1 border border-gray-300 rounded"
                  />
                </div>
                <div className="flex flex-wrap mb-4">
                  <label htmlFor="date" className="w-full md:w-1/4">
                    Email :
                  </label>
                  <input
                    type="email"
                    onChange={(e) =>
                      setFormData({
                        ...formData,
                        patientEmail: e.target.value,
                      })
                    }
                    required
                    className="w-full md:w-3/4 px-2 py-1 border border-gray-300 rounded"
                  />
                </div>
                <div className="flex flex-wrap mb-4">
                  <label htmlFor="date" className="w-full md:w-1/4">
                    Mobile Number :
                  </label>
                  <div className="flex w-full md:w-3/4">
                    <input
                      type="number"
                      disabled
                      placeholder="+91"
                      className="w-14 mr-2  px-2 py-1 border border-gray-300 rounded"
                    />
                    <input
                      type="number"
                      onChange={(e) =>
                        setFormData({
                          ...formData,
                          patientMobile: e.target.value,
                        })
                      }
                      required
                      className="w-full  px-2 py-1 border border-gray-300 rounded"
                    />
                  </div>
                </div>
                <div className="flex flex-wrap mb-4">
                  <label htmlFor="email" className="w-full md:w-1/4">
                    Address :
                  </label>
                  <textarea
                    type="text"
                    onChange={(e) =>
                      setFormData({
                        ...formData,
                        patientAddress: e.target.value,
                      })
                    }
                    required
                    row="3"
                    className="w-full md:w-3/4 px-2 py-1 border border-gray-300 rounded"
                  />
                </div>

                <button
                  type="submit"
                  className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600"
                >
                  Submit
                </button>
              </form>
            </div>
          </div>
        </div>
      </div>
    </>
  );
};
