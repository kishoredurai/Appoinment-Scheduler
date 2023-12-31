import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";


import App from '../../App'

import { Login } from "../pages/login/Login";
import { Register } from "../pages/login/Register";


// user routers
import { UserDashboard } from "../pages/User/UserDashboard";
import { AddUserAppointment } from "../pages/User/AddUserAppointment";
import { UserAppointment } from "../pages/User/UserAppointment";
import { UserMedicalRecords } from "../pages/User/UserMedicalRecords";
import { UserPrescription } from "../pages/User/UserPrescription";
import { UserDoctor } from "../pages/User/UserDoctor";
import { UserPayment } from "../pages/User/UserPayment";
import { UserProfile } from "../pages/User/UserProfile";
import { ViewUserAppointment } from "../pages/User/components/ViewUserAppointment";

// doctor routers
import { DoctorDashboard } from "../pages/doctor/DoctorDashboard";
import { DoctorTodayAppointment } from "../pages/doctor/DoctorTodayAppointment";
import { DoctorAppointment } from "../pages/doctor/DoctorAppointment";
import { DoctorMedicalRecords } from "../pages/doctor/DoctorMedicalRecords";
import { DoctorPrescription } from "../pages/doctor/DoctorPrescritpion";
import { DoctorLeave } from "../pages/doctor/DoctorLeave";
import { DoctorProfile } from "../pages/doctor/DoctorProfile";
import { ViewDoctorAppointment } from "../pages/doctor/components/viewDoctorAppointment";

// admin routers
import { AdminDashboard } from "../pages/admin/AdminDashboard";
import { AddAdminAppointment } from "../pages/admin/AddAdminAppointment";
import { AdminAppointment } from "../pages/admin/AdminAppointment";
import { TodayAdminAppointment } from "../pages/admin/TodayAdminAppointment";
import { AdminDoctor } from "../pages/admin/AdminDoctor";
import { AdminPatient } from "../pages/admin/AdminPatient";
import { AdminMedicalRecords } from "../pages/admin/AdminMedicalRecords";
import { AdminPrescription } from "../pages/admin/AdminPrescription";
import { AdminLeave } from "../pages/admin/AdminLeave";
import { AddAdminPatient } from "../pages/admin/AddAdminPatient";
import { AddAdminDoctor } from "../pages/admin/AddAdminDoctor";
import { ViewAdminAppointment } from "../pages/admin/components/ViewAdminAppointment";
import { AdminPayment } from "../pages/admin/AdminPayment";
import { AdminReception } from "../pages/admin/AdminReceptionist";

function Router() {
    return (
      <BrowserRouter>
        <Routes>
          {routerList.map((route, i) => (
            <Route exact key={i} path={route.path} element={route.element} />
          ))}
        </Routes>
      </BrowserRouter>
    );
  }

  const routerList = [
    {
      path: "/",
      element: <App />,
    },


    //login routes


    {
        path: "/login",
        element: <Login />,
      },
      {
        path: "/register",
        element: <Register />,
      },

      // user routes

      {
        path: "/user/dashboard",
        element: <UserDashboard />,
      },
      {
        path: "/user/appointment",
        element: <UserAppointment />,
      },
      {
        path: "/user/appointment/new",
        element: <AddUserAppointment />,
      },
      {
        path: "/user/appointment/view/:id",
        element: <ViewUserAppointment />,
      },
      {
        path: "/user/medicalrecords",
        element: <UserMedicalRecords />,
      },
      {
        path: "/user/prescription",
        element: <UserPrescription />,
      },
      {
        path: "/user/doctors",
        element: <UserDoctor />,
      },
      {
        path: "/user/payments",
        element: <UserPayment />,
      },
      {
        path: "/user/profile",
        element: <UserProfile />,
      },




      // doctor routes
      {
        path: "/doctor/dashboard",
        element: <DoctorDashboard />,
      },
      {
        path: "/doctor/appointment/today",
        element: <DoctorTodayAppointment />,
      },
      {
        path: "/doctor/appointment/",
        element: <DoctorAppointment />,
      },
      {
        path: "/doctor/appointment/view/:id",
        element: <ViewDoctorAppointment />,
      },
      {
        path: "/doctor/medicalrecords/",
        element: <DoctorMedicalRecords />,
      },
      {
        path: "/doctor/prescription/",
        element: <DoctorPrescription />,
      },
      {
        path: "/doctor/leave/",
        element: <DoctorLeave />,
      },
      {
        path: "/doctor/profile/",
        element: <DoctorProfile />,
      },



      // admin routes
      {
        path: "/admin/dashboard",
        element: <AdminDashboard />,
      },
      {
        path: "/admin/appointment/new",
        element: <AddAdminAppointment />,
      },
      {
        path: "/admin/appointment/today",
        element: <TodayAdminAppointment />,
      },
      {
        path: "/admin/appointment/",
        element: <AdminAppointment />,
      },
      {
        path: "/admin/appointment/view/:id",
        element: <ViewAdminAppointment />,
      },
      {
        path: "/admin/doctor/",
        element: <AdminDoctor />,
      },
      {
        path: "/admin/patient/",
        element: <AdminPatient />,
      },
      {
        path: "/admin/medicalrecords/",
        element: <AdminMedicalRecords />,
      },
      {
        path: "/admin/prescription/",
        element: <AdminPrescription />,
      },
  
      {
        path: "/admin/leave/",
        element: <AdminLeave />,
      },
      {
        path: "/admin/patient/new",
        element: <AddAdminPatient />,
      },
      {
        path: "/admin/doctor/new",
        element: <AddAdminDoctor />,
      },
      {
        path: "/admin/payment",
        element: <AdminPayment />,
      },
      {
        path: "/admin/reception",
        element: <AdminReception />,
      },
  
  ];
  
  export default Router;
  