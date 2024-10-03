// addded
import { Component, inject, OnInit } from '@angular/core';
import { RegisterComponent } from "../register/register.component";
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RegisterComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {
  registermode = false;
  users: any;
  http = inject(HttpClient);

  ngOnInit(): void {
    this.getUsers();
  }
  registerToggle() {
    this.registermode = !this.registermode
  }
  cancelRegisterMode(event: boolean) {
    this.registermode = event;
  }
  getUsers() {
    this.http.get('http://localhost:5001/api/users').subscribe({
      next: response => this.users = response,
      error: error => console.log(error),
      complete: () => console.log('request has completed')
    })

  }

}