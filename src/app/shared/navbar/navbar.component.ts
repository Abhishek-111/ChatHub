import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  public userName?:string

  /**
   * Creates an instance of navbar component
   * @param router 
   */
  constructor(private router:Router) {  }

  /**
   * on init
   */
  ngOnInit(): void {
    this.userName=sessionStorage.getItem('userName')||'';
  }

  /**
   * Log out a user
   */
  public logout(){
    sessionStorage.clear();
    this.router.navigate(['']);
  }
}
