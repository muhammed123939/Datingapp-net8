import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';

@Component({
  selector: 'app-test-errors',
  standalone: true,
  imports: [],
  templateUrl: './test-errors.component.html',
  styleUrl: './test-errors.component.css'
})
export class TestErrorsComponent {
  baseurl = 'http://localhost:5001/api/';
  private http = inject(HttpClient);
  validationErrors: string[] = [];

  get400Error() {
    this.http.get(this.baseurl + 'buggy/bad-request').subscribe({
      next: response => console.log(response),
      error: error => console.log(error)
    })
  }

  get401Error() {
    this.http.get(this.baseurl + 'buggy/auth').subscribe({
      next: response => console.log(response),
      error: error => console.log(error)
    })
  }

  get404Error() {
    this.http.get(this.baseurl + 'buggy/not-found').subscribe({
      next: response => console.log(response),
      error: error => console.log(error)
    })
  }

  get500Error() {
    this.http.get(this.baseurl + 'buggy/server-error').subscribe({
      next: response => console.log(response),
      error: error => console.log(error)
    })
  }

  getvalidationError() {
    this.http.post(this.baseurl + 'account/register', {}).subscribe({
      next: response => console.log(response),
      error: error => {
        console.log(error);
        this.validationErrors = error;
      }
    })
  }

}





