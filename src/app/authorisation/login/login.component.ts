import { Component, OnInit } from '@angular/core';
import { Form, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/core/services/auth/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  public userForm!:FormGroup;
  constructor(private fb:FormBuilder,private authService:AuthService,private router:Router) { }
  ngOnInit() {
    this.createForm();
  }

  private createForm(){
    this.userForm=this.fb.group({
      name:new FormControl('',Validators.required)
    });
  }

  public formSubmit(){
    
    if(this.userForm.valid){
      this.authService.login(this.userForm.value).subscribe({
        next:(res=>{
          // sessionStorage.setItem('userId');
          sessionStorage.setItem('userName',this.userForm.controls['name'].value);
          this.router.navigate(['/home']);
        }),
        error:(err=>{
          
        })
      })
    }
  }  

}
