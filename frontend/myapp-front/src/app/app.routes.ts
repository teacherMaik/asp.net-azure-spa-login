import { Routes } from '@angular/router';
import { Home } from './home/home';
import { Login } from './login/login';
import { ProfilePage } from './profile-page/profile-page';

export const routes: Routes = [
    { path: '', component: Home },
    { path: 'login', component: Login },
    { path: 'profile', component: ProfilePage },
    { path: '', redirectTo: '/login', pathMatch: 'full' }
];
