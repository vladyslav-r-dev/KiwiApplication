import { Routes } from '@angular/router';
import { Flights } from "./pages/flights/flights";
import { Home } from "./pages/home/home";
import { FlightDetails } from "./pages/flight-details/flight-details";
import { Register } from './pages/register/register';
import { Login } from './pages/login/login';
import { authGuard } from './guards/auth-guard';
import { MyBookings } from './pages/my-bookings/my-bookings';
import { PaymentSuccess } from './payment-success/payment-success';
import { PaymentCancel } from './payment-cancel/payment-cancel';
import { Profile } from './pages/profile/profile';
import { adminGuard } from './guards/admin-guard';
import { Admin } from './pages/admin/admin';

export const routes: Routes = [
  {
    path: 'home',
    component: Home,
    title: 'Home Page',
  },
  {
    path: 'flights',
    component: Flights,
    title: 'Flights Page',
  },
  {
    path: 'flights/:id',
    component: FlightDetails,
    title: 'Flight Details',
  },
  {
    path: 'register',
    component: Register,
    title: 'Register Page',
  },
  {
    path: 'login',
    component: Login,
    title: 'Login Page',
  },
  {
  path: 'bookings',
  component: MyBookings,
  canActivate: [authGuard],
  title: 'My Bookings',
  },
  {
    path: 'payment-success',
    component: PaymentSuccess,
    title: 'Payment Success',
  },
  {
    path: 'payment-cancel',
    component: PaymentCancel,
    title: 'Payment Cancel',
  },
  {
    path: 'profile',
    component: Profile,
    canActivate: [authGuard],
    title: 'Profile Page',
  },
  {
    path: 'admin',
    component: Admin,
    canActivate: [adminGuard],
    title: 'Admin Page',
  }
];
