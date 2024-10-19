// addded
import { Component } from '@angular/core';
import { RegisterComponent } from "../register/register.component";

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RegisterComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  registermode = false;

  registerToggle() {
    this.registermode = !this.registermode
  }
  cancelRegisterMode(event: boolean) {
    this.registermode = event;
  }

}