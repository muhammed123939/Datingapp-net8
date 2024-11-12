import { Component, inject, Input, OnInit, output } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, ValidatorFn, Validators } from '@angular/forms';
import { AccountService } from '../_services/account.service';
import { NgIf } from '@angular/common';
import { TextInputComponent } from "../_forms/text-input/text-input.component";
import { DatePickerComponent } from "../_forms/date-picker/date-picker.component";
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule, TextInputComponent, DatePickerComponent],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent implements OnInit {
  private accountService = inject(AccountService);
  private fb= inject(FormBuilder); //using reactive forms
  private router=inject(Router);

  cancelregister = output<boolean>();
  registerForm:FormGroup = new FormGroup({});
  maxDate=new Date();
  validationErrors:string[] | undefined;

  ngOnInit(): void {
    this.initializeForm();
    this.maxDate.setFullYear(this.maxDate.getFullYear()-18)
  }

  initializeForm(){
    this.registerForm=this.fb.group({
      gender:['male'] ,
      knownAs:['' , Validators.required] ,
      dateOfBirth:['' , Validators.required] ,
      city:['' , Validators.required] ,
      country:['' , Validators.required] ,
      username:['' , Validators.required] , 
      password:['', [Validators.required , Validators.minLength(4) ,Validators.maxLength(8)]] , 
      confirmPassword:['' , [Validators.required , this.matchValues('password')]] , 
    });//next part to avoid changes in password text
    this.registerForm.controls['password'].valueChanges.subscribe({
      next:()=>this.registerForm.controls['confirmPassword'].updateValueAndValidity()
    })  
  }
  matchValues(matchTo:string):ValidatorFn{
    return (control:AbstractControl)=>{
      return control.value===control.parent?.get(matchTo)?.value ? null : {isMatching:true}
    }
  }
  register() {
    const dob=this.getDateOnly(this.registerForm.get('dateOfBirth')?.value);
    this.registerForm.patchValue({dateOfBirth:dob});
    this.accountService.register(this.registerForm.value).subscribe({
       next: _=> this.router.navigateByUrl('/members') , //7atena_ bdl response 3shan mch hn3ml haga 
      error: error => this.validationErrors=error
    })
  }
  cancel() {
    this.cancelregister.emit(false);
  }
  private getDateOnly(dob:string|undefined){
    if(!dob)return;
    return new Date(dob).toISOString().slice(0,10);
  }
}
