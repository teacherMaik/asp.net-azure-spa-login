import { Component, inject } from '@angular/core';
import { AuthService } from '../auth.service';
import { Router } from '@angular/router';


@Component({
    selector: 'app-login',
    standalone: true,
    imports: [],
    templateUrl: './login.html',
})

export class Login {

    private authService = inject(AuthService);
    private router = inject(Router);

    onLogin(providerName: string) {

        this.authService.login(providerName).subscribe({
                next: (userProfile) => {
                    console.log('Login successful:', userProfile);
                    localStorage.setItem('userProfile', JSON.stringify(userProfile));
                    this.router.navigate(['/profile']);
                },
                error: (err) => {
                    console.error('Login failed', err);
                    alert("Check your browser console (F12) - likely a CORS issue!");
                }
        })
    }
}
