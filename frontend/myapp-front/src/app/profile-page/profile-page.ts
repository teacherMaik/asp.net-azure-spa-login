import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-profile-page',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './profile-page.html',
})

export class ProfilePage implements OnInit {

  user: any;

  constructor(private authService: AuthService) { }

  ngOnInit(): void {
    const userProfileString = localStorage.getItem('userProfile');
    if (userProfileString) {
      this.user = JSON.parse(userProfileString);
    }
  }

  logout() {
    localStorage.removeItem('userProfile');
    this.user = null;
    window.location.href = '/'; // Redirect to home page after logout
  }
}
