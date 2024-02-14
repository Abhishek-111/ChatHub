import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  public userName?:string
  constructor(private router:Router) {  }
  ngOnInit(): void {
    this.userName=sessionStorage.getItem('userName')||'';
  }

  public logout(){
    sessionStorage.clear();
    this.router.navigate(['']);
  }
}
