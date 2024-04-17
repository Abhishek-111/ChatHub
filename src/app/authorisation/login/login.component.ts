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

  /**
   * Creates an instance of login component.
   * @param fb 
   * @param authService 
   * @param router 
   */
  constructor(private fb:FormBuilder,private authService:AuthService,private router:Router) { }
  
  /**
   * on init
   */
  ngOnInit() {
    this.createForm();
  }

   /**
   * Creates login form
   */
  private createForm(){
    this.userForm=this.fb.group({
      name:new FormControl('',Validators.required)
    });
  }

  /**
   * Logins to the application
   */
  public formSubmit(){
    
    if(this.userForm.valid){
      this.authService.login(this.userForm.value).subscribe({
        next:(res=>{
          sessionStorage.setItem('userName',this.userForm.controls['name'].value);
          this.router.navigate(['/home']);
        }),
        error:(err=>{
          
        })
      })
    }
  }  

}
